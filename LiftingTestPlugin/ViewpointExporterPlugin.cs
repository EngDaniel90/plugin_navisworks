using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Drawing; // Para manipulação de imagens (Bitmap)
using System.Drawing.Imaging; // Para formatos de imagem

using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;

namespace LiftingTestPlugin
{
    // Atributos necessários para o plugin
    [Plugin("ViewpointExporter", "DanielDev", DisplayName = "Exportar Viewpoints", ToolTip = "Exporta todas as SavedViewpoints como imagens para uma pasta.")]
    [AddInPlugin(AddInLocation.AddIn)] // Define que o plugin aparece na aba "Add-ins"
    public class ViewpointExporterPlugin : AddInPlugin
    {
        /// <summary>
        /// Método de entrada do plugin. É chamado quando o botão é clicado.
        /// </summary>
        public override int Execute(params string[] parameters)
        {
            try
            {
                // Verifica se há um documento aberto
                if (Autodesk.Navisworks.Api.Application.ActiveDocument == null || Autodesk.Navisworks.Api.Application.ActiveDocument.IsClear)
                {
                    MessageBox.Show("Nenhum documento aberto.", "Exportar Viewpoints", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 0;
                }

                // 1. Seleção de Pasta
                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    fbd.Description = "Selecione a pasta onde as imagens das viewpoints serão salvas.";
                    fbd.ShowNewFolderButton = true;

                    DialogResult result = fbd.ShowDialog();

                    // Se o usuário cancelar, interrompe a execução
                    if (result != DialogResult.OK || string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        return 0;
                    }

                    string outputDirectory = fbd.SelectedPath;

                    // Obtém o documento ativo
                    Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;

                    // 2. Iteração (Recursiva)
                    // Iniciamos a recursão a partir da coleção raiz de SavedViewpoints
                    ProcessItems(doc.SavedViewpoints, outputDirectory, doc);

                    MessageBox.Show("Exportação concluída com sucesso!", "Exportar Viewpoints", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro durante a exportação:\n{ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return 0;
        }

        /// <summary>
        /// Método recursivo para percorrer pastas e viewpoints.
        /// </summary>
        /// <param name="items">Coleção de itens (podem ser pastas ou viewpoints).</param>
        /// <param name="directory">Diretório de destino atual.</param>
        /// <param name="doc">Documento ativo do Navisworks.</param>
        private void ProcessItems(IEnumerable<SavedItem> items, string directory, Document doc)
        {
            foreach (SavedItem item in items)
            {
                // Se for um Grupo (Pasta), entra recursivamente
                if (item.IsGroup)
                {
                    // Faz o cast para GroupItem para acessar os filhos
                    GroupItem group = item as GroupItem;
                    if (group != null)
                    {
                        // Opcional: Criar subpasta física para organizar as imagens
                        // string subDirectory = Path.Combine(directory, SanitizeFilename(group.DisplayName));
                        // Directory.CreateDirectory(subDirectory);

                        // Chamada recursiva para os itens dentro do grupo
                        // Nota: Se quiser manter tudo na raiz, passe 'directory'.
                        // Se quiser estrutura de pastas, passe 'subDirectory'.
                        // O requisito diz "Iteração: Percorrer... para entrar em pastas", mas não explicita criar subpastas no sistema de arquivos.
                        // Vou manter tudo na pasta selecionada para simplicidade, a menos que o nome colida.
                        // Mas para garantir que processamos tudo:
                        ProcessItems(group.Children, directory, doc);
                    }
                }
                // Se for uma Viewpoint
                else if (item is SavedViewpoint viewpoint)
                {
                    ExportViewpoint(viewpoint, directory, doc);
                }
            }
        }

        /// <summary>
        /// Aplica a viewpoint e exporta a imagem.
        /// </summary>
        private void ExportViewpoint(SavedViewpoint viewpoint, string directory, Document doc)
        {
            try
            {
                // 3. Exportação

                // Ative a viewpoint (aplique-a à câmera atual)
                // É importante copiar a viewpoint para o CurrentSavedViewpoint para ativar a câmera e estados salvos
                doc.SavedViewpoints.CurrentSavedViewpoint = viewpoint;

                // Força o processamento de eventos pendentes para garantir que a vista atualize (opcional, mas recomendado em loops pesados)
                // Application.DoEvents(); // Evitar em plugins modernos se possível, mas útil aqui para atualização visual imediata se necessário.

                // Nomenclatura e Tratamento de Erros
                string safeName = SanitizeFilename(viewpoint.DisplayName);
                string filePath = Path.Combine(directory, $"{safeName}.png");

                // Gera a imagem da vista atual
                // GenerateImage usa as configurações atuais de renderização
                // Altura e largura podem ser fixas ou baseadas na view atual. Usaremos Full HD como padrão ou o tamanho da view.
                // ImageGenerationStyle.Shaded garante que apareça renderizado (não wireframe).

                // Captura o tamanho atual da view para manter proporção ou define um fixo (ex: 1920x1080)
                double width = doc.ActiveView.Width;
                double height = doc.ActiveView.Height;

                // Se width/height forem 0 (janela minimizada), define padrão
                if (width <= 0) width = 1920;
                if (height <= 0) height = 1080;

                using (Bitmap bitmap = doc.ActiveView.GenerateImage(ImageGenerationStyle.Shaded, width, height))
                {
                    if (bitmap != null)
                    {
                        bitmap.Save(filePath, ImageFormat.Png);
                    }
                }
            }
            catch (Exception ex)
            {
                // Em caso de erro numa viewpoint específica, loga ou ignora, mas não para todo o processo
                Console.WriteLine($"Erro ao exportar {viewpoint.DisplayName}: {ex.Message}");
            }
        }

        /// <summary>
        /// Remove caracteres inválidos para nomes de arquivos.
        /// </summary>
        private string SanitizeFilename(string name)
        {
            string invalidChars = new string(Path.GetInvalidFileNameChars());
            string sanitized = name;

            foreach (char c in invalidChars)
            {
                sanitized = sanitized.Replace(c.ToString(), "_");
            }

            return sanitized;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Drawing; // Para manipulação de imagens (Bitmap)
using System.Drawing.Imaging; // Para formatos de imagem
using System.Diagnostics; // Para Process

using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using Autodesk.Navisworks.Api.Interop.ComApi; // Bridge to COM
using ComApi = Autodesk.Navisworks.ComApi; // COM API alias

namespace ViewpointExporter
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
                        // Chamada recursiva para os itens dentro do grupo
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
        /// Aplica a viewpoint e exporta a imagem usando COM Interop.
        /// </summary>
        private void ExportViewpoint(SavedViewpoint viewpoint, string directory, Document doc)
        {
            try
            {
                // 3. Exportação

                // Ative a viewpoint (aplique-a à câmera atual)
                doc.SavedViewpoints.CurrentSavedViewpoint = viewpoint;

                // Força o processamento para garantir que a vista atualize visualmente antes da captura
                // Essencial para loops de renderização como este.
                System.Windows.Forms.Application.DoEvents();

                // Nomenclatura
                string safeName = SanitizeFilename(viewpoint.DisplayName);
                string filePath = Path.Combine(directory, $"{safeName}.png");

                // Exporta Imagem via Screen Capture (Abordagem robusta para "Plugins Simples")
                try
                {
                    // Obter Handle da Janela do Navisworks para identificar o monitor correto
                    IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;

                    // Obter a tela onde a janela está localizada
                    Screen screen = Screen.FromHandle(handle);
                    Rectangle bounds = screen.Bounds;

                    using(Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                    {
                        using(Graphics g = Graphics.FromImage(bitmap))
                        {
                            // Copia a tela correta (onde o Navisworks está)
                            g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
                        }

                        // Nota: Isso captura a tela cheia do monitor.
                        // Para capturar APENAS a janela do Navisworks, seria necessário código Win32 complexo (GetWindowRect).
                        // Como fallback para uma IA, capturar o monitor ativo do processo é muito mais seguro que Point.Empty (Monitor Primário).

                        bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
                catch (Exception comEx)
                {
                     Console.WriteLine($"Erro ao exportar imagem: {comEx.Message}");
                }

            }
            catch (Exception ex)
            {
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

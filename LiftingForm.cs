using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

// APIs Padrão do Navisworks (.NET)
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Clash;
using Autodesk.Navisworks.Api.Plugins;

// API de Ponte (Permite converter objetos .NET para COM)
using Autodesk.Navisworks.Api.ComApi; 

// API COM (Sistema antigo/interno do Navisworks - Necessário para mover objetos em NWD)
// O Alias "ComApi" evita conflitos de nome
using ComApi = Autodesk.Navisworks.Interop.ComApi;

namespace AutoLiftingClashAnalysis
{
    public partial class LiftingForm : Form
    {
        // Fator de conversão: Navisworks usa PÉS internamente.
        // 1 Metro = 3.28084 Pés
        private const double MeterToFoot = 3.2808399;

        public LiftingForm()
        {
            InitializeComponent();
            LoadSetFolders();
        }

        private void LoadSetFolders()
        {
            try
            {
                cmbSetsFolder.Items.Clear();
                Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
                
                // Verificação de segurança se o documento está carregado
                if (doc == null || doc.IsClear) return;

                if (doc.SelectionSets != null && doc.SelectionSets.RootItem != null)
                {
                    foreach (var item in doc.SelectionSets.RootItem.Children)
                    {
                        // Só listamos Grupos (Pastas) no ComboBox
                        if (item.IsGroup)
                        {
                            cmbSetsFolder.Items.Add(item.DisplayName);
                        }
                    }
                }

                if (cmbSetsFolder.Items.Count > 0)
                    cmbSetsFolder.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar pastas: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnRun_Click(object sender, EventArgs e)
        {
            if (cmbSetsFolder.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione uma pasta válida.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // --- TRAVA A INTERFACE (Para o usuário não clicar duas vezes) ---
            btnRun.Enabled = false;
            cmbSetsFolder.Enabled = false;
            txtHeight.Enabled = false;
            txtStep.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
                string folderName = cmbSetsFolder.SelectedItem.ToString();

                // --- VALIDAÇÃO DOS NÚMEROS ---
                if (!double.TryParse(txtHeight.Text, out double userHeightMeters) ||
                    !double.TryParse(txtStep.Text, out double userStepMeters))
                {
                    MessageBox.Show("Valores inválidos. Use apenas números.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (userStepMeters <= 0)
                {
                    MessageBox.Show("O passo de descida deve ser maior que zero.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // --- BUSCA A PASTA SELECIONADA ---
                SavedItem folderItem = doc.SelectionSets.RootItem.Children.FirstOrDefault(x => x.DisplayName == folderName);
                if (folderItem == null) throw new Exception($"Pasta '{folderName}' não encontrada.");

                GroupItem folderGroup = folderItem as GroupItem;
                if (folderGroup == null) throw new Exception("O item selecionado não é uma pasta válida.");

                // Pega os módulos dentro da pasta (ignora subpastas)
                var modules = folderGroup.Children.Where(x => !x.IsGroup).ToList();
                if (modules.Count == 0)
                {
                    MessageBox.Show("A pasta selecionada está vazia.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // --- CONFIGURAÇÃO VISUAL ---
                int stepsPerModule = (int)(userHeightMeters / userStepMeters) + 2; 
                progressBar.Maximum = modules.Count * stepsPerModule;
                progressBar.Value = 0;

                List<string> issues = new List<string>();
                var clashPlugin = doc.GetClash();
                var testsData = clashPlugin.TestsData;

                // --- LOOP PRINCIPAL (MÓDULO POR MÓDULO) ---
                foreach (SavedItem module in modules)
                {
                    lblStatus.Text = $"Analisando: {module.DisplayName}";
                    await Task.Delay(50); // Pequena pausa para a tela atualizar

                    SelectionSet set = module as SelectionSet;
                    if (set == null) continue;

                    // 1. OBTER ITENS
                    // Precisamos resolver a seleção (seja Busca ou Seleção Explícita)
                    ModelItemCollection itemsToMove = new ModelItemCollection();
                    if (set.HasSearch)
                    {
                        try 
                        { 
                            itemsToMove.CopyFrom(set.Search.FindAll(doc, false)); 
                        }
                        catch { continue; } 
                    }
                    else
                    {
                        itemsToMove.CopyFrom(set.GetSelectedItems(doc));
                    }

                    if (itemsToMove.Count == 0) continue;

                    // 2. CRIAR O TESTE DE CLASH
                    ClashTest newTest = new ClashTest();
                    // Nome único com GUID para evitar erros de "Nome já existe"
                    newTest.DisplayName = $"Lift_{module.DisplayName}_{Guid.NewGuid().ToString().Substring(0, 4)}";
                    
                    newTest.SelectionA.Selection.CopyFrom(itemsToMove);
                    newTest.SelectionB.Selection.SelectAll(); // Contra todo o modelo
                    
                    newTest.Tolerance = 0.05 * MeterToFoot; // Tolerância 5cm
                    newTest.TestType = ClashTestType.Hard;
                    newTest.Status = ClashTestStatus.New;

                    testsData.Tests.Add(newTest);
                    
                    // Pega a referência "viva" do teste dentro do Navisworks
                    ClashTest activeTest = (ClashTest)testsData.Tests[testsData.Tests.Count - 1];

                    // 3. RODAR SIMULAÇÃO
                    bool collisionFound = await RunSimulation(doc, itemsToMove, activeTest, userHeightMeters, userStepMeters);

                    if (collisionFound)
                    {
                        issues.Add(module.DisplayName);
                    }
                }

                // --- FINALIZAÇÃO ---
                progressBar.Value = progressBar.Maximum;
                lblStatus.Text = "Concluído.";

                if (issues.Count > 0)
                    MessageBox.Show($"Colisões detectadas nos módulos:\n{string.Join("\n", issues)}", "Relatório de Colisões", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show("Sucesso! Nenhuma colisão detectada.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro Geral: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Destrava a interface
                btnRun.Enabled = true;
                cmbSetsFolder.Enabled = true;
                txtHeight.Enabled = true;
                txtStep.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private async Task<bool> RunSimulation(Document doc, ModelItemCollection items, ClashTest test, double heightMeters, double stepMeters)
        {
            double currentZMeters = heightMeters;
            bool collisionDetected = false;
            var clashTestsData = doc.GetClash().TestsData;
            ComApi.InwOpState10 state = ComApiBridge.State;
            ComApi.InwOpSelection comSelection = ComApiBridge.ToInwOpSelection(items);

            try
            {
                // Mover para o TOPO (Posição Inicial)
                MoveItemsUsingCOM(state, comSelection, currentZMeters);

                // LOOP DE DESCIDA
                while (currentZMeters >= 0)
                {
                    await Task.Delay(50); // Delay crítico para o Navisworks renderizar a geometria nova

                    try 
                    {
                        // Roda o Clash Test na posição atual
                        clashTestsData.TestsRunTest(test);
                    }
                    catch { /* Ignora erros momentâneos do motor de clash */ }

                    // Verifica Resultados
                    foreach (SavedItem resultItem in test.Children)
                    {
                        ClashResult cr = resultItem as ClashResult;
                        if (cr != null && (cr.Status == ClashResultStatus.New || cr.Status == ClashResultStatus.Active))
                        {
                            collisionDetected = true;
                            break;
                        }
                    }

                    if (collisionDetected)
                    {
                        break;
                    }

                    // Prepara próximo passo
                    currentZMeters -= stepMeters;

                    if (currentZMeters >= 0)
                    {
                        // Atualiza a posição visual
                        MoveItemsUsingCOM(state, comSelection, currentZMeters);
                    }

                    if (progressBar.Value < progressBar.Maximum)
                    {
                        progressBar.Increment(1);
                    }
                }
            }
            finally
            {
                // RESET FINAL: Garante que o objeto volte ao lugar original
                ResetItemsUsingCOM(state, comSelection);
            }

            return collisionDetected;
        }

        // ====================================================================================
        // MÉTODOS DE MANIPULAÇÃO VISUAL (COM API)
        // Estes métodos acessam o núcleo do Navisworks para mover objetos sem alterar o arquivo NWD (evita Read-Only)
        // ====================================================================================

        private void MoveItemsUsingCOM(ComApi.InwOpState10 state, ComApi.InwOpSelection comSelection, double zMeters)
        {
            try
            {
                // 3. Criar Objeto de Transformação 3D
                ComApi.InwLTransform3f transform = (ComApi.InwLTransform3f)state.ObjectFactory(ComApi.nwEObjectType.eObjectType_nwLTransform3f, null, null);
                
                // 4. Definir Translação (Z em Pés)
                double zFeet = zMeters * MeterToFoot;
                transform.MakeTranslation(0, 0, (float)zFeet);

                // 5. Aplicar Override Visual
                state.OverrideTransform(comSelection, transform);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Erro COM Move: " + ex.Message);
            }
        }

        private void ResetItemsUsingCOM(ComApi.InwOpState10 state, ComApi.InwOpSelection comSelection)
        {
            try
            {
                // Para resetar, criamos uma transformação "Identidade" (sem movimento)
                ComApi.InwLTransform3f transform = (ComApi.InwLTransform3f)state.ObjectFactory(ComApi.nwEObjectType.eObjectType_nwLTransform3f, null, null);
                transform.MakeIdentity(); 

                // Aplicando a identidade, removemos o deslocamento anterior
                state.OverrideTransform(comSelection, transform);
            }
            catch (Exception ex) 
            {
                System.Diagnostics.Debug.WriteLine("Erro COM Reset: " + ex.Message);
            }
        }
    }
}
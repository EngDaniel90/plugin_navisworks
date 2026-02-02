using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Clash;
using Autodesk.Navisworks.Api.Plugins;

namespace LiftingTestPlugin
{
    public partial class LiftingForm : Form
    {
        // Conversion factor: 1 Meter = 3.2808399 Feet
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
                Document doc = Application.ActiveDocument;
                if (doc == null || doc.IsClear) return;

                if (doc.SelectionSets.RootItem != null)
                {
                    foreach (var item in doc.SelectionSets.RootItem.Children)
                    {
                        // Check if it is a Group (Folder)
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
                MessageBox.Show("Erro ao carregar pastas de Sets: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (cmbSetsFolder.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione uma pasta válida.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Disable UI
            btnRun.Enabled = false;
            cmbSetsFolder.Enabled = false;
            txtHeight.Enabled = false;
            txtStep.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                Document doc = Application.ActiveDocument;
                string folderName = cmbSetsFolder.SelectedItem.ToString();

                // Validate Inputs
                if (!double.TryParse(txtHeight.Text, out double userHeightMeters) ||
                    !double.TryParse(txtStep.Text, out double userStepMeters))
                {
                    MessageBox.Show("Por favor, insira valores numéricos válidos.", "Erro de Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (userStepMeters <= 0)
                {
                    MessageBox.Show("O passo de descida deve ser maior que zero.", "Erro de Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Find Folder
                SavedItem folderItem = doc.SelectionSets.RootItem.Children.FirstOrDefault(x => x.DisplayName == folderName);
                if (folderItem == null)
                {
                    MessageBox.Show($"Pasta '{folderName}' não encontrada.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Get only selection sets (exclude sub-groups)
                var modules = folderItem.Children.Where(x => !x.IsGroup).ToList();
                if (modules.Count == 0)
                {
                    MessageBox.Show("A pasta selecionada está vazia ou não contém Sets.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Setup Progress Bar
                int stepsPerModule = (int)(userHeightMeters / userStepMeters) + 2; // +2 buffer
                progressBar.Maximum = modules.Count * stepsPerModule;
                progressBar.Value = 0;

                List<string> issues = new List<string>();

                // Get Clash Plugin
                var clashPlugin = doc.GetClash();
                var testsData = clashPlugin.TestsData;

                foreach (SavedItem module in modules)
                {
                    if (progressBar.Value >= progressBar.Maximum) progressBar.Value = progressBar.Maximum - 1; // Prevent overflow

                    lblStatus.Text = $"Processando módulo: {module.DisplayName}";
                    Application.DoEvents();

                    SelectionSet set = module as SelectionSet;
                    if (set == null) continue;

                    // 1. Create or Find Clash Test
                    // We create a new one to avoid conflicts
                    ClashTest test = new ClashTest();
                    test.DisplayName = $"Lift_{module.DisplayName}";

                    // Configure Selection A (The Module)
                    test.SelectionA.Selection.CopyFrom(set.Search);

                    // Configure Selection B (The Whole Model)
                    test.SelectionB.Selection.SelectAll();

                    // Tolerance: 5cm = 0.05m -> Converted to Feet
                    test.Tolerance = 0.05 * MeterToFoot;
                    test.TestType = ClashTestType.Hard;
                    test.Status = ClashTestStatus.New;

                    testsData.Tests.AddTest(test);

                    // 2. Simulation
                    bool collisionFound = RunSimulation(doc, set, test, userHeightMeters, userStepMeters);

                    if (collisionFound)
                    {
                        issues.Add(module.DisplayName);
                    }
                }

                progressBar.Value = progressBar.Maximum;
                lblStatus.Text = "Concluído.";

                if (issues.Count > 0)
                {
                    MessageBox.Show($"Processo finalizado.\n\nColisões detectadas nos seguintes módulos:\n{string.Join("\n", issues)}", "Relatório de Colisões", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Processo finalizado com SUCESSO!\nNenhuma colisão detectada durante o içamento.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro inesperado durante a execução:\n{ex.Message}", "Erro Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRun.Enabled = true;
                cmbSetsFolder.Enabled = true;
                txtHeight.Enabled = true;
                txtStep.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private bool RunSimulation(Document doc, SelectionSet set, ClashTest test, double heightMeters, double stepMeters)
        {
            ModelItemCollection items = set.GetSelectedItems(doc);
            if (items.Count == 0) return false;

            double currentZMeters = heightMeters;
            bool collisionDetected = false;

            try
            {
                // Initial Move to Top (+Z)
                double zFeet = currentZMeters * MeterToFoot;
                Vector3D vec = new Vector3D(0, 0, zFeet);
                doc.Models.OverridePermanentTransform(items, Transform3D.CreateTranslation(vec), true);

                // Simulation Loop
                while (currentZMeters >= 0)
                {
                    Application.DoEvents();

                    // 1. Run Clash Test
                    doc.GetClash().TestsData.TestsRunTest(test);

                    // 2. Check for Results
                    // We check if the test has any 'Active' or 'New' results at this position.
                    foreach (var result in test.Children)
                    {
                        ClashResult cr = result as ClashResult;
                        if (cr != null && (cr.Status == ClashResultStatus.New || cr.Status == ClashResultStatus.Active))
                        {
                            collisionDetected = true;
                            // We could break here if we just want to know IF there is a collision,
                            // but usually we want to record the full path or at least continue the visual simulation.
                            // For this requirement, we just flag it.
                        }
                    }

                    // 3. Move Down
                    currentZMeters -= stepMeters;

                    // Prepare next position
                    if (currentZMeters >= 0) // Avoid moving below ground if loop condition allows one last check
                    {
                        zFeet = currentZMeters * MeterToFoot;
                        vec = new Vector3D(0, 0, zFeet);
                        doc.Models.OverridePermanentTransform(items, Transform3D.CreateTranslation(vec), true);
                    }

                    if (progressBar.Value < progressBar.Maximum) progressBar.Increment(1);
                }
            }
            finally
            {
                // Reset Final (Return to original position)
                doc.Models.OverridePermanentTransform(items, Transform3D.Identity, true);
            }

            return collisionDetected;
        }
    }
}

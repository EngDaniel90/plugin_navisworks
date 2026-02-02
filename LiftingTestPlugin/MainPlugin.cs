using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;

namespace LiftingTestPlugin
{
    [Plugin("LiftingTest", "DanielDev", DisplayName = "Lifting Test", ToolTip = "Realiza Análise Dinâmica de Içamento")]
    public class MainPlugin : AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {
            try
            {
                // Ensure we have an active document
                if (Application.ActiveDocument == null || Application.ActiveDocument.IsClear)
                {
                    MessageBox.Show("Nenhum documento aberto.", "Lifting Test", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 0;
                }

                // Using ShowDialog to block execution until the form is closed.
                // This ensures the plugin stays alive during the user interaction.
                using (LiftingForm form = new LiftingForm())
                {
                    form.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro crítico no plugin: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Erro - Lifting Test", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return 0;
        }
    }
}

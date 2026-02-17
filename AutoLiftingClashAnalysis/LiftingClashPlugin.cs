using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;

namespace AutoLiftingClashAnalysis
{
    [Plugin("AutoLiftingClashAnalysis", "DanielDev", DisplayName = "Auto Lifting Clash Analysis", ToolTip = "Realiza Análise Dinâmica de Içamento e Detecção de Colisões")]
    public class LiftingClashPlugin : AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {
            try
            {
                // Ensure we have an active document
                // Using full namespace to avoid ambiguity if System.Windows.Forms is also used heavily
                if (Autodesk.Navisworks.Api.Application.ActiveDocument == null || Autodesk.Navisworks.Api.Application.ActiveDocument.IsClear)
                {
                    MessageBox.Show("Nenhum documento aberto.", "Auto Lifting Clash Analysis", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show($"Erro crítico no plugin: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Erro - Auto Lifting Clash Analysis", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;

namespace AutoLiftingClashAnalysis
{
    // O atributo 'Plugin' define a identidade
    [Plugin("AutoLiftingClashAnalysis", "DanielDev", DisplayName = "Auto Lifting", ToolTip = "Análise de Içamento")]
    // O atributo 'AddInPlugin' define ONDE o botão aparece (Aba Add-ins -> Ferramentas)
    [AddInPlugin(AddInLocation.AddIn)]
    public class LiftingClashPlugin : AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {
            try
            {
                if (Autodesk.Navisworks.Api.Application.ActiveDocument == null || Autodesk.Navisworks.Api.Application.ActiveDocument.IsClear)
                {
                    MessageBox.Show("Nenhum documento aberto.", "Auto Lifting", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 0;
                }

                // Usando 'using' para garantir que o formulário seja limpo da memória depois
                using (LiftingForm form = new LiftingForm())
                {
                    form.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}", "Erro do Plugin", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return 0;
        }
    }
}
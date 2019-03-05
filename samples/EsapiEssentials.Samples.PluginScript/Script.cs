using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using EsapiEssentials.Plugin;
using VMS.TPS.Common.Model.Types;

namespace VMS.TPS
{
    public class Script : ScriptBase
    {
        public override void Execute(PluginScriptContext context)
        {
            // Define the desired structure ID
            var structureId = GetStructureIdFromSettings();

            // Find the structure in the current structure set
            var structure = context?.StructureSet?.Structures?.FirstOrDefault(x => x.Id == structureId);

            if (structure == null)
                throw new InvalidOperationException("Unable to find the structure.");

            // Calculate D2cc[Gy]
            var d2cc = context?.PlanSetup?.GetDoseAtVolume(structure, 2, VolumePresentation.AbsoluteCm3, DoseValuePresentation.Absolute);

            if (d2cc == null)
                throw new InvalidOperationException("Unable to calculate the dose metric.");

            // Show the result
            MessageBox.Show($"{structureId} D2cc[Gy] = {d2cc}");
        }

        private string GetStructureIdFromSettings()
        {
            var settings = new AssemblySettings(Assembly.GetExecutingAssembly());
            return settings.GetSetting("StructureId");
        }
    }
}

using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using EsapiEssentials.Plugin;
using VMS.TPS.Common.Model.Types;

namespace VMS.TPS
{
    // This script demonstrates deriving from ScriptBase in order to use the PluginRunner,
    // which needs to be referenced from a separate WPF project.

    // This script also demonstrates using AssemblySettings in order to obtain
    // the desired structure ID from the App.config file.
    // This file is not automatically created in a class library project,
    // so it must be added manually to the project (via Add New Item).
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

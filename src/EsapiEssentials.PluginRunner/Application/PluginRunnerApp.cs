using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using VMS.TPS.Common.Model.API;
using Application = VMS.TPS.Common.Model.API.Application;

namespace EsapiEssentials.PluginRunner
{
    internal class PluginRunnerApp
    {
        private const string ApplicationName = "External Beam Planning";
        private const string VersionInfo = "13.6.32";

        private readonly ScriptBase _script;

        private Application _esapiApp;
        private PatientSummarySearch _search;

        public PluginRunnerApp(ScriptBase script)
        {
            _script = script;
        }

        public void LogInToEsapi()
        {
            _esapiApp = Application.CreateApplication(null, null);
            _search = new PatientSummarySearch(_esapiApp.PatientSummaries, 20);
        }

        public void LogOutFromEsapi()
        {
            _esapiApp.Dispose();
        }

        public IEnumerable<PatientMatch> FindPatientMatchesAsync(string searchText) =>
            _search.FindMatches(searchText).Select(CreatePatientMatch);

        public IEnumerable<PlanOrPlanSum> GetPlansAndPlanSumsFor(string patientId)
        {
            var patient = _esapiApp.OpenPatientById(patientId);

            // Must do ToArray before closing patient
            var plansAndPlanSums = patient.GetPlanningItems().Select(CreatePlanOrPlanSum).ToArray();

            _esapiApp.ClosePatient();

            return plansAndPlanSums;
        }

        public void RunScript(string patientId, IEnumerable<PlanOrPlanSum> plansAndPlanSumsInScope, PlanOrPlanSum activePlan)
        {
            try
            {
                var patient = _esapiApp.OpenPatientById(patientId);
                var context = CreateScriptContext(patient, plansAndPlanSumsInScope, activePlan);

                var window = new Window();
                _script.Execute(context, window);
                window.ShowDialog();
            }
            catch (Exception e)
            {
                // Mimic Eclipse by showing a message box on a script Exception
                MessageBox.Show(e.Message, ApplicationName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            finally
            {
                _esapiApp.ClosePatient();
            }
        }

        private PluginScriptContext CreateScriptContext(Patient patient, IEnumerable<PlanOrPlanSum> plansAndPlanSumsInScope, PlanOrPlanSum activePlan)
        {
            var planningItems = patient.GetPlanningItems();
            var planningItemsInScope = plansAndPlanSumsInScope.Select(p => FindPlanningItem(p, planningItems));
            var planSetup = activePlan != null ? FindPlanningItem(activePlan, planningItems) as PlanSetup : null;

            return new PluginScriptContext
            {
                CurrentUser = _esapiApp.CurrentUser,
                Course = planSetup?.Course,
                Image = planSetup?.StructureSet?.Image,
                StructureSet = planSetup?.StructureSet,
                Patient = patient,
                PlanSetup = planSetup,
                ExternalPlanSetup = planSetup as ExternalPlanSetup,
                BrachyPlanSetup = planSetup as BrachyPlanSetup,
                PlansInScope = planningItemsInScope.Where(p => p is PlanSetup).Cast<PlanSetup>(),
                ExternalPlansInScope = planningItemsInScope.Where(p => p is ExternalPlanSetup).Cast<ExternalPlanSetup>(),
                BrachyPlansInScope = planningItemsInScope.Where(p => p is BrachyPlanSetup).Cast<BrachyPlanSetup>(),
                PlanSumsInScope = planningItemsInScope.Where(p => p is PlanSum).Cast<PlanSum>(),
                ApplicationName = ApplicationName,
                VersionInfo = VersionInfo
            };
        }

        private PlanningItem FindPlanningItem(PlanOrPlanSum planOrPlanSum, IEnumerable<PlanningItem> planningItems) =>
            planningItems.FirstOrDefault(p => p.GetCourse().Id == planOrPlanSum.CourseId && p.Id == planOrPlanSum.Id);

        private PatientMatch CreatePatientMatch(PatientSummary ps) =>
            new PatientMatch
            {
                Id = ps.Id,
                LastName = ps.LastName,
                FirstName = ps.FirstName
            };

        private PlanOrPlanSum CreatePlanOrPlanSum(PlanningItem plan) =>
            new PlanOrPlanSum
            {
                Type = GetPlanType(plan),
                Id = plan.Id,
                CourseId = plan.GetCourse().Id
            };

        private PlanType GetPlanType(PlanningItem plan)
        {
            if (plan is PlanSetup)
                return PlanType.Plan;

            if (plan is PlanSum)
                return PlanType.PlanSum;

            throw new InvalidOperationException("Unknown plan type.");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using VMS.TPS.Common.Model.API;
using Application = VMS.TPS.Common.Model.API.Application;

namespace EsapiEssentials.PluginRunner
{
    internal class PluginRunner : IDisposable
    {
        private const string ApplicationName = "External Beam Planning";
        private const string VersionInfo = "13.6.32";
        private const int MaxSearchResults = 20;

        private readonly ScriptBase _script;
        private readonly ScriptBaseWithoutWindow _scriptWithoutWindow;

        private readonly Application _esapiApp;
        private readonly PatientSummarySearch _search;

        public PluginRunner(ScriptBase script, string userId, string password)
        {
            _script = script;

            _esapiApp = Application.CreateApplication(userId, password);
            _search = new PatientSummarySearch(_esapiApp.PatientSummaries, MaxSearchResults);
        }

        public PluginRunner(ScriptBaseWithoutWindow scriptWithoutWindow, string userId, string password)
        {
            _scriptWithoutWindow = scriptWithoutWindow;

            _esapiApp = Application.CreateApplication(userId, password);
            _search = new PatientSummarySearch(_esapiApp.PatientSummaries, MaxSearchResults);
        }

        public void Dispose()
        {
            _esapiApp.Dispose();
        }

        public PatientMatch[] FindPatientMatches(string searchText) =>
            _search.FindMatches(searchText).Select(CreatePatientMatch).ToArray();

        public PlanOrPlanSum[] GetPlansAndPlanSumsFor(string patientId)
        {
            var patient = _esapiApp.OpenPatientById(patientId);

            // Must call ToArray() before closing the patient
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

                if (_script != null)
                {
                    var window = new Window();
                    _script.Execute(context, window);
                    window.ShowDialog();
                }
                else if (_scriptWithoutWindow != null)
                {
                    _scriptWithoutWindow.Execute(context);
                }
            }
            catch (Exception e)
            {
                // Mimic Eclipse by showing a message box when an exception is thrown
                MessageBox.Show(e.Message, ApplicationName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            finally
            {
                _esapiApp.ClosePatient();
            }
        }

        private PluginScriptContext CreateScriptContext(Patient patient, IEnumerable<PlanOrPlanSum> plansAndPlanSumsInScope, PlanOrPlanSum activePlan)
        {
            var planningItems = patient?.GetPlanningItems().ToArray();
            var planningItemsInScope = FindPlanningItems(plansAndPlanSumsInScope, planningItems);
            var planSetup = FindPlanningItem(activePlan, planningItems) as PlanSetup;

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
                PlansInScope = planningItemsInScope?.Where(p => p is PlanSetup).Cast<PlanSetup>(),
                ExternalPlansInScope = planningItemsInScope?.Where(p => p is ExternalPlanSetup).Cast<ExternalPlanSetup>(),
                BrachyPlansInScope = planningItemsInScope?.Where(p => p is BrachyPlanSetup).Cast<BrachyPlanSetup>(),
                PlanSumsInScope = planningItemsInScope?.Where(p => p is PlanSum).Cast<PlanSum>(),
                ApplicationName = ApplicationName,
                VersionInfo = VersionInfo
            };
        }

        private PlanningItem[] FindPlanningItems(IEnumerable<PlanOrPlanSum> plansAndPlanSums, IEnumerable<PlanningItem> planningItems) =>
            plansAndPlanSums?.Select(p => FindPlanningItem(p, planningItems)).ToArray();

        private PlanningItem FindPlanningItem(PlanOrPlanSum planOrPlanSum, IEnumerable<PlanningItem> planningItems) =>
            planningItems?.FirstOrDefault(p => p.GetCourse().Id == planOrPlanSum?.CourseId && p.Id == planOrPlanSum?.Id);

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

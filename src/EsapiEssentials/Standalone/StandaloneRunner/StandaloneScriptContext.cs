using System.Collections.Generic;
using System.Linq;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials.Standalone
{
    public class StandaloneScriptContext
    {
        public User CurrentUser { get; set; }
        public Course Course { get; set; }
        public Image Image { get; set; }
        public StructureSet StructureSet { get; set; }
        public Patient Patient { get; set; }
        public PlanSetup PlanSetup { get; set; }
        public ExternalPlanSetup ExternalPlanSetup { get; set; }
        public BrachyPlanSetup BrachyPlanSetup { get; set; }
        public IEnumerable<PlanSetup> PlansInScope { get; set; }
        public IEnumerable<ExternalPlanSetup> ExternalPlansInScope { get; set; }
        public IEnumerable<BrachyPlanSetup> BrachyPlansInScope { get; set; }
        public IEnumerable<PlanSum> PlanSumsInScope { get; set; }
        public string ApplicationName { get; set; }
        public string VersionInfo { get; set; }

        public static StandaloneScriptContext From(string[] args, Application app)
        {
            var scriptContext = new StandaloneScriptContext();

            scriptContext.CurrentUser = app.CurrentUser;
            // Note: It's possible that the user from the command-line arguments
            // and the user that signed in to the standalone app are different,
            // but the best we can do is return the standalone app user

            var scArgs = ScriptContextArgs.From(args);

            scriptContext.ApplicationName = scArgs.ApplicationName;
            scriptContext.VersionInfo = scArgs.VersionInfo;

            // If there's no patient, there's nothing else to open
            if (scArgs.PatientId == null)
                return scriptContext;

            var patient = GetPatient(app, scArgs.PatientId);

            scriptContext.Patient = patient;
            scriptContext.Course = GetCourse(patient, scArgs.CourseId);
            scriptContext.Image = GetImage(patient, scArgs.SeriesUid, scArgs.ImageId);
            scriptContext.StructureSet = GetStructureSet(patient, scArgs.StructureSetId);
            scriptContext.PlanSetup = GetPlanSetup(patient, scArgs.PlanSetupId, scArgs.CourseId);
            scriptContext.ExternalPlanSetup = GetExternalPlanSetup(patient, scArgs.ExternalPlanSetupId, scArgs.CourseId);
            scriptContext.BrachyPlanSetup = GetBrachyPlanSetup(patient, scArgs.BrachyPlanSetupId, scArgs.CourseId);
            scriptContext.PlansInScope = GetPlansInScope(patient, scArgs.PlansInScopeUids);
            scriptContext.ExternalPlansInScope = GetExternalPlansInScope(patient, scArgs.ExternalPlansInScopeUids);
            scriptContext.BrachyPlansInScope = GetBrachyPlansInScope(patient, scArgs.BrachyPlansInScopeUids);
            scriptContext.PlanSumsInScope = GetPlanSumsInScope(patient, scArgs.PlanSumsInScopeCourseIds, scArgs.PlanSumsInScopeIds);

            return scriptContext;
        }

        private static Patient GetPatient(Application app, string patientId) =>
            app?.OpenPatientById(patientId);

        private static Course GetCourse(Patient patient, string courseId) =>
            patient?.Courses?.FirstOrDefault(x => x.Id == courseId);

        private static Image GetImage(Patient patient, string imageId, string seriesUid) =>
            GetSeries(patient, seriesUid)?.Images?.FirstOrDefault(x => x.Id == imageId);

        private static Series GetSeries(Patient patient, string seriesUid) =>
            patient?.Studies?.SelectMany(x => x.Series).FirstOrDefault(x => x.UID == seriesUid);

        private static StructureSet GetStructureSet(Patient patient, string structureSetId) =>
            patient?.StructureSets?.FirstOrDefault(x => x.Id == structureSetId);

        private static PlanSetup GetPlanSetup(Patient patient, string planSetupId, string courseId) =>
            GetCourse(patient, courseId)?.PlanSetups?.FirstOrDefault(x => x.Id == planSetupId);

        private static ExternalPlanSetup GetExternalPlanSetup(Patient patient, string planSetupId, string courseId) =>
            GetPlanSetup(patient, planSetupId, courseId) as ExternalPlanSetup;

        private static BrachyPlanSetup GetBrachyPlanSetup(Patient patient, string planSetupId, string courseId) =>
            GetPlanSetup(patient, planSetupId, courseId) as BrachyPlanSetup;

        private static IEnumerable<PlanSetup> GetPlansInScope(Patient patient, IEnumerable<string> plansInScopeUids) =>
            patient?.Courses?.SelectMany(x => x.PlanSetups).Where(x => plansInScopeUids.Contains(x.UID));

        private static IEnumerable<ExternalPlanSetup> GetExternalPlansInScope(Patient patient, IEnumerable<string> plansInScopeUids) =>
            GetPlansInScope(patient, plansInScopeUids)?.Where(x => x is ExternalPlanSetup).Cast<ExternalPlanSetup>();

        private static IEnumerable<BrachyPlanSetup> GetBrachyPlansInScope(Patient patient, IEnumerable<string> plansInScopeUids) =>
            GetPlansInScope(patient, plansInScopeUids)?.Where(x => x is BrachyPlanSetup).Cast<BrachyPlanSetup>();

        private static IEnumerable<PlanSum> GetPlanSumsInScope(Patient patient, IEnumerable<string> planSumsInScopeCourseIds, IEnumerable<string> planSumsInScopeIds) =>
            planSumsInScopeCourseIds?.Zip(planSumsInScopeIds ?? new string[0], (courseId, planSumId) => GetPlanSum(patient, courseId, planSumId));

        private static PlanSum GetPlanSum(Patient patient, string courseId, string planSumId) =>
            patient?.Courses?.FirstOrDefault(x => x.Id == courseId)?.PlanSums?.FirstOrDefault(x => x.Id == planSumId);
    }
}
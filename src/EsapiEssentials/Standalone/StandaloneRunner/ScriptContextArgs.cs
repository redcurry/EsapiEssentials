using System.Collections.Generic;
using System.Linq;
using CommandLine;
using EsapiEssentials.Plugin;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials.Standalone
{
    internal class ScriptContextArgs
    {
        [Option("user-id", Required = true)]
        public string CurrentUserId { get; set; }

        [Option("course-id")]
        public string CourseId { get; set; }

        // Used to find the exact image
        [Option("series-uid")]
        public string SeriesUid { get; set; }

        [Option("image-id")]
        public string ImageId { get; set; }

        [Option("structure-set-id")]
        public string StructureSetId { get; set; }

        [Option("patient-id")]
        public string PatientId { get; set; }

        [Option("plan-id")]
        public string PlanSetupId { get; set; }

        [Option("external-plan-id")]
        public string ExternalPlanSetupId { get; set; }

        [Option("brachy-plan-id")]
        public string BrachyPlanSetupId { get; set; }

        [Option("plans-in-scope-uids")]
        public IEnumerable<string> PlansInScopeUids { get; set; }

        [Option("external-plans-in-scope-uids")]
        public IEnumerable<string> ExternalPlansInScopeUids { get; set; }

        [Option("brachy-plans-in-scope-uids")]
        public IEnumerable<string> BrachyPlansInScopeUids { get; set; }

        // Used to find the exact plan sums
        [Option("plan-sums-in-scope-course-ids")]
        public IEnumerable<string> PlanSumsInScopeCourseIds { get; set; }

        [Option("plan-sums-in-scope-ids")]
        public IEnumerable<string> PlanSumsInScopeIds { get; set; }

        [Option("app-name")]
        public string ApplicationName { get; set; }

        [Option("version")]
        public string VersionInfo { get; set; }

        public static ScriptContextArgs From(ScriptContext context)
        {
            return new ScriptContextArgs
            {
                CurrentUserId = context.CurrentUser?.Id,
                CourseId = context.Course?.Id,
                SeriesUid = context.Image?.Series?.UID,
                ImageId = context.Image?.Id,
                StructureSetId = context.StructureSet?.Id,
                PatientId = context.Patient?.Id,
                PlanSetupId = context.PlanSetup?.Id,
                ExternalPlanSetupId = context.ExternalPlanSetup?.Id,
                BrachyPlanSetupId = context.BrachyPlanSetup?.Id,
                PlansInScopeUids = context.PlansInScope?.Select(x => x.UID),
                ExternalPlansInScopeUids = context.ExternalPlansInScope?.Select(x => x.UID),
                BrachyPlansInScopeUids = context.BrachyPlansInScope?.Select(x => x.UID),
                PlanSumsInScopeCourseIds = context.PlanSumsInScope?.Select(x => x.Course?.Id),
                PlanSumsInScopeIds = context.PlanSumsInScope?.Select(x => x.Id),
                ApplicationName = context.ApplicationName,
                VersionInfo = context.VersionInfo
            };
        }

        public static ScriptContextArgs From(string[] args)
        {
            ScriptContextArgs scriptContextArgs = null;
            Parser.Default.ParseArguments<ScriptContextArgs>(args)
                .WithParsed(parsedArgs => scriptContextArgs = parsedArgs);
            return scriptContextArgs;
        }

        public string ToArgs()
        {
            return Parser.Default.FormatCommandLine(this);
        }

        public PluginScriptContext ToScriptContext(Application app)
        {
            var scriptContext = new PluginScriptContext();

            scriptContext.CurrentUser = app.CurrentUser;
            // Note: It's possible that the user from the command-line arguments
            // and the user that signed in to the standalone app are different,
            // but the best we can do is return the standalone app user

            scriptContext.ApplicationName = ApplicationName;
            scriptContext.VersionInfo = VersionInfo;

            // If there's no patient, there's nothing else to open
            if (PatientId == null)
                return scriptContext;

            var patient = GetPatient(app);

            scriptContext.Patient = patient;
            scriptContext.Course = GetCourse(patient);
            scriptContext.Image = GetImage(patient);
            scriptContext.StructureSet = GetStructureSet(patient);
            scriptContext.PlanSetup = GetPlanSetup(patient);
            scriptContext.ExternalPlanSetup = GetExternalPlanSetup(patient);
            scriptContext.BrachyPlanSetup = GetBrachyPlanSetup(patient);
            scriptContext.PlansInScope = GetPlansInScope(patient);
            scriptContext.ExternalPlansInScope = GetExternalPlansInScope(patient);
            scriptContext.BrachyPlansInScope = GetBrachyPlansInScope(patient);
            scriptContext.PlanSumsInScope = GetPlanSumsInScope(patient);

            return scriptContext;
        }

        private Patient GetPatient(Application app) =>
            app.OpenPatientById(PatientId);

        private StructureSet GetStructureSet(Patient patient) =>
            patient?.StructureSets?.FirstOrDefault(x => x.Id == StructureSetId);

        private Series GetSeries(Patient patient) =>
            patient?.Studies?.SelectMany(x => x.Series).FirstOrDefault(x => x.UID == SeriesUid);

        private Image GetImage(Patient patient) =>
            GetSeries(patient)?.Images?.FirstOrDefault(x => x.Id == ImageId);

        private Course GetCourse(Patient patient) =>
            patient?.Courses?.FirstOrDefault(x => x.Id == CourseId);

        private PlanSetup GetPlanSetup(Patient patient) =>
            GetCourse(patient)?.PlanSetups?.FirstOrDefault(x => x.Id == PlanSetupId);

        private ExternalPlanSetup GetExternalPlanSetup(Patient patient) =>
            GetPlanSetup(patient) as ExternalPlanSetup;

        private BrachyPlanSetup GetBrachyPlanSetup(Patient patient) =>
            GetPlanSetup(patient) as BrachyPlanSetup;

        private IEnumerable<PlanSetup> GetPlansInScope(Patient patient) =>
            patient?.Courses?.SelectMany(x => x.PlanSetups).Where(x => PlansInScopeUids.Contains(x.UID));

        private IEnumerable<ExternalPlanSetup> GetExternalPlansInScope(Patient patient) =>
            GetPlansInScope(patient).Where(x => x is ExternalPlanSetup).Cast<ExternalPlanSetup>();

        private IEnumerable<BrachyPlanSetup> GetBrachyPlansInScope(Patient patient) =>
            GetPlansInScope(patient).Where(x => x is BrachyPlanSetup).Cast<BrachyPlanSetup>();

        private IEnumerable<PlanSum> GetPlanSumsInScope(Patient patient) =>
            PlanSumsInScopeCourseIds.Zip(PlanSumsInScopeIds, (courseId, planSumId) => GetPlanSum(patient, courseId, planSumId));

        private PlanSum GetPlanSum(Patient patient, string courseId, string planSumId) =>
            patient?.Courses?.FirstOrDefault(x => x.Id == courseId)?.PlanSums?.FirstOrDefault(x => x.Id == planSumId);
    }
}
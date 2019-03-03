using System.Collections.Generic;
using System.Linq;
using CommandLine;
using VMS.TPS.Common.Model.API;

namespace EsapiEssentials
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
    }
}
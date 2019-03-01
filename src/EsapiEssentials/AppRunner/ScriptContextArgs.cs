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

        [Option("course-id")]
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

        [Option("plans-in-scope-ids")]
        public IEnumerable<string> PlansInScopeIds { get; set; }

        [Option("external-plans-in-scope-ids")]
        public IEnumerable<string> ExternalPlansInScopeIds { get; set; }

        [Option("brachy-plans-in-scope-ids")]
        public IEnumerable<string> BrachyPlansInScopeIds { get; set; }

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
                ImageId = context.Image?.Id,
                StructureSetId = context.StructureSet?.Id,
                PatientId = context.Patient?.Id,
                PlanSetupId = context.PlanSetup?.Id,
                ExternalPlanSetupId = context.ExternalPlanSetup?.Id,
                BrachyPlanSetupId = context.BrachyPlanSetup?.Id,
                PlansInScopeIds = context.PlansInScope?.Select(x => x.Id),
                ExternalPlansInScopeIds = context.ExternalPlansInScope?.Select(x => x.Id),
                BrachyPlansInScopeIds = context.BrachyPlansInScope?.Select(x => x.Id),
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
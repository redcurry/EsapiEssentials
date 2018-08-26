using VMS.TPS.Common.Model.API;

namespace EsapiEssentials.Extensions
{
    public static class StructureExtensions
    {
        public static bool IsTarget(this Structure structure) =>
            structure.DicomType == "GTV" || structure.DicomType == "CTV" || structure.DicomType == "PTV";

        public static bool IsOrgan(this Structure structure) =>
            structure.DicomType == "ORGAN";
    }
}
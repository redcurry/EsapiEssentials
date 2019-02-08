using VMS.TPS.Common.Model.API;

namespace EsapiEssentials
{
    /// <summary>
    /// Extension methods for the Structure class.
    /// </summary>
    public static class StructureExtensions
    {
        /// <summary>
        /// Determines whether the given Structure is a target (DICOM type: GTV, CTV, or PTV).
        /// </summary>
        /// <param name="structure">The Structure to determine whether it is a target.</param>
        /// <returns>true, if the given Structure is a target; false, otherwise.</returns>
        public static bool IsTarget(this Structure structure) =>
            structure.DicomType == "GTV" || structure.DicomType == "CTV" || structure.DicomType == "PTV";

        /// <summary>
        /// Determines whether the given Structure is an organ (DICOM type: ORGAN).
        /// </summary>
        /// <param name="structure">The Structure to determine whether it is an organ.</param>
        /// <returns>true, if the given Structure is an organ; false, otherwise.</returns>
        public static bool IsOrgan(this Structure structure) =>
            structure.DicomType == "ORGAN";
    }
}
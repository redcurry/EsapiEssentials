using System;
using EsapiEssentials.Core;
using VMS.TPS.Common.Model.Types;

namespace EsapiEssentials
{
    /// <summary>
    /// Extension methods for the DoseValue struct.
    /// </summary>
    public static class DoseValueExtensions
    {
        /// <summary>
        /// Converts the given DoseValue instance to a Dose object.
        /// </summary>
        /// <param name="doseValue"></param>
        /// <returns>The Dose object that represents the given DoseValue.</returns>
        public static Dose ToDose(this DoseValue doseValue)
        {
            switch (doseValue.Unit)
            {
                case DoseValue.DoseUnit.Gy:  return new Dose(doseValue.Dose, DoseUnit.Gy);
                case DoseValue.DoseUnit.cGy: return new Dose(doseValue.Dose, DoseUnit.CGy);
                default:
                    throw new ArgumentException($"Dose unit {doseValue.Unit} is not supported.");
            }
        }
    }
}
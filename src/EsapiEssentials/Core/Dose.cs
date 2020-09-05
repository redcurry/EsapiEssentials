using System;

namespace EsapiEssentials.Core
{
    public class Dose : IEquatable<Dose>
    {
        private const double CGyPerGy = 100;

        // Default tolerance for equality and comparisons
        // (may be changed using the Tolerance property)
        private static Dose _tolerance = new Dose(0.0001, DoseUnit.Gy);

        // Default number of decimals to display for ToString()
        // (may be changed using the DisplayDecimals property)
        private static int _displayDecimals = 2;

        /// <summary>
        /// Creates an instance of Dose.
        /// </summary>
        /// <param name="dose">The dose value.</param>
        /// <param name="unit">The dose unit.</param>
        public Dose(double dose, DoseUnit unit)
        {
            switch (unit)
            {
                case DoseUnit.Gy:  Gy = dose;          break;
                case DoseUnit.CGy: Gy = CGyToGy(dose); break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(unit), unit, null);
            }
        }

        /// <summary>
        /// Get or sets the dose unit to display for ToString().
        /// </summary>
        public static DoseUnit DisplayUnit { get; set; } = DoseUnit.Gy;

        /// <summary>
        /// Gets or sets the number of decimals to display for ToString().
        /// </summary>
        public static int DisplayDecimals
        {
            get => _displayDecimals;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Display decimals must be greater than or equal to 0.");

                _displayDecimals = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum difference in dose between two doses to be considered equal.
        /// </summary>
        public static Dose Tolerance
        {
            get => _tolerance;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                if (value.IsUndefined)
                    throw new ArgumentException("Tolerance cannot be set to Undefined.");

                _tolerance = value;
            }
        }

        /// <summary>
        /// Gets a dose of zero.
        /// </summary>
        public static Dose Zero { get; } =
            new Dose(0, DoseUnit.Gy);

        /// <summary>
        /// Gets an Undefined dose.
        /// </summary>
        public static Dose Undefined { get; } =
            new Dose(double.NaN, DoseUnit.Gy);

        /// <summary>
        /// Gets the dose in Gy.
        /// </summary>
        public double Gy { get; }

        /// <summary>
        /// Gets the dose in cGy.
        /// </summary>
        public double CGy => GyToCGy(Gy);

        /// <summary>
        /// Determines whether the dose is Undefined.
        /// </summary>
        public bool IsUndefined => double.IsNaN(Gy);

        /// <summary>
        /// Converts the string representation of a dose (with unit) to an instance of Dose.
        /// </summary>
        /// <param name="doseStr">The string to convert to a Dose.</param>
        /// <returns>The parsed Dose instance.</returns>
        public static Dose Parse(string doseStr)
        {
            var doseUnitIndex = doseStr.IndexOf("cGy", StringComparison.CurrentCultureIgnoreCase);

            if (doseUnitIndex == -1)
                doseUnitIndex = doseStr.IndexOf("Gy", StringComparison.CurrentCultureIgnoreCase);

            var doseValueStr = doseStr.Substring(0, doseUnitIndex);
            var doseUnitStr = doseStr.Substring(doseUnitIndex, doseStr.Length - doseUnitIndex);

            var doseValue = double.Parse(doseValueStr);
            var doseUnit = ParseUnit(doseUnitStr);

            return new Dose(doseValue, doseUnit);
        }

        /// <summary>
        /// Converts the string representation of a dose (with unit) to an instance of Dose.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="doseStr">The string to convert to a Dose.</param>
        /// <param name="dose">The parsed dose, if the conversion succeeded; otherwise, null.</param>
        /// <returns>true, if the conversion succeeded; otherwise, false.</returns>
        public static bool TryParse(string doseStr, out Dose dose)
        {
            try
            {
                dose = Parse(doseStr);
                return true;
            }
            catch
            {
                dose = null;
                return false;
            }
        }

        /// <summary>
        /// Calculates the sum of the given doses.
        /// </summary>
        /// <param name="d1">The first dose to add.</param>
        /// <param name="d2">The second dose to add.</param>
        /// <returns>The sum of the doses.</returns>
        public static Dose operator +(Dose d1, Dose d2) =>
            new Dose(d1.Gy + d2.Gy, DoseUnit.Gy);

        /// <summary>
        /// Calculates the difference between the given doses.
        /// </summary>
        /// <param name="d1">The first dose.</param>
        /// <param name="d2">The second dose.</param>
        /// <returns>The difference between the first and second doses.</returns>
        public static Dose operator -(Dose d1, Dose d2) =>
            new Dose(d1.Gy - d2.Gy, DoseUnit.Gy);

        /// <summary>
        /// Calculates the negative of the given dose.
        /// </summary>
        /// <param name="d">The dose.</param>
        /// <returns>The negative of the dose.</returns>
        public static Dose operator -(Dose d) =>
            new Dose(-d.Gy, DoseUnit.Gy);

        /// <summary>
        /// Calculates the product between the given dose and number.
        /// </summary>
        /// <param name="d">The dose.</param>
        /// <param name="x">The number.</param>
        /// <returns>The product between the dose and number.</returns>
        public static Dose operator *(Dose d, double x) =>
            new Dose(d.Gy * x, DoseUnit.Gy);

        /// <summary>
        /// Calculates the product between the given number and dose.
        /// </summary>
        /// <param name="x">The number.</param>
        /// <param name="d">The dose.</param>
        /// <returns>The product between the number and dose.</returns>
        public static Dose operator *(double x, Dose d) =>
            d * x;

        /// <summary>
        /// Calculates the ratio between the given doses.
        /// </summary>
        /// <param name="d1">The dose of the numerator.</param>
        /// <param name="d2">The dose of the denominator.</param>
        /// <returns>The ration between the doses.</returns>
        public static Dose operator /(Dose d1, Dose d2) =>
            new Dose(d1.Gy / d2.Gy, DoseUnit.Gy);

        /// <summary>
        /// Calculates the division between the given dose and number.
        /// </summary>
        /// <param name="d">The dose (numerator).</param>
        /// <param name="x">The number (denominator).</param>
        /// <returns>The division between the dose and number.</returns>
        public static Dose operator /(Dose d, double x) =>
            new Dose(d.Gy / x, DoseUnit.Gy);

        /// <summary>
        /// Determines whether the given doses are equal.
        /// </summary>
        /// <param name="d1">The first dose to compare.</param>
        /// <param name="d2">The second dose to compare.</param>
        /// <returns>true if the doses are equal within Tolerance; otherwise, false.</returns>
        public static bool operator ==(Dose d1, Dose d2)
        {
            if (ReferenceEquals(d1, d2)) return true;  // handles (Dose)null == (Dose)null
            return d1?.Equals(d2) ?? false;
        }

        /// <summary>
        /// Determines whether the given doses are not equal.
        /// </summary>
        /// <param name="d1">The first dose to compare.</param>
        /// <param name="d2">The second dose to compare.</param>
        /// <returns>true if the doses are not equal within Tolerance; otherwise, false.</returns>
        public static bool operator !=(Dose d1, Dose d2) =>
            !(d1 == d2);

        /// <summary>
        /// Determines whether the first given dose is less than the second given dose.
        /// </summary>
        /// <param name="d1">The first dose to compare.</param>
        /// <param name="d2">The second dose to compare.</param>
        /// <returns>
        /// true if the first dose is less than the second dose
        /// within Tolerance; otherwise, false.
        /// </returns>
        public static bool operator <(Dose d1, Dose d2) =>
            d1.Gy < (d2.Gy - Tolerance.Gy);

        /// <summary>
        /// Determines whether the first given dose is greater than the second given dose.
        /// </summary>
        /// <param name="d1">The first dose to compare.</param>
        /// <param name="d2">The second dose to compare.</param>
        /// <returns>
        /// true if the first dose is greater than the second dose
        /// within Tolerance; otherwise, false.
        /// </returns>
        public static bool operator >(Dose d1, Dose d2) =>
            d1.Gy > (d2.Gy + Tolerance.Gy);

        /// <summary>
        /// Determines whether the first given dose is less than or equal to the second given dose.
        /// </summary>
        /// <param name="d1">The first dose to compare.</param>
        /// <param name="d2">The second dose to compare.</param>
        /// <returns>
        /// true if the first dose is less than or equal to the second dose
        /// within Tolerance; otherwise, false.
        /// </returns>
        public static bool operator <=(Dose d1, Dose d2) =>
            d1.Gy < (d2.Gy + Tolerance.Gy);

        /// <summary>
        /// Determines whether the first given dose is greater than or equal to the second given dose.
        /// </summary>
        /// <param name="d1">The first dose to compare.</param>
        /// <param name="d2">The second dose to compare.</param>
        /// <returns>
        /// true if the first dose is greater than or equal to the second dose
        /// within Tolerance; otherwise, false.
        /// </returns>
        public static bool operator >=(Dose d1, Dose d2) =>
            d1.Gy > (d2.Gy - Tolerance.Gy);

        /// <summary>
        /// Converts this dose instance to its string representation.
        /// </summary>
        /// <returns>The string representation of this dose instance.</returns>
        public override string ToString()
        {
            switch (DisplayUnit)
            {
                case DoseUnit.Gy:  return  Gy.ToString($"f{DisplayDecimals}") + " Gy";
                case DoseUnit.CGy: return CGy.ToString($"f{DisplayDecimals}") + " cGy";
                default:
                    throw new InvalidOperationException("Unknown display unit.");
            }
        }

        /// <summary>
        /// Determines whether this instance and the given dose are equal.
        /// </summary>
        /// <param name="other">The dose to compare to this instance.</param>
        /// <returns>true of the given dose and this instance are equal; otherwise, false.</returns>
        public bool Equals(Dose other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Math.Abs(Gy - other.Gy) < Tolerance.Gy;
        }

        /// <summary>
        /// Determines whether this instance and the given object are equal.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns>true of the given object and this instance are equal; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return (obj is Dose dose) && Equals(dose);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return Gy.GetHashCode();
        }

        private static double GyToCGy(double d) => d * CGyPerGy;
        private static double CGyToGy(double d) => d / CGyPerGy;

        private static DoseUnit ParseUnit(string doseUnitStr)
        {
            switch (doseUnitStr.Trim().ToLower())
            {
                case "gy" : return DoseUnit.Gy;
                case "cgy": return DoseUnit.CGy;

                default:
                    throw new ArgumentException($"Unknown dose unit \"{doseUnitStr}\".");
            }
        }
    }
}

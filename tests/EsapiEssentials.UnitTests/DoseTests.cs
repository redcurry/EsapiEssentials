using System;
using EsapiEssentials.Core;
using NUnit.Framework;

namespace EsapiEssentials.UnitTests
{
    [TestFixture]
    public class DoseTests
    {
        [Test]
        public void Gy_WhenCreatedWithGy()
        {
            var dose = new Dose(10, DoseUnit.Gy);
            Assert.That(dose.Gy, Is.EqualTo(10));
        }

        [Test]
        public void CGy_WhenCreatedWithGy()
        {
            var dose = new Dose(10, DoseUnit.Gy);
            Assert.That(dose.CGy, Is.EqualTo(1000));
        }

        [Test]
        public void Gy_WhenCreatedWithCGy()
        {
            var dose = new Dose(10, DoseUnit.CGy);
            Assert.That(dose.Gy, Is.EqualTo(0.1));
        }

        [Test]
        public void CGy_WhenCreatedWithCGy()
        {
            var dose = new Dose(10, DoseUnit.CGy);
            Assert.That(dose.CGy, Is.EqualTo(10));
        }

        [Test]
        public void Add_ReturnsSumOfDoses()
        {
            var dose1 = new Dose(10,  DoseUnit.Gy);
            var dose2 = new Dose(100, DoseUnit.CGy);

            var result = dose1 + dose2;

            Assert.That(result.Gy, Is.EqualTo(11));
        }

        [Test]
        public void Subtract_ReturnsSubtractionOfDoses()
        {
            var dose1 = new Dose(100,  DoseUnit.Gy);
            var dose2 = new Dose(1000, DoseUnit.CGy);

            var result = dose1 - dose2;

            Assert.That(result.Gy, Is.EqualTo(90));
        }

        [Test]
        public void Negative_ReturnsNegativeDose()
        {
            var dose = new Dose(10, DoseUnit.Gy);

            var result = -dose;

            Assert.That(result.Gy, Is.EqualTo(-10));
        }

        [Test]
        public void MultiplyScalar_ReturnsDoseTimesScalarValue()
        {
            var dose = new Dose(10, DoseUnit.Gy);

            var result1 = dose * 2;
            var result2 = 3 * dose;

            Assert.That(result1.Gy, Is.EqualTo(20));
            Assert.That(result2.Gy, Is.EqualTo(30));
        }

        [Test]
        public void Divide_ReturnsRatioOfDoses()
        {
            var dose1 = new Dose(100,  DoseUnit.Gy);
            var dose2 = new Dose(1000, DoseUnit.CGy);

            var result = dose1 / dose2;

            Assert.That(result.Gy, Is.EqualTo(10));
        }

        [Test]
        public void DivideByScalar_ReturnsDoseDividedByScalar()
        {
            var dose = new Dose(10, DoseUnit.Gy);

            var result = dose / 10;

            Assert.That(result.Gy, Is.EqualTo(1));
        }

        [Test]
        public void Equals_ReturnsTrue_WhenDosesAreEqualButDifferentUnits()
        {
            var dose1 = new Dose(1000, DoseUnit.CGy);
            var dose2 = new Dose(10,   DoseUnit.Gy);

            var result = dose1 == dose2;

            Assert.That(result, Is.True);
            Assert.That(dose1.Equals(dose2));
        }

        [Test]
        public void Equals_ReturnsFalse_WhenOneDoseIsNull()
        {
            var dose = new Dose(10, DoseUnit.Gy);

            var result1 = dose == null;
            var result2 = null == dose;

            Assert.That(result1, Is.False);
            Assert.That(result2, Is.False);
        }

        [Test]
        public void Equals_ReturnsTrue_IfBothDosesAreNull()
        {
            Dose dose1 = null;

            var result = dose1 == null;

            Assert.That(result, Is.True);
        }

        [TestCase(10, 10.1, "==", true)]
        [TestCase(10, 20  , "==", false)]
        [TestCase(10, 20  , "<",  true)]
        [TestCase(10, 10.1, "<",  false)]
        [TestCase(10,  5  , ">",  true)]
        [TestCase(10,  9.9, ">",  false)]
        [TestCase(10,  9.9, "<=", true)]
        [TestCase(10, 20  , "<=", true)]
        [TestCase(10,  5  , "<=", false)]
        public void TestComparisons(double doseGy1, double doseGy2, string comparison, bool expectedResult)
        {
            // Set to a large tolerance to make the tests more obvious
            Dose.Tolerance = new Dose(0.5, DoseUnit.Gy);

            var dose1 = new Dose(doseGy1, DoseUnit.Gy);
            var dose2 = new Dose(doseGy2, DoseUnit.Gy);

            bool result;
            switch (comparison)
            {
                case "==": result = dose1 == dose2; break;
                case "!=": result = dose1 != dose2; break;
                case "<" : result = dose1 <  dose2; break;
                case ">" : result = dose1 >  dose2; break;
                case "<=": result = dose1 <= dose2; break;
                case ">=": result = dose1 >= dose2; break;
                default: throw new ArgumentException("Unknown comparison");
            }

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Zero_CreateZeroDose()
        {
            var dose = Dose.Zero;

            Assert.That(dose.Gy, Is.EqualTo(0));
        }

        [Test]
        public void Undefined_IsUndefinedIsTrue()
        {
            var dose = Dose.Undefined;

            Assert.That(dose.IsUndefined);
        }

        [Test]
        public void SetTolerance_Throws_IfDoseIsNull()
        {
            Assert.That(() => Dose.Tolerance = null, Throws.ArgumentNullException);
        }

        [Test]
        public void SetTolerance_Throws_IfDoseIsUndefined()
        {
            Assert.That(() => Dose.Tolerance = Dose.Undefined, Throws.ArgumentException);
        }

        [TestCase("10 Gy",     10,    DoseUnit.Gy)]
        [TestCase("10 gy",     10,    DoseUnit.Gy)]
        [TestCase("10 cGy",    10,    DoseUnit.CGy)]
        [TestCase("10Gy",      10,    DoseUnit.Gy)]
        [TestCase("10cGy",     10,    DoseUnit.CGy)]
        [TestCase("10cgy",     10,    DoseUnit.CGy)]
        [TestCase("10.2 Gy",   10.2,  DoseUnit.Gy)]
        [TestCase(" 10.2 Gy ", 10.2,  DoseUnit.Gy)]
        [TestCase("100.8 cGy", 100.8, DoseUnit.CGy)]
        [TestCase("-10.8 cGy", -10.8, DoseUnit.CGy)]
        [TestCase("10e4 cGy",  10e4,  DoseUnit.CGy)]
        public void Parse_ValidDose_ReturnsParsedDose(string doseStr, double expectedDoseValue, DoseUnit expectedDoseUnit)
        {
            var dose = Dose.Parse(doseStr);
            Assert.That(dose, Is.EqualTo(new Dose(expectedDoseValue, expectedDoseUnit)));
        }

        [TestCase("10a")]
        [TestCase("10Gya")]
        [TestCase("Gy")]
        [TestCase("Gy10")]
        [TestCase("10_cGy")]
        public void Parse_InvalidDose_Throws(string doseStr)
        {
            Assert.That(() => Dose.Parse(doseStr), Throws.Exception);
        }

        [TestCase("10 Gy",     10,    DoseUnit.Gy)]
        [TestCase("10 cGy",    10,    DoseUnit.CGy)]
        [TestCase("10Gy",      10,    DoseUnit.Gy)]
        [TestCase("10cGy",     10,    DoseUnit.CGy)]
        [TestCase("10.2 Gy",   10.2,  DoseUnit.Gy)]
        [TestCase("100.8 cGy", 100.8, DoseUnit.CGy)]
        [TestCase("-10.8 cGy", -10.8, DoseUnit.CGy)]
        [TestCase("10e4 cGy",  10e4,  DoseUnit.CGy)]
        public void TryParse_Validdose_ReturnsTrueAndParsedDose(string doseStr, double expectedDoseValue, DoseUnit expectedDoseUnit)
        {
            var parsed = Dose.TryParse(doseStr, out var dose);
            Assert.That(parsed, Is.True);
            Assert.That(dose, Is.EqualTo(new Dose(expectedDoseValue, expectedDoseUnit)));
        }

        [TestCase("10a")]
        [TestCase("10Gya")]
        [TestCase("Gy")]
        [TestCase("Gy10")]
        [TestCase("10_cGy")]
        public void TryParse_InvalidDose_ReturnsFalse(string doseStr)
        {
            var parsed = Dose.TryParse(doseStr, out _);
            Assert.That(parsed, Is.False);
        }

        [TestCase("10 Gy",   DoseUnit.Gy,  0, "10 Gy")]
        [TestCase("10 Gy",   DoseUnit.Gy,  2, "10.00 Gy")]
        [TestCase("100 cGy", DoseUnit.Gy,  2, "1.00 Gy")]
        [TestCase("10 Gy",   DoseUnit.CGy, 0, "1000 cGy")]
        [TestCase("10 Gy",   DoseUnit.CGy, 2, "1000.00 cGy")]
        [TestCase("100 cGy", DoseUnit.CGy, 2, "100.00 cGy")]
        public void ToString_GyUnit_ReturnsString(string doseStr, DoseUnit displayUnit, int displayDecimals, string expectedDoseStr)
        {
            Dose.DisplayUnit = displayUnit;
            Dose.DisplayDecimals = displayDecimals;

            var dose = Dose.Parse(doseStr);

            var result = dose.ToString();

            Assert.That(result, Is.EqualTo(expectedDoseStr));
        }
    }
}

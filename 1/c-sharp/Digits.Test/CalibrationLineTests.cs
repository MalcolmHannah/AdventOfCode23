using NUnit.Framework;
using System;

namespace Digits.Test
{
    [TestFixture]
    public class CalibrationLineTests
    {
        /// <summary>
        /// Test cases from the example file given by Advent Of Code.
        /// </summary>
        /// <param name="line">Calibration file line.</param>
        /// <param name="result">Expected result.</param>
        [TestCase("two1nine", 29)]
        [TestCase("eightwothree", 83)]
        [TestCase("abcone2threexyz", 13)]
        [TestCase("xtwone3four", 24)]
        [TestCase("4nineeightseven2", 42)]
        [TestCase("zoneight234", 14)]
        [TestCase("7pqrstsixteen", 76)]
        public void AdventOfCode_Examples(string line, int result)
        {
            var cal = new CalibrationLine(line);
            Assert.That(cal.Result, Is.EqualTo(result));
        }

        /// <summary>
        /// Corner cases.  Note zero is not allowed.
        /// </summary>
        /// <param name="line">Calibration file line.</param>
        /// <param name="result">Expected result.</param>
        [TestCase("", 0)]
        [TestCase(" ", 0)]
        [TestCase("nodigits", 0)]
        [TestCase("*&^%$£!", 0)]
        [TestCase("0zero0", 0)]
        [TestCase("zero00zero", 0)]
        public void CornerCases(string line, int result)
        {
            var cal = new CalibrationLine(line);
            Assert.That(cal.Result, Is.EqualTo(result));
        }
    }
}

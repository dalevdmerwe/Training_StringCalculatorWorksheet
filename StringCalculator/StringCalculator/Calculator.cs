using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using StringCalculator;

namespace StringCalculator
{
    public class Calculator
    {
        public int Add(string inputString)
        {
            int sumamount = 0;

            if (inputString == String.Empty)
                inputString = "0";

            var numbers = inputString.Split(',');

            foreach (var c in numbers)
            {
                sumamount += Convert.ToInt32(c);
            }

            return sumamount;
        }
    }

    [TestFixture]
    public class CalculatorTests
    {
        [Test]
        public void emptyStringReturnsZero()
        {
            //arrange
            string emptystring = "";

            //act
            var sut = new Calculator();

            //assert
            Assert.That(sut.Add(emptystring), Is.EqualTo(0));
        }

        [TestCase("", 0)]
        [TestCase("0", 0)]
        [TestCase("1",1)]
        [TestCase("5", 5)]
        [TestCase("7", 7)]
        public void stringwithNumberReturnsthatNumber(string inputnumber, int expectedresult)
        {
            //string numberstring = "1";
            var sut = new Calculator().Add(inputnumber);
            Assert.That(sut,Is.EqualTo(expectedresult));
        }

        [TestCase("1,2",3)]
        [TestCase("1,2,5", 8)]
        [TestCase("0,1,2", 3)]
        [TestCase("0,0,0", 0)]
        public void TwoNumbersReturnsSum(string inputstring, int result)
        {
            //arrange
            //string twonumbers = "1,2";
            //act
            var sut = new Calculator().Add(inputstring);
            //assert
            Assert.That(sut, Is.EqualTo(result));

        }
    }
}

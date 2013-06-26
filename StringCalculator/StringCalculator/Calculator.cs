using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using StringCalculator;

namespace StringCalculator
{
    class program 
    {
        static void Main(string[] args)
        {
//            var myCalculator = new Calculator();
//            myCalculator.Add("//;\n1;3");
        }
    }

    public class Calculator
    {
        public int Add(string inputString)
        {
            char [] delimeters = new char[5];
            if (inputString.Contains("//"))
            {
                delimeters[0] = inputString.Substring(2, 1)[0];
                inputString = inputString.Substring(3, inputString.Length - 3);
            }

            if (inputString.Contains("-"))
            {
                throw new Exception("Negatives Not allowed");
            }

            delimeters[1] = '\n';
            delimeters[2] = ',';

            var sumamount = 0;
            var numbers = inputString.Split(delimeters);

            
            foreach (var number in numbers)
            {
                if (number != String.Empty)
                sumamount += Convert.ToInt32(number);
            }

            return sumamount;
        }
    }

    [TestFixture]
    public class CalculatorTests
    {
        [Test]
        public void EmptyStringReturnsZero()
        {
            string emptystring = "";
            var sut = new Calculator();
            Assert.That(sut.Add(emptystring), Is.EqualTo(0));
        }

        [TestCase("", 0)]
        [TestCase("0", 0)]
        [TestCase("1",1)]
        [TestCase("5", 5)]
        [TestCase("7", 7)]
        public void StringwithNumberReturnsthatNumber(string inputnumber, int expectedresult)
        {
            var sut = new Calculator().Add(inputnumber);
            Assert.That(sut,Is.EqualTo(expectedresult));
        }

        [TestCase("1,2",3)]
        [TestCase("1,2,5", 8)]
        [TestCase("0,1,2", 3)]
        [TestCase("0,0,0", 0)]
        public void TwoOrMoreNumbersReturnsSum(string inputstring, int result)
        {
            var sut = new Calculator().Add(inputstring);
            Assert.That(sut, Is.EqualTo(result));

        }

        [TestCase("1\n2",3)]
        [TestCase("1\n2,3", 6)]
        public void NewLineCharacterAsDelimiter(string inputString, int result)
        {
            var sut = new Calculator().Add(inputString);
            Assert.That(sut,Is.EqualTo(result));
        }

        [TestCase("//;\n1;3",4)]
        [TestCase("//|\n1|3|5", 9)]
        [TestCase("// \n1 3 6", 10)]
        public void SpecifyDelimeter(string input, int result)
        {
            var sut = new Calculator().Add(input);
            Assert.That(sut,Is.EqualTo(result));
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void AddingNegativesWillThrowException()
        {
            string negativeinput = "//&\n-5&7";
            var sut = new Calculator().Add(negativeinput);
            //Assert.That(sut, Is.EqualTo("Negatives Not allowed"));
            //Assert.Throws(typeof(ArgumentException), "Negatives Not allowed");
           
//            TestDelegate add = () => Calculator.Add("-1");
//            Assert.Throws<Exception>(add);
        }


        [Test]
        [ExpectedException("Negatives Not allowed")]
        public void NegativesThrowsException()
        {
            string negativeinput = "//&\n-5&7";
            var sut = new Calculator().Add(negativeinput);
        }


        
    }
}

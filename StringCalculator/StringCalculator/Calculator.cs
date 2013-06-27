using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
using StringCalculator;

namespace StringCalculator
{
    class program 
    {
        static void Main(string[] args)
        {
//          var myCalculator = new Calculator();
//          myCalculator.Add("//;\n1;3");

//          var pattern = @"\[(.*?)\]";
//            var pattern = @"\[(.*?)\]";
//            var query = "H1-receptor antagonist [HSA:3269] [PATH:hsa04080(3269)] [test]";
//            var matches = Regex.Matches(query, pattern);
//
//            foreach (Match m in matches)
//            {
//                Console.WriteLine(m.Groups[1]);
//            }
        }
    }

    public class NegativeValueException:Exception
    {
        public NegativeValueException()
            : base("Negatives Not allowed")
        {
            
        }
    }

    public class Calculator
    {
        public int Add(string inputString)
        {
            string [] delimeters = new string[50];
            if (inputString.Contains("//"))
            {
                
                var pattern = @"\[(.*?)\]";
                var query = inputString;
                var matches = Regex.Matches(query, pattern);

                for (int i = 0; i < matches.Count; i++)
                {
                    //Console.WriteLine(matches[i].Groups[1]);
                    delimeters[i] = matches[i].Groups[1].ToString();
                }

                int firstemptyspace = 0;
                for (int j = 0; j<delimeters.Length; j++)
                {
                    if (delimeters[j] == null)
                    {
                        firstemptyspace = j;
                        break;
                    }
                }
                int indexofnewlinechar = inputString.IndexOf('\n');
                int lengthofdelimiter = indexofnewlinechar - 2;  //-2 for //

                if (!inputString.Contains('[') && !inputString.Contains(']'))
                {
                    delimeters[firstemptyspace] = inputString.Substring(indexofnewlinechar - lengthofdelimiter, lengthofdelimiter);
                }
                inputString = inputString.Substring(indexofnewlinechar, inputString.Length - indexofnewlinechar);
            }

            if (inputString.Contains("-"))
            {
                throw new NegativeValueException();
            }

            int firstemptyspace2 = 0;
            for (int j = 0; j < delimeters.Length; j++)
            {
                if (delimeters[j] == null)
                {
                    firstemptyspace2 = j;
                    break;
                }
            }


            delimeters[firstemptyspace2] = '\n'.ToString();
            delimeters[firstemptyspace2+1] = ','.ToString();

            var sumamount = 0;
            var numbers = inputString.Split(delimeters,10,StringSplitOptions.None);

            
            foreach (var number in numbers)
            {
                if (number != String.Empty && Convert.ToInt32(number) <=1000)
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
        [ExpectedException(typeof(NegativeValueException))]
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
        [ExpectedException(typeof(NegativeValueException))]
        public void NegativesThrowsException()
        {
            string negativeinput = "//&\n-5&7";
            try
            {
                var sut = new Calculator().Add(negativeinput);
            }
            catch (NegativeValueException nve)
            {
                Assert.That(nve.Message, Is.EqualTo("Negatives Not allowed"));
                throw;
            }
        }

        [Test]
        public void NumberBiggerThen1000Ignored()
        {
            string bignumberinput = "1002,2";
            var sut = new Calculator().Add(bignumberinput);
            Assert.That(sut, Is.EqualTo(2));
        }

        [TestCase("//[***]\n1***2***3",6)]
        [TestCase("//[///]\n1///2///3", 6)]
        [TestCase("//[[[[]\n1[[[2[[[3", 6)]
        public void LongDelimitersSpecified(string input, int result)
        {
            //string longDelimitersinputstring = "//[***]\n1***2***3";
            var sut = new Calculator().Add(input);
            Assert.That(sut, Is.EqualTo(result));
        }

        [Test]
        public void MultipletDelimeters()
        {
            string inputstring = "//[*][%]\n1*2%3";
            var sut = new Calculator().Add(inputstring);
            Assert.That(sut,Is.EqualTo(6));
        }

        [Test]
        public void MultipletDelimetersofanyLength()
        {
            string inputstring = "//[***][%%%]\n1***2%%%3";
            var sut = new Calculator().Add(inputstring);
            Assert.That(sut, Is.EqualTo(6));
        }

    }
}

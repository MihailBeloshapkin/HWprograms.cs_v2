using Microsoft.VisualBasic;
using NUnit.Framework;
using System;
using System.Net.Http.Headers;


namespace HW1T1
{
    public class MatrixTest
    {
        Matrix matrix;

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void IsEqualToTest()
        {
            matrix = new Matrix("../../../matrix1.txt");
            Assert.IsTrue(matrix.IsEqualTo(new int[,] { { 2, 1, 9 }, { 4, 5, 8 }, { 7, 3, 6 } }));
            Assert.IsFalse(matrix.IsEqualTo(new int[,] { { 1, 2, 3 }, { 4, 7, 8 }, { 9, 3, 4 } }));
        }

        [Test]
        public void SimpleLoadingTest()
        {
            matrix = new Matrix("../../../matrix1.txt");
            Assert.IsTrue(matrix.IsEqualTo(new int[,] { { 2, 1, 9}, { 4, 5, 8 }, { 7, 3, 6 } }));
        }

        [Test]
        public void IncorrectInputTest()
        {
            Assert.Throws<UnrecognisedCharException>(() => new Matrix("../../../incorrectInput.txt"));
        }
    }
}

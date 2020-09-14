using Microsoft.VisualBasic;
using NUnit.Framework;
using System;
using System.Net.Http.Headers;

namespace HW1T1
{
    public class Tests
    {
        private Matrix matrix1;
        private Matrix matrix2;

        [SetUp]
        public void Setup()
        {
            matrix1 = null;
            matrix2 = null;
        }

        [Test]
        public void SimpleMultiplicationTest1()
        {
            matrix1 = new Matrix(new int[,] { { 1, 2, 3 } });
            matrix2 = new Matrix(new int[,] { { 1 }, { 2 }, { 3 } });
            Assert.IsTrue(Multiplication.SimpleMultiplication(matrix1, matrix2).IsEqualTo(new int[,] { { 14 } }));
        }

        [Test]
        public void SimpleMultiplicationTest2()
        {
            matrix1 = new Matrix(new int[,] { { 30 } });
            matrix2 = new Matrix(new int[,] { { 40 } });
            Assert.IsTrue(Multiplication.SimpleMultiplication(matrix1, matrix2).IsEqualTo(new int[,] { { 1200 } }));
        }

        [Test]
        public void SimpleMultiplicationOfQuadraticMatricesTest1()
        {
            matrix1 = new Matrix(new int[,] { { 2, 1, 9 }, { 4, 5, 8 }, { 7, 3, 6 } });
            matrix2 = new Matrix(new int[,] { { 7, 8, 0 }, { 3, 9, 1 }, { 5, 6, 2 } });
            Assert.IsTrue(Multiplication.SimpleMultiplication(matrix1, matrix2).IsEqualTo(new int[,] { { 62, 79, 19 }, { 83, 125, 21 }, { 88, 119, 15 } }));
        }

        [Test]
        public void ParallelMultiplicationTest1()
        {
            matrix1 = new Matrix(new int[,] { { 1, 2, 3 } });
            matrix2 = new Matrix(new int[,] { { 1 }, { 2 }, { 3 } });
            Assert.IsTrue(Multiplication.ParallelMultiplication(matrix1, matrix2).IsEqualTo(new int[,] { { 14 } }));
        }

        [Test]
        public void ParallelMultiplicationTest2()
        {
            matrix1 = new Matrix(new int[,] { { 30 } });
            matrix2 = new Matrix(new int[,] { { 40 } });
            Assert.IsTrue(Multiplication.SimpleMultiplication(matrix1, matrix2).IsEqualTo(new int[,] { { 1200 } }));
        }

        [Test]
        public void IncorrectInputForSimpeMultiplicationTest()
        {
            matrix1 = new Matrix(new int[,] { { 1, 2, 3 }, { 4, 2, 1 } });
            matrix2 = new Matrix(new int[,] { { 3, 2, 1 }, { 4, 2, 1} });
            Assert.Throws<ArgumentException>(() => Multiplication.SimpleMultiplication(matrix1, matrix2));
        }

        [Test]
        public void IncorrectInputForParallelMultiplicationTest()
        {
            matrix1 = new Matrix(new int[,] { { 1, 2, 3 }, { 4, 2, 1 } });
            matrix2 = new Matrix(new int[,] { { 3, 2, 1 }, { 4, 2, 1 } });
            Assert.Throws<ArgumentException>(() => Multiplication.ParallelMultiplication(matrix1, matrix2));
        }

    }
}
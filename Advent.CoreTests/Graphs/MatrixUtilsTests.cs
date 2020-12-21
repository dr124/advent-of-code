using Microsoft.VisualStudio.TestTools.UnitTesting;
using Advent.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Core.Tests
{
    [TestClass()]
    public class MatrixUtilsTests
    {
        [TestMethod()]
        public void BoolToIntMatrixTest()
        {
            var matrix = new[,]
            {
                {true, true, false},
                {true, false, false},
                {false, true, false},
                {false, false, false},
            };

            var expected = new[,]
            {
                {1, 1, 0},
                {1, 0, 0},
                {0, 1, 0},
                {0, 0, 0}
            };

            var result = matrix.BoolToIntMatrix();

            for (int i = 0; i < expected.GetLength(0); i++)
                for (int j = 0; j < expected.GetLength(1); j++)
                    Assert.AreEqual(expected[i, j], result[i, j]);
        }

        [TestMethod()]
        public void RotateTest()
        {
            var m1 = new[,]
            {
                {1, 0, 0, 0},
                {1, 0, 1, 0},
                {0, 1, 1, 0}
            };
            var expected = new[,]
            {
                {0, 1, 1},
                {1, 0, 0},
                {1, 1, 0},
                {0, 0, 0}
            };
            var result = m1.Rotate();

            for (int i = 0; i < expected.GetLength(0); i++)
                for (int j = 0; j < expected.GetLength(1); j++)
                    Assert.AreEqual(expected[i, j], result[i, j]);
        }

        [TestMethod()]
        public void FlipUDTest()
        {
            var m1 = new[,]
            {
                {1, 0, 0, 1},
                {1, 0, 1, 0},
                {0, 1, 1, 0}
            };
            var expected = new[,]
            {
                {0, 1, 1, 0},
                {1, 0, 1, 0},
                {1, 0, 0, 1}
            };
            var result = m1.FlipUD();

            for (int i = 0; i < expected.GetLength(0); i++)
            for (int j = 0; j < expected.GetLength(1); j++)
                Assert.AreEqual(expected[i, j], result[i, j]);
        }

        [TestMethod()]
        public void FlipLRTest()
        {
            var m1 = new[,]
            {
                {1, 0, 0, 0},
                {1, 0, 1, 1},
                {0, 1, 1, 0}
            };
            var expected = new[,]
            {
                {0, 0, 0, 1},
                {1, 1, 0, 1},
                {0, 1, 1, 0}
            };
            var result = m1.FlipLR();

            for (int i = 0; i < expected.GetLength(0); i++)
            for (int j = 0; j < expected.GetLength(1); j++)
                Assert.AreEqual(expected[i, j], result[i, j]);
        }
    }
}
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
            var matrix = new bool[,]
            {
                {true, true, false},
                {true, false, false},
                {false, true, false},
                {false, false, false},
            };

            var expected = new int[,]
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
    }
}
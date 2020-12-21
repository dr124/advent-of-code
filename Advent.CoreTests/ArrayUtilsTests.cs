using Microsoft.VisualStudio.TestTools.UnitTesting;
using Advent.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Core.Tests
{
    [TestClass()]
    public class ArrayUtilsTests
    {
        [TestMethod()]
        public void ToBitMapTest()
        {
            {
                var arr = new[] {true, false, false, true, false};
                var expected = 1 + 0 + 0 + 8 + 0;

                var result = arr.ToBitMap();
                Assert.AreEqual(expected, result);
            }

            {
                var arr = new bool [0];
                var expected = 0;

                var result = arr.ToBitMap();
                Assert.AreEqual(expected, result);
            }

            {
                var arr = new [] {false,true };
                var expected = 2;

                var result = arr.ToBitMap();
                Assert.AreEqual(expected, result);
            }
        }
    }
}
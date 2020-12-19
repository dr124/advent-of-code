using Microsoft.VisualStudio.TestTools.UnitTesting;
using Advent.Core.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Core.Graphs.Tests
{
    [TestClass()]
    public class MatchingTests
    {
        [TestMethod()]
        public void MaxFlowTest_1()
        {
            var graph = new int[,]
            {
                {0,1,0 },
                {1,1,1 },
                {0,1,0 }
            };

            var matching = new Matching().MaxFlow(graph);

            Assert.AreEqual(2, matching);
        }

        [TestMethod()]
        public void MaxFlowTest_2()
        {
            var graph = new int[,]
            {
                {0, 1, 1, 0, 0},
                {1, 1, 0, 1, 0},
                {0, 0, 0, 0, 1},
                {0, 1, 0, 1, 1},
                {0, 1, 1, 0, 0}
            };

            var matching = new Matching().MaxFlow(graph);

            Assert.AreEqual(5, matching);
        }
    }
}
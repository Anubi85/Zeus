using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zeus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Tests
{
    [TestClass()]
    public class CircularIndexTests
    {
        [TestMethod()]
        public void CircularIndexCastTest()
        {
            Assert.AreEqual(0, new CircularIndex(5));
            Assert.AreEqual(3, new CircularIndex(5, 3));
            Assert.AreEqual(0, new CircularIndex(5, 5));
        }

        [TestMethod()]
        public void CircularIndexAddTest()
        {
            CircularIndex idx = new CircularIndex(5);
            Assert.AreEqual(4, idx + 4);
            idx += 2;
            Assert.AreEqual(2, idx);
            idx += 4;
            Assert.AreEqual(1, idx);
        }

        [TestMethod()]
        public void CircularIndexIncrementTest()
        {
            CircularIndex idx = new CircularIndex(5);
            Assert.AreEqual(0, idx++);
            Assert.AreEqual(2, ++idx);
        }

        [TestMethod()]
        public void CircularIndexSubTest()
        {
            CircularIndex idx = new CircularIndex(5);
            Assert.AreEqual(3, idx -2);
            idx -= 2;
            Assert.AreEqual(3, idx);
            idx -= 4;
            Assert.AreEqual(4, idx);
        }

        [TestMethod()]
        public void CircularIndexDecrementTest()
        {
            CircularIndex idx = new CircularIndex(5);
            Assert.AreEqual(0, idx--);
            Assert.AreEqual(3, --idx);
        }
    }
}
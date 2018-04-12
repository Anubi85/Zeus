using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Zeus.Tests
{
    [TestClass()]
    public class FormatParserTests
    {
        [TestMethod()]
        public void ParseTest()
        {
            Dictionary<string, string> keys = new Dictionary<string, string>();
            keys["Date"] = "0";
            keys["Time"] = "0";
            keys["Id"] = "1";
            keys["Level"] = "2";
            string toParse = "{date:yyyy-mm-dd};{TIME:HH:MM};{iD};{LEVEL}";
            string parsed = FormatParser.Parse(toParse, keys);
            Assert.AreEqual("{0:yyyy-mm-dd};{0:HH:MM};{1};{2}", parsed);
        }
    }
}
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSUtils;

namespace CoreTesting
{
    [TestClass]
    public class ExtensionMethodsNullableUtilsTest
    {
        [TestMethod]
        public void ParseUInt_WithValidNumber()
        {
            System.Diagnostics.Debug.WriteLine("Parsing UInt with a Valid number");
            string validToParse = "51";
            var test = ExtensionMethodsNullableUtils.ParseUInt(validToParse);
            Assert.AreEqual((uint)51, test);
        }
        [TestMethod]
        public void ParseUInt_WithNegativeNumbers()
        {
            string negativeToParse = "-51";
            Assert.IsNull(ExtensionMethodsNullableUtils.ParseUInt(negativeToParse));
        }
        [TestMethod]
        public void ParseUInt_WithNull()
        {
            Assert.IsNull(ExtensionMethodsNullableUtils.ParseUInt(null));
        }
        [TestMethod]
        public void ParseUInt_WithEmpty()
        {
            Assert.IsNull(ExtensionMethodsNullableUtils.ParseUInt(""));
        }
    }
}

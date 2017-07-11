using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSUtils;

namespace CoreTesting
{
    [TestClass]
    public class ExtensionMethodsNullableUtilsTest
    {
        [TestMethod]
        public void TryToString_NullObject()
        {
            Assert.AreEqual("",ExtensionMethodsNullableUtils.TryToString(null));
        }
        [TestMethod]
        public void TryToString_StringableObject()
        {
            Assert.AreEqual("12", ExtensionMethodsNullableUtils.TryToString(12));
        }
        [TestMethod]
        public void TryValue_Int_ValidInt()
        {
            Assert.AreEqual(12, ExtensionMethodsNullableUtils.TryValue(12));
        }
        [TestMethod]
        public void TryValue_Int_OverflowLong()
        {
            Assert.AreEqual(-2147483649L, ExtensionMethodsNullableUtils.TryValue(-2147483649));
            Assert.IsNotInstanceOfType(ExtensionMethodsNullableUtils.TryValue(-2147483649), typeof(int));
            Assert.IsInstanceOfType(ExtensionMethodsNullableUtils.TryValue(-2147483649), typeof(float));
        }
        [TestMethod]
        public void ParseUInt_WithValidNumber()
        {
            Assert.AreEqual(51u, ExtensionMethodsNullableUtils.ParseUInt("51"));
        }
        [TestMethod]
        public void ParseUInt_WithNegativeNumbers()
        {
            Assert.IsNull(ExtensionMethodsNullableUtils.ParseUInt("-51"));
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

using System;
using NUnit.Framework;
using NSUtils.Touch.Service;

namespace TouchTesting.NSUtils.Touch.Service
{
    [TestFixture]
    public class TouchStorageTest
    {
        [Test]
        public void GetUndefinedBoolKey_asFalse()
        {
            var _settings = new TouchStorage();
            Assert.IsFalse(_settings.GetBool("UndefinedBool"));
        }
        [Test]
        public void GetUndefinedBoolKey_asTrue()
        {
            var _settings = new TouchStorage();
            Assert.IsTrue(_settings.GetBool("UndefinedBool", true));
        }
        

        [Test]
        [Ignore("another time")]
        public void Ignore()
        {
            Assert.True(false);
        }
    }
}
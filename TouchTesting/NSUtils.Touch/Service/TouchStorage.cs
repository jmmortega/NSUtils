using System;
using NUnit.Framework;
using NSUtils.Touch.Service;
using System.Threading;
using System.Globalization;
using System.Diagnostics;

namespace TouchTesting.NSUtils.Touch.Service
{
    [TestFixture]
    public class TouchStorageTest
    {
        TouchStorage _settings;
        Stopwatch SWLegacy;
        Stopwatch SWNew;
        [SetUp]
        public void Init()
        {
            _settings = new TouchStorage();
            SWLegacy = new Stopwatch();
            SWNew = new Stopwatch();
        }
        [Test]
        public void GetUndefinedBoolKey_asFalse()
        {
            Assert.IsFalse(_settings.GetBool("UndefinedBool"));
        }
        [Test]
        public void GetUndefinedBoolKey_asTrue()
        {
            Assert.IsTrue(_settings.GetBool("UndefinedBool", true));
        }
        [Test]
        public void SetAndGetDouble_DifferentCultures()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            _settings.SetDouble("test",52.12d);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
            Assert.AreEqual(52.12d,_settings.GetDouble("test",0));

            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
            _settings.SetDouble("test", 52.12d);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            Assert.AreEqual(52.12d, _settings.GetDouble("test", 0));
        }
        [Test]
        public void Perfomance_SetInt()
        {
            SWLegacy.Start();
            _settings.Legacy_SetInt("testL", int.MaxValue);
            SWLegacy.Stop();
            SWNew.Start();
            _settings.SetInt("testN", int.MaxValue);
            SWNew.Stop();
            Console.WriteLine("SetInt New:{0} Legacy:{1}", SWNew.ElapsedMilliseconds, SWLegacy.ElapsedMilliseconds);
            Assert.True(SWNew.ElapsedMilliseconds <= SWLegacy.ElapsedMilliseconds);
        }
        [Test]
        public void Perfomance_SetDouble()
        {
            SWLegacy.Start();
            _settings.Legacy_SetDouble("testL", float.MaxValue);
            SWLegacy.Stop();
            SWNew.Start();
            _settings.SetDouble("testN", float.MaxValue);
            SWNew.Stop();
            Console.WriteLine("SetDouble New:{0} Legacy:{1}", SWNew.ElapsedMilliseconds, SWLegacy.ElapsedMilliseconds);
            Assert.True(SWNew.ElapsedMilliseconds <= SWLegacy.ElapsedMilliseconds);
        }
        [Test]
        public void Perfomance_SetBool()
        {
            SWLegacy.Start();
            _settings.Legacy_SetBool("testL", true);
            SWLegacy.Stop();
            SWNew.Start();
            _settings.SetBool("testN", true);
            SWNew.Stop();
            Console.WriteLine("SetBool New:{0} Legacy:{1}", SWNew.ElapsedMilliseconds, SWLegacy.ElapsedMilliseconds);
            Assert.True(SWNew.ElapsedMilliseconds <= SWLegacy.ElapsedMilliseconds);
        }
        [Test]
        public void Perfomance_GetInt_Undefined()
        {
            SWLegacy.Start();
            _settings.Legacy_GetInt("testL");
            SWLegacy.Stop();
            SWNew.Start();
            _settings.GetInt("testN");
            SWNew.Stop();
            Console.WriteLine("GetUndefinedInt New:{0} Legacy:{1}", SWNew.ElapsedMilliseconds, SWLegacy.ElapsedMilliseconds);
            Assert.True(SWNew.ElapsedMilliseconds <= SWLegacy.ElapsedMilliseconds);
        }
        [Test]
        public void Perfomance_GetInt_Defined()
        {
            _settings.SetInt("test", int.MaxValue);
            SWLegacy.Start();
            _settings.Legacy_GetInt("test");
            SWLegacy.Stop();
            SWNew.Start();
            _settings.GetInt("test");
            SWNew.Stop();
            Console.WriteLine("GetDefinedInt New:{0} Legacy:{1}", SWNew.ElapsedMilliseconds, SWLegacy.ElapsedMilliseconds);
            Assert.True(SWNew.ElapsedMilliseconds <= SWLegacy.ElapsedMilliseconds);
        }
        [Test]
        public void Perfomance_GetDouble_Undefined()
        {
            SWLegacy.Start();
            _settings.Legacy_GetDouble("testL");
            SWLegacy.Stop();
            SWNew.Start();
            _settings.GetDouble("testN");
            SWNew.Stop();
            Console.WriteLine("GetUndefinedDouble New:{0} Legacy:{1}", SWNew.ElapsedMilliseconds, SWLegacy.ElapsedMilliseconds);
            Assert.True(SWNew.ElapsedMilliseconds <= SWLegacy.ElapsedMilliseconds);
        }
        [Test]
        public void Perfomance_GetDouble_Defined()
        {
            _settings.SetDouble("test", float.MaxValue);
            SWLegacy.Start();
            _settings.Legacy_GetDouble("test");
            SWLegacy.Stop();
            SWNew.Start();
            _settings.GetDouble("test");
            SWNew.Stop();
            Console.WriteLine("GetDefinedDouble New:{0} Legacy:{1}", SWNew.ElapsedMilliseconds, SWLegacy.ElapsedMilliseconds);
            Assert.True(SWNew.ElapsedMilliseconds <= SWLegacy.ElapsedMilliseconds);
        }
        [Test]
        public void Perfomance_GetBool_Undefined()
        {
            SWLegacy.Start();
            _settings.Legacy_GetBool("testL");
            SWLegacy.Stop();
            SWNew.Start();
            _settings.GetBool("testN");
            SWNew.Stop();
            Console.WriteLine("GetUndefinedBool New:{0} Legacy:{1}", SWNew.ElapsedMilliseconds, SWLegacy.ElapsedMilliseconds);
            Assert.True(SWNew.ElapsedMilliseconds <= SWLegacy.ElapsedMilliseconds);
        }
        [Test]
        public void Perfomance_GetBool_Defined()
        {
            _settings.SetBool("test", true);
            SWLegacy.Start();
            _settings.Legacy_GetBool("test");
            SWLegacy.Stop();
            SWNew.Start();
            _settings.GetBool("test");
            SWNew.Stop();
            Console.WriteLine("GetDefinedBool New:{0} Legacy:{1}", SWNew.ElapsedMilliseconds, SWLegacy.ElapsedMilliseconds);
            Assert.True(SWNew.ElapsedMilliseconds <= SWLegacy.ElapsedMilliseconds);
        }
    }
}
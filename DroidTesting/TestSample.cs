using System;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Threading;
using NSUtils.Droid.Service;

namespace DroidTesting
{
    [TestFixture]
    public class TestsSample
    {

        [SetUp]
        public void Setup() { }


        [TearDown]
        public void Tear() { }

        [Test]
        public void ProgressTask()
        {
            DroidDialog.ShowLoading( () =>
            {
                Console.WriteLine("$$$$$PreWait$$$$$");
                Thread.Sleep(5000);
                Console.WriteLine("$$$$$PostWait$$$$$");
            }, 3000);
        }
    }
}
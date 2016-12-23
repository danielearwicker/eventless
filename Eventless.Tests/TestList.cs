using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Eventless.Tests
{
    [TestClass]
    public class TestList
    {
        [TestMethod]
        public void TestSimple()
        {
            var l = new MutableList<string>();

            var notified = 0;

            l.PropertyChanged += (s, e) => notified++;
            
            l.Value.Add("a");
            
            Assert.AreEqual(1, notified);
            
            l.Value.RemoveAt(0);

            Assert.AreEqual(2, notified);            
        }

        [TestMethod]
        public void TestReentrance()
        {
            var l = new MutableList<int>(new [] { 1, 2, 3, 4, 5, 6 });

            // Set it up so that if any item is changed, they all change to 10!
            l.PropertyChanged += (s, e) =>
                {
                    for (var i = 0; i < l.Value.Count; i++)
                        l.Value[i] = 10;
                };

            l.Value[3] = 10; // set one of them to 10, should be fine

            foreach (var t in l.Value)
                Assert.AreEqual(10, t);
        }

        [TestMethod]
        [ExpectedException(typeof(RecursiveModificationException))]
        public void TestRecursion()
        {
            var l = new MutableList<int>(new[] { 1, 2, 3, 4, 5, 6 });

            // Same set up (any changes => all change to 10)
            l.PropertyChanged += (s, e) =>
            {
                for (var i = 0; i < l.Value.Count; i++)
                    l.Value[i] = 10;
            };

            l.Value[3] = 11; // But now set one to 11, so handler will try to change it again (boom)
        }
    }
}
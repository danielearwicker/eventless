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
            var l = new WriteableList<string>();

            var added = new List<int>();
            var removed = new List<int>();
            var updated = new List<int>();
            var cleared = 0;

            l.Added += added.Add;
            l.Removed += removed.Add;
            l.Updated += updated.Add;
            l.Cleared += () => cleared++;

            l.Add("a");
            l.Add("b");
            l.Add("c");
            l.Add("d");

            Assert.AreEqual(4, added.Count);
            Assert.AreEqual(0, added[0]);
            Assert.AreEqual(1, added[1]);
            Assert.AreEqual(2, added[2]);
            Assert.AreEqual(3, added[3]);

            l.RemoveAt(1);

            Assert.AreEqual(4, added.Count);
            Assert.AreEqual(1, removed.Count);
            Assert.AreEqual(1, removed[0]);

            l.Insert(1, "x");

            Assert.AreEqual(5, added.Count);
            Assert.AreEqual(1, added[4]);
            Assert.AreEqual(1, removed.Count);

            l[2] = "y";

            Assert.AreEqual(5, added.Count);
            Assert.AreEqual(1, removed.Count);
            Assert.AreEqual(1, updated.Count);
            Assert.AreEqual(2, updated[0]);

            Assert.AreEqual("a", l[0]);
            Assert.AreEqual("x", l[1]);
            Assert.AreEqual("y", l[2]);
            Assert.AreEqual("d", l[3]);
        }

        [TestMethod]
        public void TestReentrance()
        {
            var l = new WriteableList<int> {1, 2, 3, 4, 5, 6};

            // Set it up so that if any item is changed, they all change to 10!
            l.Changed += () =>
                {
                    for (var i = 0; i < l.Count; i++)
                        l[i] = 10;
                };

            l[3] = 10; // set one of them to 10, should be fine

            foreach (var t in l)
                Assert.AreEqual(10, t);
        }

        [TestMethod]
        [ExpectedException(typeof(RecursiveModificationException))]
        public void TestRecursion()
        {
            var l = new WriteableList<int> { 1, 2, 3, 4, 5, 6 };

            // Same set up (any changes => all change to 10)
            l.Changed += () =>
            {
                for (var i = 0; i < l.Count; i++)
                    l[i] = 10;
            };

            l[3] = 11; // But now set one to 11, so handler will try to change it again (boom)
        }
    }
}
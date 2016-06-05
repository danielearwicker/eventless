﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Eventless.Tests
{
    [TestClass]
    public class TestSetable
    {
        [TestMethod]
        public void TestSimpleUnchanged()
        {
            var w = Setable.From(0);
            w.Changed += () => { throw new Exception("Shouldn't fire Changed event if value is unchanged"); };
            w.Value = 0;
        }

        [TestMethod]
        public void TestSimpleChanged()
        {
            var w = Setable.From(0);
            var changed = false;
            w.Changed += () => changed = true;
            w.Value = 1;
            Assert.IsTrue(changed);
        }

        [TestMethod]
        public void TestCustomEquality()
        {
            // Default comparison uses equals method
            var changed = false;
            var w = Setable.From(new[] { 5, 20, 13 });
            w.Changed += () => changed = true;

            w.Value = new[] { 5, 20, 13 }; // even though contents are same, still counts as changed
            Assert.IsTrue(changed);
            changed = false;

            // Replace with order-agnostic sequence comparison
            w.EqualityComparer = (a, b) => a.OrderBy(i => i).SequenceEqual(b.OrderBy(i => i));

            // Same contents in different order, no Changed event
            w.Value = new[] { 5, 13, 20 };
            Assert.IsFalse(changed);

            // Change one value and it triggers Changed
            w.Value = new[] { 5, 3, 20 };
            Assert.IsTrue(changed);
        }

        [TestMethod]
        public void TestReentrance()
        {
            var w = Setable.From(0);

            // Okay to assign to writeable that just changed as long as it's the same (new) value
            w.Changed += () => w.Value = 1;
            w.Value++;
        }

        [TestMethod]
        [ExpectedException(typeof(RecursiveModificationException))]
        public void TestRecursion()
        {
            var w = Setable.From(0);

            // Not okay to set it to a different value (hence expected exception)
            w.Changed += () => w.Value++;;
            w.Value++;
        }
    }
}

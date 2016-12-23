using System;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Eventless.Tests
{
    [TestClass]
    public class TestComputed
    {
        [TestMethod]
        public void TestNotification()
        {
            var x = Mutable.From(1);
            var y = Mutable.From(2);
            var z = Computed.From(() => x.Value + y.Value);

            Assert.AreEqual(3, z.Value);
            Assert.AreEqual(false, z.IsActive);

            x.Value = 3;

            Assert.AreEqual(5, z.Value);
            Assert.AreEqual(false, z.IsActive);

            var p = Computed.From(() => z.Value*2);
            Assert.AreEqual(10, p.Value);
            Assert.AreEqual(false, p.IsActive);
            Assert.AreEqual(false, z.IsActive);

            var notified = 0;

            PropertyChangedEventHandler changed = (s, e) => notified++;

            p.PropertyChanged += changed;

            Assert.AreEqual(true, p.IsActive);
            Assert.AreEqual(true, z.IsActive);
            Assert.AreEqual(0, notified);

            y.Value = 1;

            Assert.AreEqual(true, p.IsActive);
            Assert.AreEqual(true, z.IsActive);
            Assert.AreEqual(1, notified);
            Assert.AreEqual(8, p.Value);

            p.PropertyChanged -= changed;

            Assert.AreEqual(false, p.IsActive);
            Assert.AreEqual(false, z.IsActive);
            Assert.AreEqual(1, notified);
            Assert.AreEqual(8, p.Value);

            x.Value = 1;

            Assert.AreEqual(false, p.IsActive);
            Assert.AreEqual(false, z.IsActive);
            Assert.AreEqual(1, notified);
            Assert.AreEqual(4, p.Value);
        }
    }
}

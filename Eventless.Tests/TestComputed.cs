using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Eventless.Tests
{
    [TestClass]
    public class TestComputed
    {
        [TestMethod]
        public void TestSimple()
        {
            var i = Writeable.From(2);
            var j = Writeable.From(3);

            // Implicit conversion to value seems cool here!
            var sum = Computed.From(() => i + j);

            // But sometimes it's a pain...
            Assert.AreEqual(5, sum.Value); 

            i.Value++;
            Assert.AreEqual(6, sum.Value); 
       }

        [TestMethod]
        public void TestImperative()
        {
            var i = Writeable.From(2);
            var j = Writeable.From(3);
            var sum = Computed.From(() => i + j);
            var doubled = Computed.From(() => sum.Value * 2);

            var executionCount = 0;

            Computed.Do(() =>
                {
                    Assert.AreEqual(sum.Value, i + j);
                    Assert.AreEqual(doubled.Value, sum.Value * 2);
                    executionCount++;
                });

            i.Value++;

            // Should have executed 4 times: init and then mutation of i, each time changing two observed readables
            Assert.AreEqual(4, executionCount);
        }
    }
}

using DotNet = System;
using NUnit.Framework;

namespace Core.Test
{
    [TestFixture]
    public class ObjectTest
    {
        [Test]
        public void EqualsTest()
        {
            var o1 = new Object();
            var o2 = new Object();

            Assert.That(o1, Is.Not.EqualTo(o2));

            Assert.That(o1.MyEquals(o2), Is.EqualTo(Bool.False));
        }

        [Test]
        public void IsNullTest()
        {
            var o = new Object();
            Object o2 = null;
            DotNet.Object o3 = null;

            Assert.That(!Object.IsNull(o));
            Assert.That(Object.IsNull(o2));
            Assert.That(Object.IsNull(o3));
        }

        [Test]
        public void EqualsByReferenceTest()
        {
            var o = new Object();
            var o1 = o;

            Assert.That(Object.EqualsByReference(o, o1));
            Assert.That(!Object.EqualsByReference(o, null));
            Assert.That(Object.EqualsByReference(null, null));
            Assert.That(!Object.EqualsByReference(null, o));
        }

        
    }
}

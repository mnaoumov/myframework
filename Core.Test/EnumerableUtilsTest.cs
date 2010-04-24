using System;
using System.Collections.Generic;
using Core.Utils;
using NUnit.Framework;

namespace Core.Test
{
    [TestFixture]
    public class EnumerableUtilsTest
    {
        [Test]
        public void IsEmptyTest()
        {
            Assert.That(EmptyEnumerable().IsEmpty());
            Assert.That(NotEmptyEnumerable().IsEmpty().ToDotNetBool(),Is.False);
        }

        private static IEnumerable<Object> NotEmptyEnumerable()
        {
            yield return new Object();
        }

        private static IEnumerable<Object> EmptyEnumerable()
        {
            yield break;
        }
    }
}
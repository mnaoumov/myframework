using Core.Collections;
using NUnit.Framework;
using Core.Utils;

namespace Core.Test
{
    [TestFixture]
    public class ListTest
    {
        [Test]
        public void AddTest()
        {
            var list = new List<String> {"a", "b", "c"};

            String first = null;
            String second = null;
            String third = null;

            foreach (var s in list)
            {
                if (first == null)
                {
                    first = s;
                    Assert.That(s.Equals("a"));
                }
                else if (second==null)
                {
                    second = s;
                    Assert.That(s.Equals("b"));
                }
                else if (third==null)
                {
                    third = s;
                    Assert.That(s.Equals("c"));
                }
                else
                {
                    Assert.Fail();
                }
            }
        }

        [Test]
        public void EmptyListTest()
        {
            var emptyList = new List<String>();

            Assert.That(emptyList.IsEmpty());
            Assert.That(emptyList.IsEmpty<String>());

            var notEmptyList = new List<String>{"test"};

            Assert.That(!notEmptyList.IsEmpty());
            Assert.That(!notEmptyList.IsEmpty<String>());
        }
    }
}
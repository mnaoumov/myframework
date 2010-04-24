using NUnit.Framework;

namespace Core.Test
{
    [TestFixture]
    public class StringTest
    {
        [Test]
        public void EqualsTest()
        {
            String test = "test";
            String test2 = "test";
            String test3 = "test ";

            Assert.That(test, Is.EqualTo(test2));
            Assert.That(test, Is.Not.EqualTo(test3));
            Assert.That(test.Equals("test"));
            Assert.That(test3.Equals("test "));
        }

        [Test]
        public void EmptyTest()
        {
            String empty = String.Empty;
            String empty2 = "";

            Assert.That(empty,Is.SameAs(empty2));

            Assert.That(String.IsNullOrEmpty(empty));
            Assert.That(!String.IsNullOrEmpty("test"));
            Assert.That(String.IsNullOrEmpty(null));
        }
    }
}
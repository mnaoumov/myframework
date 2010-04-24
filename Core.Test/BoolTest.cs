using NUnit.Framework;

namespace Core.Test
{
    [TestFixture]
    public class BoolTest
    {
        [Test]
        public void ValuesTest()
        {
            Assert.That(Bool.True.ToDotNetBool(),Is.True);
            Assert.That(Bool.False.ToDotNetBool(), Is.False);
        }

        [Test]
        public void NotTest()
        {
            Assert.That(Bool.Not(Bool.True), Is.EqualTo(Bool.False));
            Assert.That(!Bool.True, Is.EqualTo(Bool.False));

            Assert.That(Bool.Not(Bool.False), Is.EqualTo(Bool.True));
            Assert.That(!Bool.False, Is.EqualTo(Bool.True));
        }
    }
}
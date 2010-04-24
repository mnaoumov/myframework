using NUnit.Framework;
using Core.Collections;

namespace Core.Test
{
    [TestFixture]
    public class TypeTest
    {
        [Test]
        public void MyGetTypeTest()
        {
            var o = new Object();

            var myGetType = o.MyGetType();

            Type @from = Type.From<Object>();

            Type fromDotNetType = Type.FromDotNetType(typeof(Object));

            Assert.That(myGetType, Is.EqualTo(@from));
            Assert.That(myGetType, Is.EqualTo(fromDotNetType));
            Assert.That(@from, Is.EqualTo(fromDotNetType));
            Assert.That(myGetType, Is.Not.EqualTo(Type.From<Bool>()));
        }

        
        [Test]
        public void TypeToStringTest()
        {
            Assert.That(Type.From<String>().Name.Equals("String"));
            Assert.That(Type.From<Char>().Name.Equals("Char"));
            var name = Type.From<List<Char>>().Name;
            Assert.That(name.Equals("List<Char>"));
            Assert.That(Type.From<List<Pool<Char, Bool>>>().Name.Equals("List<Pool<Char,Bool>>"));
        }
        
    }
}
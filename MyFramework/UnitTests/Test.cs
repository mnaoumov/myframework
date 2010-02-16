using MyFramework.Collections;
using NUnit.Framework;

namespace MyFramework.UnitTests
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void ObjectTest()
        {
            Object o = new Object();

            Assert.AreEqual(o.ToString(), "Object");
            Assert.AreEqual(o.MyToString(), "Object");
        }

        [Test]
        public void TypeNameTest()
        {
            Assert.AreEqual(Type.GetTypeName(typeof(Bool)), "Bool");
            Assert.AreEqual(Type.GetTypeName(typeof(LinkedList<Char>)), "LinkedList<Char>");
            Assert.AreEqual(Type.GetTypeName(typeof(int)), "int");
            Assert.AreEqual(Type.GetTypeName(typeof(Dictionary<String, LinkedList<Char>>)), "Dictionary<String,LinkedList<Char>>");
        }

        [Test]
        public void BoolTest()
        {
            Assert.AreEqual(Bool.True.ToString(), "True");
            Assert.AreEqual(Bool.True.MyToString(), "True");
            Assert.AreEqual(Bool.False.ToString(), "False");
            Assert.AreEqual(Bool.False.MyToString(), "False");
        }

        [Test]
        public void StringFormatTest()
        {
            String s = String.Format("{A}{B}{C}{A}", new String.ParametersPair("A", "AAA"),
                                              new String.ParametersPair("B", "BBB"), new String.ParametersPair("C", "CCC"));

            Assert.AreEqual(s,"AAABBBCCCAAA");

            s = String.Format("{{}}{{}}");

            Assert.AreEqual(s, "{}{}");
        }

        [Test]
        public void ToStringTest()
        {
            LinkedList<String> list = new LinkedList<String>("1", "2", "3");

            Assert.AreEqual(list.MyToString(),"{1, 2, 3}");

            LinkedList<Bool> bools = new LinkedList<Bool>(Bool.True, Bool.True, Bool.False, Bool.False);

            Assert.AreEqual(bools.MyToString(),"{True, True, False, False}");

            LinkedList<Char> c = new LinkedList<Char>();

            Assert.AreEqual(c.MyToString(),"{}");

            Dictionary<String, Bool> dictionary = new Dictionary<String, Bool>(new Dictionary<String, Bool>.KeyValuePair("A", Bool.True),
                                                                         new Dictionary<String, Bool>.KeyValuePair("B", Bool.False));

            Assert.AreEqual(dictionary.MyToString(),"{(A: True), (B: False)}");

            Pool<String, Bool> pool = new Pool<String, Bool>(new Dictionary<String, Bool>.KeyValuePair("A", Bool.True),
                                                                         new Dictionary<String, Bool>.KeyValuePair("B", Bool.False));

            Assert.AreEqual(pool.MyToString(), "{(A: True), (B: False)}");

            String s = Bool.NotTable.MyToString();

            Assert.AreEqual(s, "True | False\nFalse | True");
        }
    }
}
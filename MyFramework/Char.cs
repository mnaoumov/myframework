using MyFramework.Factories;

namespace MyFramework
{
    public class Char : Object
    {
        private static readonly PooledFactory<Char, char> factory = Factory.Instance;
        private readonly char dotNetChar;

        private Char(char dotNetChar)
        {
            this.dotNetChar = dotNetChar;
        }

        public static implicit operator char(Char @char)
        {
            return @char.ToDotNetChar();
        }

        public static Char Create(char dotNetChar)
        {
            return factory.Create(dotNetChar);
        }

        public override String MyToString()
        {
            return dotNetChar.ToString();
        }

        public override Bool MyEquals(object obj)
        {
            if (obj is char)
            {
                return MyEquals((char) obj);
            }

            return base.MyEquals(obj);
        }

        private Bool MyEquals(Char @char)
        {
            return base.MyEquals(@char);
        }

        public static implicit operator Char(char dotNetChar)
        {
            return Create(dotNetChar);
        }

        public char ToDotNetChar()
        {
            return dotNetChar;
        }

        #region Nested type: Factory

        private class Factory : PooledFactory<Char, char>
        {
            private static readonly Factory instance = new Factory();

            private Factory()
            {
            }

            public static Factory Instance
            {
                get { return instance; }
            }

            protected override Char CreateNew(char dotNetChar)
            {
                return new Char(dotNetChar);
            }
        }

        #endregion
    }
}
using Core.Factories;
using DotNet = System;

namespace Core
{
    public class Char : ImmutableObject
    {
        private readonly DotNet.Char _dotNetChar;

        private Char(DotNet.Char dotNetChar)
        {
            _dotNetChar = dotNetChar;
        }

        internal DotNet.Char ToDotNetChar()
        {
            return _dotNetChar;
        }

        private static Char FromDotNetChar(char dotNetChar)
        {
            return CharFactory.Instance.GetOrCreate(dotNetChar);
        }

        private class CharFactory : PooledFactory<DotNet.Char, Char>
        {
            public static readonly CharFactory Instance = new CharFactory();

            private CharFactory()
            {

            }

            public override Char Create(char dotNetChar)
            {
                return new Char(dotNetChar);
            }
        }

        public static implicit operator Char(DotNet.Char dotNetChar)
        {
            return FromDotNetChar(dotNetChar);
        }

        public override String MyToString()
        {
            return String.ToDotNetString(_dotNetChar);
        }
    }
}
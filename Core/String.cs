using System.Collections;
using System.Collections.Generic;
using Core.Collections;
using Core.Factories;
using Core.Utils;
using DotNet = System;

namespace Core
{
    public class String : ImmutableObject, IEnumerable<Char>
    {
        public static readonly String Empty = "";
        private readonly Collections.List<Char> _chars;

        private String(IEnumerable<Char> chars)
        {
            _chars = new Collections.List<Char>(chars);
        }

        public IEnumerable<Char> Chars
        {
            get { return new ReadOnlyList<Char>(_chars); }
        }

        #region IEnumerable<Char> Members

        public IEnumerator<Char> GetEnumerator()
        {
            return _chars.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public DotNet.String ToDotNetString()
        {
            IEnumerable<char> chars = from @char in _chars select @char.ToDotNetChar();

            return new string(@chars.ToDotNetArray());
        }

        public static implicit operator String(DotNet.String dotNetString)
        {
            return Create(dotNetString);
        }

        public static String Create(IEnumerable<DotNet.Char> dotNetChars)
        {
            return StringFactory.Instance.GetOrCreate(dotNetChars);
        }

        public override String MyToString()
        {
            return this;
        }

        public static DotNet.String ToDotNetString(DotNet.Object obj)
        {
            return ToDotNetString(ToString(obj));
        }

        public static DotNet.String ToDotNetString(String @string)
        {
            return @string == null ? null : @string.ToDotNetString();
        }

        public static String ToString(DotNet.Object obj)
        {
            if (obj == null)
            {
                return null;
            }

            var objAsObject = obj as Object;

            if (objAsObject != null)
            {
                return objAsObject.MyToString();
            }

            return obj.ToString();
        }

        public override Bool MyEquals(DotNet.Object obj)
        {
            var @string = obj as String;

            if (@string != null)
            {
                return MyEquals(@string);
            }

            var dotNetString = obj as DotNet.String;

            if (dotNetString != null)
            {
                return MyEquals(dotNetString);
            }

            return Bool.False;
        }

        public Bool MyEquals(String obj)
        {
            return EqualsByReference(this, obj);
        }

        public static Bool IsNullOrEmpty(String @string)
        {
            return @string == Empty || @string == null;
        }

        public static String Create(IEnumerable<Char> chars)
        {
            return StringFactory.Instance.GetOrCreate(new Collections.List<Char>(chars));
        }

        public static String operator +(String @string, Char @char)
        {
            return Concat(@string, @char);
        }

        public static String operator +(Char @char, String @string)
        {
            return Concat(@char, @string);
        }

        public static String operator +(String string1, String string2)
        {
            return Concat(string1, string2);
        }


        public static String Concat(String @string, Char @char)
        {
            return Concat(@string, @char.MyToString());
        }

        public static String Concat(Char @char, String @string)
        {
            return Concat(@char.MyToString(), @string);
        }

        public static String Concat(params String[] strings)
        {
            IEnumerable<IEnumerable<Char>> charsSets = from @string in strings
                                                       select @string.Chars;

            return Create(charsSets.Join());
        }

        #region Nested type: StringFactory

        private class StringFactory : PooledFactory<IEnumerable<Char>, String>
        {
            public static readonly StringFactory Instance = new StringFactory();

            private StringFactory()
            {
            }

            public String GetOrCreate(IEnumerable<DotNet.Char> dotNetChars)
            {
                var chars = from dotNetChar in dotNetChars select (Char) dotNetChar;

                return GetOrCreate(new Collections.List<Char>(chars) {UseSequentialEquals = Bool.True});
            }

            public override String Create(IEnumerable<Char> chars)
            {
                return new String(chars);
            }
        }

        #endregion
    }
}
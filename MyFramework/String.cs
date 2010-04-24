using System;
using System.Collections;
using System.Collections.Generic;
using MyFramework.Collections;
using MyFramework.Delegates;
using MyFramework.Factories;
using MyFramework.Helpers;

namespace MyFramework
{
    public class String : Object, IEnumerable<Char>
    {
        public static readonly String Empty;
        private static readonly PooledFactory<String, IEnumerable<Char>> factory = Factory.Instance;

        private readonly Collections.LinkedList<Char> chars = new Collections.LinkedList<Char>();
        private string innerDotNetString;

        static String()
        {
            Empty = Create(CollectionsHelper.GetEmptyEnumerable<Char>());
        }

        private String(IEnumerable<Char> chars)
        {
            foreach (Char @char in chars)
            {
                this.chars.Add(@char);
            }
        }

        private string InnerDotNetString
        {
            get
            {
                if (innerDotNetString == null)
                {
                    PrepareInnerDotNetString();
                }

                return innerDotNetString;
            }
        }

        #region IEnumerable<Char> Members

        public IEnumerator<Char> GetEnumerator()
        {
            return chars.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public static string ToDotNetString(String s)
        {
            return s == null ? null : s.InnerDotNetString;
        }

        private void PrepareInnerDotNetString()
        {
            innerDotNetString = CollectionsHelper.ChainTransform(chars, string.Empty,
                                                                 new ChainTransformer<Char, System.String>(
                                                                     delegate(System.String result, Char item) { return result + item.ToDotNetChar(); }));
        }

        public static implicit operator String(string dotNetString)
        {
            return Create(dotNetString);
        }

        public static implicit operator string(String @string)
        {
            return ToDotNetString(@string);
        }

        private static String Create(IEnumerable<char> dotNetChars)
        {
            return
                Create(CollectionsHelper.Transform(dotNetChars,
                                                   new Transformer<char, Char>(
                                                       delegate(char argument) { return argument; })));
        }

        public override Bool MyEquals(object obj)
        {
            if (obj is string)
            {
                return MyEquals((string)obj);
            }

            return base.MyEquals(obj);
        }

        private Bool MyEquals(String @string)
        {
            return CollectionsHelper.AreEqual(chars, @string.chars);
        }

        public override String MyToString()
        {
            return this;
        }

        public static String operator +(String @string, Char @char)
        {
            return @string + @char.MyToString();
        }

        public static String operator +(String @string, char dotNetChar)
        {
            return @string + Char.Create(dotNetChar);
        }

        public static String operator +(String left, String right)
        {
            return Concat(left, right);
        }

        private static String Concat(String left, String right)
        {
            if (left == null)
            {
                left = Empty;
            }

            if (right == null)
            {
                right = Empty;
            }

            return Create(LinkedList.Join(left.chars, right.chars));
        }

        public static String Create(IEnumerable<Char> chars)
        {
            return factory.Create(chars);
        }

        public static String Format(String format, params ParametersPair[] parameters)
        {
            return new StringFormatter(format, parameters).Format();
        }

        public static String MyToString(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (obj is IEnumerable && !Type.MyGetType(obj).HasOverridenToStringMethod())
            {
                return CollectionsHelper.MyToString((IEnumerable)obj);
            }

            return obj.ToString();
        }

        #region Nested type: Factory

        private class Factory : PooledFactory<String, IEnumerable<Char>>
        {
            private static readonly Factory instance = new Factory();

            private Factory()
            {
            }

            public static Factory Instance
            {
                get { return instance; }
            }

            protected override String CreateNew(IEnumerable<Char> myChars)
            {
                return new String(myChars);
            }
        }

        #endregion

        #region Nested type: ParametersPair

        public class ParametersPair
        {
            private readonly Pair<String, object> innerPair;

            public ParametersPair(String placeHolder, object parameter)
            {
                innerPair = new Pair<String, object>(placeHolder, parameter);
            }

            public String PlaceHolder
            {
                get { return innerPair.First; }
            }

            public object Parameter
            {
                get { return innerPair.Second; }
            }
        }

        #endregion

        #region Nested type: StringFormatter

        public class StringFormatter
        {
            private readonly String format;

            private readonly Collections.Dictionary<String, String> parametersDictionary =
                new Collections.Dictionary<String, String>();

            public StringFormatter(String format, IEnumerable<ParametersPair> parameters)
            {
                this.format = format;
                PrepareParametersDictionary(parameters);
            }

            private void PrepareParametersDictionary(IEnumerable<ParametersPair> parameters)
            {
                foreach (ParametersPair parametersPair in parameters)
                {
                    parametersDictionary.Add(parametersPair.PlaceHolder, MyToString(parametersPair.Parameter));
                }
            }

            public String Format()
            {
                String formattedString = Empty;

                Bool insideParameter = Bool.False;

                String parameter = null;

                const char beforeParameterChar = '{';
                const char afterParameterChar = '}';
                Bool expectedAfterParameterChar = Bool.False;

                foreach (Char @char in format)
                {
                    if (expectedAfterParameterChar && @char != afterParameterChar)
                    {
                        throw new FormatException();
                    }

                    switch (@char)
                    {
                        case beforeParameterChar:
                            if (insideParameter)
                            {
                                if (parameter == null)
                                {
                                    insideParameter = Bool.False;
                                    formattedString += beforeParameterChar;
                                }
                                else
                                {
                                    throw new FormatException();
                                }
                            }
                            else
                            {
                                insideParameter = true;
                            }

                            break;

                        case afterParameterChar:
                            if (expectedAfterParameterChar)
                            {
                                expectedAfterParameterChar = Bool.False;
                                formattedString += afterParameterChar;
                            }
                            else if (insideParameter)
                            {
                                insideParameter = Bool.False;

                                formattedString += parametersDictionary[parameter];

                                parameter = null;
                            }
                            else
                            {
                                expectedAfterParameterChar = Bool.True;
                            }

                            break;
                        default:
                            if (insideParameter)
                            {
                                parameter += @char;
                            }
                            else
                            {
                                formattedString += @char;
                            }
                            break;
                    }
                }

                if (expectedAfterParameterChar || insideParameter)
                {
                    throw new FormatException();
                }

                return formattedString;
            }
        }

        #endregion

        public static IEnumerable<String> EnumerableToMyString(IEnumerable enumerable)
        {
            return CollectionsHelper.Transform(CollectionsHelper.ConvertToGenericEnumerable(enumerable), new Transformer<object, String>(MyToString));
        }
    }
}
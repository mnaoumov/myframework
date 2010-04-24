using System.Collections.Generic;
using Core.Collections;
using Core.Factories;
using Core.Utils;
using DotNet = System;

namespace Core
{
    public sealed class Type : ImmutableObject
    {
        private readonly IEnumerable<Type> _genericParameters;

        private Type(System.Type dotNetType)
        {
            DotNetType = dotNetType;

            IsGeneric = DotNetType.IsGenericType;
            _genericParameters = PrepareGenericParameters();

            Name = PrepareName();
        }

        private IEnumerable<Type> PrepareGenericParameters()
        {
            return from dotNetType in DotNetType.GetGenericArguments()
                   select FromDotNetType(dotNetType);
        }

        private String PrepareName()
        {
            var dotNetName = (String)DotNetType.Name;

            if (!IsGeneric)
            {
                return dotNetName;
            }

            Char genericSeparator = '`';

            var mainPart = dotNetName.Chars.TakeWhile(@char => @char != genericSeparator);

            var name = String.Create(mainPart);

            Char beforeGenericParameterChar = '<';
            Char afterGenericParameterChar = '>';

            name += beforeGenericParameterChar;
            name += (from type in GenericParameters
                     select type.Name)
                     .Aggregate((typeName1, typeName2) => typeName1 + ',' + typeName2);

            name += afterGenericParameterChar;

            return name;
        }

        public IEnumerable<Type> GenericParameters
        {
            get
            {
                return new ReadOnlyList<Type>(_genericParameters);
            }
        }

        private System.Type DotNetType { get; set; }

        public String Name { get; private set; }

        public static Type GetType(System.Object obj)
        {
            return obj == null ? null : FromDotNetType(obj.GetType());
        }

        public static Type FromDotNetType(System.Type dotNetType)
        {
            return TypeFactory.Instance.GetOrCreate(dotNetType);
        }

        public static Type From<T>()
        {
            return FromDotNetType(typeof(T));
        }

        public override String MyToString()
        {
            return Name;
        }

        private class TypeFactory : PooledFactory<DotNet.Type, Type>
        {
            public static readonly TypeFactory Instance = new TypeFactory();

            private TypeFactory()
            {

            }

            public override Type Create(System.Type key)
            {
                return new Type(key);
            }
        }

        private Bool IsGeneric { get; set; }
    }
}
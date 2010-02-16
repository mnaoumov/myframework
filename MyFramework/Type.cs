using System.Collections.Generic;
using System.Reflection;
using MyFramework.Delegates;
using MyFramework.Factories;
using MyFramework.Helpers;
using MyFramework.Wrappers;

namespace MyFramework
{
    public class Type : Object
    {
        private static readonly PooledFactory<Type, System.Type> factory = Factory.Instance;
        private readonly System.Type dotNetType;
        private LazyWrapper<Type> baseType;
        private LazyWrapper<IEnumerable<Type>> genericArguments;
        private LazyWrapper<Bool> hasOverridenToStringMethod;
        private LazyWrapper<Bool> isGenericType;
        private LazyWrapper<String> name;
        private LazyWrapper<String> nameMainPart;

        static Type()
        {
            InitializeSpecialTypes();
        }

        private Type(System.Type dotNetType)
        {
            LazyFieldsInitializer.Initialize(this);
            this.dotNetType = dotNetType;
        }

        protected virtual String Name
        {
            get { return name.Value; }
        }

        private Bool IsGenericType
        {
            get { return isGenericType.Value; }
        }

        private String NameMainPart
        {
            get { return nameMainPart.Value; }
        }

        public Type BaseType
        {
            get { return baseType.Value; }
        }

        public IEnumerable<Type> GenericArguments
        {
            get { return genericArguments.Value; }
        }

        public static implicit operator Type(System.Type dotNetType)
        {
            return Create(dotNetType);
        }

        private static void InitializeSpecialTypes()
        {
            SpecialType.Create<byte>("byte");
            SpecialType.Create<sbyte>("sbyte");
            SpecialType.Create<int>("int");
            SpecialType.Create<long>("long");
            SpecialType.Create<ushort>("ushort");
            SpecialType.Create<uint>("uint");
            SpecialType.Create<float>("float");
            SpecialType.Create<double>("double");
            SpecialType.Create<bool>("bool");
            SpecialType.Create<char>("char");
            SpecialType.Create<decimal>("decimal");
            SpecialType.Create<object>("object");
            SpecialType.Create<string>("string");
        }

        private static Type Create(System.Type dotNetType)
        {
            return dotNetType == null ? null : factory.Create(dotNetType);
        }

        public static Type Create<T>()
        {
            return Create(typeof (T));
        }

        public override String MyToString()
        {
            return Name;
        }

        public static String GetTypeName<T>()
        {
            return Create<T>().Name;
        }

        public static String GetTypeName(System.Type type)
        {
            return Create(type).Name;
        }

        public static Type MyGetType(object o)
        {
            return o == null ? null : Create(o.GetType());
        }

        public Bool IsChildOf(Type type)
        {
            return dotNetType.IsInstanceOfType(type.dotNetType);
        }

        public Bool IsParentOf(Type type)
        {
            return type.IsChildOf(this);
        }

        public static IEnumerable<Type> GetTypesTree(object source)
        {
            return GetTypesTree(MyGetType(source));
        }

        public static IEnumerable<Type> GetTypesTree(Type type)
        {
            while (type != null)
            {
                yield return type;

                type = type.BaseType;
            }
        }


        public static IEnumerable<GeneralTransformer> GetTransformers(Type typeIn, Type typeOut)
        {
            return CollectionsHelper.Join(GetTransformers(typeIn, typeOut, CastType.ImplicitCast),
                                          GetTransformers(typeIn, typeOut, CastType.ExplicitCast));
        }

        public static IEnumerable<GeneralTransformer> GetTransformers(Type typeIn, Type typeOut, CastType castType)
        {
            return CollectionsHelper.Join(GetTransformers(typeIn, typeIn, typeOut, castType),
                                          GetTransformers(typeOut, typeIn, typeOut, castType));
        }

        private static IEnumerable<GeneralTransformer> GetTransformers(Type methodSourceType, Type typeIn, Type typeOut,
                                                                       CastType castType)
        {
            return new EnumerableWrapper<MethodDelegate>(
                methodSourceType.GetMethods(castType.MethodName, typeIn))
                .Filter(
                new Predicate<MethodDelegate>(
                    delegate(MethodDelegate @delegate) { return @delegate.ReturnType == typeOut; }))
                .Transform(
                new Transformer<MethodDelegate, GeneralTransformer>(
                    delegate(MethodDelegate @delegate) { return @delegate.ToGeneralTransformer(); }));
        }

        public IEnumerable<MethodDelegate> GetMethods(String methodName, params Type[] argumentTypes)
        {
            Predicate<MethodDelegate> nameCondition =
                new Predicate<MethodDelegate>(
                    delegate(MethodDelegate @delegate) { return @delegate.Name == methodName; });
            Predicate<MethodDelegate> parameterTypesCondition =
                new Predicate<MethodDelegate>(
                    delegate(MethodDelegate @delegate) { return CollectionsHelper.AreEqual(argumentTypes, @delegate.ArgumentsTypes); });

            Predicate<MethodDelegate> condition = nameCondition & parameterTypesCondition;

            return CollectionsHelper.Filter(GetMethods(), condition);
        }

        public IEnumerable<MethodDelegate> GetMethods()
        {
            return CollectionsHelper.Transform(dotNetType.GetMethods(),
                                               new Transformer<MethodInfo, MethodDelegate>(
                                                   delegate(MethodInfo methodInfo) { return new MethodDelegate(methodInfo); }
                                                   ));
        }

        public Bool HasOverridenToStringMethod()
        {
            return hasOverridenToStringMethod.Value;
        }

        #region Nested type: Factory

        private class Factory : PooledFactory<Type, System.Type>
        {
            private static readonly Factory instance = new Factory();

            private Factory()
            {
            }

            public static Factory Instance
            {
                get { return instance; }
            }

            protected override Type CreateNew(System.Type type)
            {
                return new Type(type);
            }
        }

        #endregion

        #region Nested type: LazyFieldsInitializer

        private class LazyFieldsInitializer
        {
            private readonly Type type;

            private LazyFieldsInitializer(Type type)
            {
                this.type = type;
            }

            public static void Initialize(Type type)
            {
                new LazyFieldsInitializer(type).Initialize();
            }

            private void Initialize()
            {
                type.baseType = LazyWrapper<Type>.Create(new Initializer<Type>(InitBaseType));
                type.name = LazyWrapper<String>.Create(new Initializer<String>(InitName));
                type.isGenericType = LazyWrapper<Bool>.Create(new Initializer<Bool>(InitIsGenericType));
                type.nameMainPart = LazyWrapper<String>.Create(new Initializer<String>(InitNameMainPart));
                type.genericArguments =
                    LazyWrapper<IEnumerable<Type>>.Create(new Initializer<IEnumerable<Type>>(InitGenericArguments));
                type.hasOverridenToStringMethod =
                    LazyWrapper<Bool>.Create(new Initializer<Bool>(InitHasOverridenToStringMethod));
            }

            private Bool InitHasOverridenToStringMethod()
            {
                Type foundType;
                if (!CollectionsHelper.ContainsElement(GetTypesTree(type),
                                                       new Predicate<Type>(
                                                           delegate(Type checkType)
                                                               {
                                                                   return
                                                                       !CollectionsHelper.IsEmpty(
                                                                            checkType.GetMethods("ToString"));
                                                               }), out foundType))
                {
                    return Bool.False;
                }

                if (foundType == Create<System.Object>() || foundType == Create<Object>())
                {
                    return Bool.False;
                }

                return Bool.True;
            }


            private Bool InitIsGenericType()
            {
                return type.dotNetType.IsGenericType;
            }


            private Type InitBaseType()
            {
                return type.dotNetType.BaseType;
            }

            private String InitNameMainPart()
            {
                const char genericTypesSeparator = '`';

                Predicate<Char> predicate =
                    new Predicate<Char>(delegate(Char @char) { return @char != genericTypesSeparator; });

                return String.Create(CollectionsHelper.FilterWhile((String) type.dotNetType.Name, predicate));
            }

            private String InitName()
            {
                String myName = type.NameMainPart;

                if (!type.IsGenericType)
                {
                    return myName;
                }

                const char beforeGenericArgumentsChar = '<';
                const string genericArgumentsSeparator = ",";
                const char afterGenericArgumentsChar = '>';

                myName += beforeGenericArgumentsChar;

                Bool first = true;

                foreach (Type genericArgument in type.GenericArguments)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        myName += genericArgumentsSeparator;
                    }

                    myName += genericArgument.Name;
                }

                myName += afterGenericArgumentsChar;

                return myName;
            }


            private IEnumerable<Type> InitGenericArguments()
            {
                return CollectionsHelper.Transform(type.dotNetType.GetGenericArguments(),
                                                   new Transformer<System.Type, Type>(
                                                       delegate(System.Type argument) { return argument; }));
            }
        }

        #endregion

        #region Nested type: SpecialType

        private class SpecialType : Type
        {
            private SpecialType(System.Type specialType, String specialTypeName)
                : base(specialType)
            {
                name = specialTypeName;
            }

            protected override String Name
            {
                get { return name; }
            }

            public static void Create<TSpecialType>(string specialTypeName)
            {
                factory.Create(typeof (TSpecialType),
                               new Creator<System.Type, Type>(
                                   delegate(System.Type type) { return new SpecialType(type, specialTypeName); }));
            }
        }

        #endregion
    }
}
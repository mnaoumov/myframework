using System.Collections.Generic;
using System.Reflection;
using MyFramework.Helpers;
using MyFramework.Wrappers;

namespace MyFramework.Delegates
{
    public class MethodDelegate : BaseDelegate
    {
        private readonly LazyWrapper<IEnumerable<Type>> argumentsTypes;
        private readonly MethodInfo methodDelegate;
        private readonly LazyWrapper<String> name;
        private readonly LazyWrapper<Type> returnType;

        public MethodDelegate(MethodInfo methodInfo)
        {
            methodDelegate = methodInfo;
            argumentsTypes =
                LazyWrapper<IEnumerable<Type>>.Create(new Initializer<IEnumerable<Type>>(InitArgumentsTypes));
            returnType = LazyWrapper<Type>.Create(new Initializer<Type>(delegate { return methodDelegate.ReturnType; }));
            name = LazyWrapper<String>.Create(new Initializer<String>(delegate { return methodDelegate.Name; }));
        }

        public Type ReturnType
        {
            get { return returnType.Value; }
        }

        public String Name
        {
            get { return name.Value; }
        }

        public IEnumerable<Type> ArgumentsTypes
        {
            get { return argumentsTypes.Value; }
        }

        public object InvokeStatic(params object[] arguments)
        {
            return Invoke(null, arguments);
        }

        public object Invoke(object instance, params object[] arguments)
        {
            return methodDelegate.Invoke(instance, arguments);
        }

        private IEnumerable<Type> InitArgumentsTypes()
        {
            return CollectionsHelper.Transform(methodDelegate.GetParameters(),
                                               new Transformer<ParameterInfo, Type>(
                                                   delegate(ParameterInfo parameterInfo) { return parameterInfo.ParameterType; }));
        }

        public GeneralTransformer ToGeneralTransformer()
        {
            return new GeneralTransformer(delegate(object argument) { return InvokeStatic(argument); });
        }
    }
}
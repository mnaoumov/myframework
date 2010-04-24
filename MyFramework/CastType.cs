using System;
using MyFramework.Wrappers;

namespace MyFramework
{
    public class CastType : EnumWrapper
    {
        #region EnumValues enum

        public enum EnumValues
        {
            ImplicitCast,
            ExplicitCast
        }

        #endregion

        public static readonly CastType ExplicitCast;
        public static readonly String ExplicitMethodName = "op_Explicit";
        public static readonly CastType ImplicitCast;
        public static readonly String ImplicitMethodName = "op_Implicit";
        private readonly EnumValues enumValue;
        private readonly String methodName;

        static CastType()
        {
            ImplicitCast = new CastType(EnumValues.ImplicitCast, ImplicitMethodName);
            ExplicitCast = new CastType(EnumValues.ExplicitCast, ExplicitMethodName);

            //ImplicitCast.methodName = ImplicitMethodName;
            //ExplicitCast.methodName = ExplicitMethodName;

        }

        private CastType(EnumValues enumValue, String methodName)
        {
            this.enumValue = enumValue;
            this.methodName = methodName;
        }

        protected override Enum InnerEnum
        {
            get { return EnumValue; }
        }

        public EnumValues EnumValue
        {
            get { return enumValue; }
        }

        public String MethodName
        {
            get { return methodName; }
        }
    }
}
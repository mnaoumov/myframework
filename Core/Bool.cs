using DotNet = System;
using ArgumentException = Core.Exceptions.ArgumentException;

namespace Core
{
    public class Bool : ImmutableObject
    {
        public enum Enum { True, False }

        public readonly static Bool True = new Bool(Enum.True);
        public static readonly Bool False = new Bool(Enum.False);

        private Bool(Enum enumValue)
        {
            EnumValue = enumValue;
        }

        private Enum EnumValue { get; set; }

        public static implicit operator bool(Bool @bool)
        {
            if (@bool == null)
            {
                @bool = False;
            }

            return @bool.ToDotNetBool();
        }

        public static implicit operator Bool(DotNet.Boolean dotNetBool)
        {
            return dotNetBool ? True : False;
        }

        public static Bool operator !(Bool @bool)
        {
            return Not(@bool);
        }

        public static Bool Not(Bool @bool)
        {
            switch (@bool.EnumValue)
            {
                case Enum.True:
                    return False;
                case Enum.False:
                    return True;
                default:
                    throw new ArgumentException("@bool");
            }
        }

        public DotNet.Boolean ToDotNetBool()
        {
            switch (EnumValue)
            {
                case Enum.True:
                    return true;
                case Enum.False:
                    return false;
                default:
                    throw new ArgumentException("@bool");
            }
        }
    }
}
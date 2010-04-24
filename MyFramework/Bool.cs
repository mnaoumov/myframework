using System;
using System.Collections.Generic;
using MyFramework.Collections;
using MyFramework.Helpers;
using MyFramework.Wrappers;

namespace MyFramework
{
    public class Bool : EnumWrapper
    {
        #region EnumValues enum

        public enum EnumValues
        {
            True,
            False
        }

        #endregion

        private static readonly Table<Bool, Bool> andTable;

        public static readonly Bool False;
        public static readonly Bool True;

        private static readonly Table<Bool, Bool> notTable;

        private static readonly Table<Bool, Bool> orTable;
        private readonly EnumValues enumValue;

        static Bool()
        {
            False = new Bool(EnumValues.False);
            True = new Bool(EnumValues.True);

            notTable = new Table<Bool, Bool>(
                new Table<Bool, Bool>.Row(False, True),
                new Table<Bool, Bool>.Row(True, False)
                );

            andTable = new Table<Bool, Bool>(
                new Table<Bool, Bool>.Row(True, True, True),
                new Table<Bool, Bool>.Row(False, True, False),
                new Table<Bool, Bool>.Row(False, False, True),
                new Table<Bool, Bool>.Row(False, False, False));

            orTable = new Table<Bool, Bool>(
                new Table<Bool, Bool>.Row(True, True, True),
                new Table<Bool, Bool>.Row(True, True, False),
                new Table<Bool, Bool>.Row(True, False, True),
                new Table<Bool, Bool>.Row(False, False, False));
        }

        private Bool(EnumValues enumValue)
        {
            this.enumValue = enumValue;
        }

        public EnumValues EnumValue
        {
            get { return enumValue; }
        }

        private static Table<Bool, Bool> AndTable
        {
            get { return andTable; }
        }

        protected override Enum InnerEnum
        {
            get { return EnumValue; }
        }

        public static Table<Bool, Bool> NotTable
        {
            get { return notTable; }
        }

        public static Table<Bool, Bool> OrTable
        {
            get { return orTable; }
        }

        public static bool operator true(Bool @bool)
        {
            return @bool == True;
        }

        public static bool operator false(Bool @bool)
        {
            return @bool == False;
        }

        public static Bool operator &(Bool left, Bool right)
        {
            return And(left, right);
        }

        public static Bool operator |(Bool left, Bool right)
        {
            return Or(left, right);
        }

        public static Bool operator !(Bool @bool)
        {
            return Not(@bool);
        }

        private static Bool Not(Bool @bool)
        {
            switch (@bool.EnumValue)
            {
                case EnumValues.True:
                    return False;
                case EnumValues.False:
                    return True;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static explicit operator bool(Bool @bool)
        {
            switch (@bool.EnumValue)
            {
                case EnumValues.True:
                    return true;
                case EnumValues.False:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static implicit operator Bool(bool dotNetBool)
        {
            return dotNetBool ? True : False;
        }

        public static Bool Or(IEnumerable<Bool> bools)
        {
            foreach (Bool @bool in bools)
            {
                if (@bool)
                {
                    return True;
                }
            }

            return False;
        }

        public static Bool Or(params Bool[] bools)
        {
            return Or(CollectionsHelper.Iterate(bools));
        }

        public static Bool And(params Bool[] bools)
        {
            return And(CollectionsHelper.Iterate(bools));
        }

        public static Bool And(IEnumerable<Bool> bools)
        {
            foreach (Bool @bool in bools)
            {
                if (!@bool)
                {
                    return False;
                }
            }

            return True;
        }
    }
}
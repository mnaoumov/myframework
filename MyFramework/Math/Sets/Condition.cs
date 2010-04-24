using MyFramework.Delegates;
using MyFramework.Helpers;

namespace MyFramework.Math.Sets
{
    public class Condition<T> : SetDefinition
    {
        protected Condition(SetDescription description, Predicate<T> predicate)
        {
            
        }


        public static Condition<T> ConditionForElement(T element)
        {
            Variable variable = Variable.Create("x");
            SetDescription description =
                new SetDescription(String.Format("{{x}}={A}", new String.ParametersPair("A", element)), variable);
            Predicate<T> predicate = new Predicate<T>(delegate(T x) { return ObjectsHelper.AreEqual(x, element); });

            return new Condition<T>(description, predicate);
        }
    }
}
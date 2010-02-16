using MyFramework.Factories;

namespace MyFramework.Math.Sets
{
    public class Variable
    {
        private String name;
        private static readonly Factory factory = Factory.Instance;

        private Variable(String name)
        {
            this.name = name;
        }

        public static Variable Create(String name)
        {
            return factory.Create(name);
        }

        private class Factory:PooledFactory<Variable,String>
        {
            private Factory()
            {
                
            }
            public static readonly Factory Instance=new Factory();

            protected override Variable CreateNew(String key)
            {
                return new Variable(key);
            }
        }
    }
}
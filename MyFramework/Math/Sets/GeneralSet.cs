namespace MyFramework.Math.Sets
{
    public class GeneralSet<T> : Object
    {
        private SetDefinition leftSide;
        private SetDefinition rightSide;

        public SetDefinition LeftSide
        {
            get { return leftSide; }
        }

        public SetDefinition RightSide
        {
            get { return rightSide; }
        }
    }
}
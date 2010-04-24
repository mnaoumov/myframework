namespace MyFramework.Delegates
{
    public class Initializer<T> : BaseDelegate
    {
        private readonly Creator<T> creator;

        public Initializer(Creator<T>.InnerCreator initializer)
        {
            creator = new Creator<T>(initializer);
        }

        public T Initialize()
        {
            return creator.Create();
        }
    }
}
namespace Features.Binding.Scripts
{

    public interface IBinder
    {
        void Bind(object source);
        void Unbind();
    }

    public interface IBinder<in T> : IBinder
    {
        void Bind(T source);
    }
}
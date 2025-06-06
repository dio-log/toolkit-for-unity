namespace Features.Binding.Scripts
{
    public interface IBinder<in T>
    {
        void Bind(T source);
        void Unbind();
    }
}
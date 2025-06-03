namespace Modules.Binding.Scripts
{
    public interface IValueConvertor<TSource, TTarget>
    {
        TTarget ToTarget(TSource source);
     
        TSource ToSource(TTarget target);
    }
}
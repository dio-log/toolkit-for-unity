using System;

namespace Features.Binding.Scripts
{
    public class BindingContext<TSource, TTarget> 
    {
        public IBindable<TSource> Source { get; }
        public IBindable<TTarget> Target { get; }
        public BindingType BindingType { get; }
        public IValueConvertor<TSource, TTarget> Converter { get; }
        public Action<TSource> OnSourceChanged { get; }
        public Action<TTarget> OnTargetChanged { get; }

        public BindingContext(IBindable<TSource> source, IBindable<TTarget> target, BindingType bindingType = BindingType.OneWay, IValueConvertor<TSource, TTarget> converter = null)
        {
            Source = source;
            Target = target;
            BindingType = bindingType;
            Converter = converter;
            
            OnSourceChanged = value => Target.SetValue(Converter.ToTarget(value));
            OnTargetChanged = value => Source.SetValue(Converter.ToSource(value));
        }
    }

    public enum BindingType
    {
        OneWay,
        TwoWay,
    }
    
}
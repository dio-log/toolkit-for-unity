using System;

namespace Features.Binding.Scripts
{

    public interface IBindable
    {
        public event Action<IBindable> OnValueChanged;
        public IBindable Value { get; }
        public void SetValue(IBindable bindable);
    }
  
    public interface IBindable<T> 
    {
        public event Action<T> OnValueChanged;
        public T Value { get; }
        public void SetValue(T value);
    }
}
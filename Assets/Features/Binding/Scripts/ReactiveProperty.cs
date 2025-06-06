using System;

namespace Features.Binding.Scripts
{
    public class ReactiveProperty<T> : IBindable<T>
    {
        public event Action<T> OnValueChanged = delegate { };
        
        public T Value { get; private set; }

        public void SetValue(T value)
        {
            if (Equals(value, Value)) return;
            
            Value = value;
            
            OnValueChanged.Invoke(Value);
        }
    }
}
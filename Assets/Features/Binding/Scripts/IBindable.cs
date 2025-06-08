using System;
using System.Collections.Generic;

namespace Features.Binding.Scripts
{

    public interface IBindable<T> 
    {
        public event Action<T> OnValueChanged;
        public T Value { get; }
        public void SetValue(T value);
    }
}
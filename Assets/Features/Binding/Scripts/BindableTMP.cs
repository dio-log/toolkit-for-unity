using System;
using TMPro;

namespace Features.Binding.Scripts
{
    public class BindableTMP : IBindable<string>
    {
        private TMP_Text _textField;
        
        public event Action<string> OnValueChanged = delegate { };
        public string Value => _textField.text;
        public void SetValue(string value)
        {
            if (Equals(value, Value)) return;
            
            _textField.text = value;
            
            OnValueChanged.Invoke(Value);
        }

        public BindableTMP(TMP_Text textField)
        {
            _textField = textField;
        }
    }
}
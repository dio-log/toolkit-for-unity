using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.Binding.Scripts
{
    public class TextBinder : UIBehaviour
    {
        [SerializeField] private TMP_Text _textField;

        private BindableTMP _bindableTMP;

        private Action _unbindAction;
            
        public void Bind<TSource>(IBindable<TSource> source, BindingType bindingType = BindingType.OneWay, IValueConvertor<TSource, string> convertor = null)
        { 
            var context = new BindingContext<TSource, string>(source, _bindableTMP, bindingType, convertor);
            BindingOperation.Bind(context);

            _unbindAction = () => { BindingOperation.Unbind(context); };
        }

        public void Unbind()
        {
            _unbindAction?.Invoke();
        }
        
        protected override void Awake()
        {
            if(_textField == null) throw new ArgumentNullException(nameof(_textField));
            
            _bindableTMP = new BindableTMP(_textField);
        }
    }
}
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.Binding.Scripts
{
    public class TextBinder : UIBehaviour
    {
        [SerializeField] private TMP_Text _textField;
        [SerializeField] private BindingType _bindingType;
        
        public BindingType BindingType => _bindingType;

        private BindableTMP _bindableTMP;

        private Action _unbindAction;

        public void SetBindingType(BindingType bindingType)
        {
            _bindingType = bindingType;
        }
        
        public void Bind<TSource>(IBindable<TSource> source, IValueConvertor<TSource, string> convertor = null)
        {
            Unbind();
            
            var context = new BindingContext<TSource, string>(source, _bindableTMP, _bindingType, convertor);
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
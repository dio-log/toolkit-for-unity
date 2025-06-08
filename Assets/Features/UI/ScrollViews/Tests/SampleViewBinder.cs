using System;
using Features.Binding.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.UI.ScrollViews.Tests
{
    public class SampleViewBinder : UIBehaviour, IBinder<ReactiveProperty<string>>
    {
        [SerializeField] private TMP_Text _textField;

        private BindableTMP _bindableTMP;

        private Action _unbindAction;
        public void Bind(object source)
        {
            Bind((ReactiveProperty<string>) source);
        }

        public void Bind(ReactiveProperty<string> source)
        {
            var ctx = new BindingContext<string, string>(source, _bindableTMP);
            BindingOperation.Bind(ctx);

            _unbindAction = () => BindingOperation.Unbind(ctx);

            _textField.text = source.Value;
        }


        public void Unbind()
        {
            _unbindAction.Invoke();
        }

        protected override void Awake()
        {
            _bindableTMP = new (_textField);
        }
    }
}
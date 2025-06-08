using Features.Binding.Scripts;
using Features.UI.ScrollViews.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.UI.ScrollViews.Tests
{
    
    //sample
    public class HierarchyViewItem : UIBehaviour, IBinder<ViewNode>
    {
        [SerializeField] private RectTransform _indent;
        [SerializeField] private Button _expandButton;
        [SerializeField] private TMP_Text _textField;
        [SerializeField] private int _indentSize = 30;
        
        private ViewNode _viewNode;

        public void Bind(object source)
        {
            if (source is not ViewNode node) return;
            
            Bind(node);
        }

        public void Bind(ViewNode source)
        {
            _viewNode = source;
            var indent = _indent.sizeDelta;
            indent.x = _viewNode.Depth * _indentSize;
            _indent.sizeDelta = indent;
            _textField.text = source.Id;
        }

        public void Unbind()
        {
            _viewNode = null;
            _textField.text = "";
        }

        private void HandleClicked()
        {
            _viewNode.OnExpanded.Invoke(_viewNode);
        }

        protected override void Awake()
        {
            _expandButton.onClick.AddListener(HandleClicked);
        }

        protected override void OnDestroy()
        {
            _expandButton.onClick.RemoveListener(HandleClicked);
        }
    }
}
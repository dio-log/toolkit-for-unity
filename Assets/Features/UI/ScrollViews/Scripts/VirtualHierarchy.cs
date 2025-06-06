using System.Collections.Generic;
using System.Linq;
using Features.Binding.Scripts;
using Features.Tree.Scripts;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.UI.ScrollViews.Scripts
{
    public class VirtualHierarchy : UIBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private GameObject _item;
        [SerializeField] private int _poolSize = 20;

        private bool _isInitialized = false;
        //auto pooling
        private RectTransform _content;
        private float _itemHeight;
        
        private List<ViewNode> _rootNodes = new();// 전체 트리 루트 노드들
        private List<ViewNode> _visibleNodes = new();// 보이는 노드만 평면화된 리스트

        private List<KeyValuePair<GameObject, IBinder<ViewNode>>> _pool = new();

        public void Initialize()
        {
            if (_isInitialized) return;
            
            _content = _scrollRect.content;
            _itemHeight = _item.GetComponent<RectTransform>().sizeDelta.y;
            _scrollRect.onValueChanged.AddListener(OnScroll);
            InitializePool();
            UpdateVisibleNodes(); 
            UpdateContentSize();
            UpdateFields(0);
            
            _isInitialized = true;
        }

        protected override void Awake()
        {
            Initialize();
        }

        private void InitializePool()
        {
            for (var i = 0; i < _poolSize; i++)
            {
                var go = Instantiate(_item, _content); 
                var bindable = go.GetComponent<IBinder<ViewNode>>();

                var kvp = new KeyValuePair<GameObject, IBinder<ViewNode>>(go, bindable);
                
                _pool.Add(kvp);
            }
        }
        
        private void UpdateContentSize()
        {
            _content.sizeDelta = new Vector2(_content.sizeDelta.x, _visibleNodes.Count * _itemHeight);
        }
        
        private void OnScroll(Vector2 scrollPos)
        {
            var startIndex = GetCurrentStartIndex();
            UpdateFields(startIndex);
        }

        private void UpdateVisibleNodes()
        {
            _visibleNodes.Clear();
            
            foreach (var root in _rootNodes)
            {
                FlattenNode(root, _visibleNodes);
            }
        }

        private void FlattenNode(ViewNode node, List<ViewNode> visible)
        {
            visible.Add(node);
            if (node.IsExpanded)
            {
                foreach (var child in node.Children)
                {
                    FlattenNode(child, visible);
                }
            }
        }   

        private void HandleExpanded(ViewNode sender)
        {
            sender.IsExpanded = !sender.IsExpanded;
            UpdateVisibleNodes(); // 폴딩 후 전체 재평면화
            UpdateContentSize();
            UpdateFields(GetCurrentStartIndex());
        }
       

        private void UpdateFields(int startIndex)
        {
            for (var i = 0; i < _pool.Count; i++)
            {
                var dataIndex = startIndex + i;
                var kvp = _pool[i];

                if (dataIndex >= 0 && dataIndex < _visibleNodes.Count)
                {
                    kvp.Key.SetActive(true);
                    var node = _visibleNodes[dataIndex];
                    kvp.Key.transform.localPosition = new Vector3(0, -dataIndex * _itemHeight, 0);
                    kvp.Value.Bind(node);
                }
                else
                {
                    kvp.Key.SetActive(false);
                }
            }
        }
        private int GetCurrentStartIndex()
        {
            var scrollY = _content.anchoredPosition.y;
            var maxIndex = Mathf.Max(0, _visibleNodes.Count - _poolSize);
            var startIndex = Mathf.FloorToInt(scrollY / _itemHeight);
            return Mathf.Clamp(startIndex, 0, maxIndex);
        }

        public void RemoveNode(string id)
        {
            var node = NodeUtil.FindDown(_rootNodes, n => n.Children.ToList(), n => n.Id == id);

            if (node == null) return;
            
            node.SetParent(null);
            NodeUtil.ForEachDown(node, n=> n.Children.ToList(), n=> n.OnExpanded -= HandleExpanded);

            UpdateVisibleNodes();
            UpdateContentSize();
            UpdateFields(GetCurrentStartIndex());
        }
        
        
        public void InsertNode(ViewNode newNode, string parentId = null)
        {
            if (newNode == null) return;
            
            NodeUtil.ForEachDown(newNode, n=> n.Children.ToList(), n=> n.OnExpanded += HandleExpanded);

            if (parentId == null)
            {
                _rootNodes.Add(newNode);
            }
            else
            {
                var parentNode = FindNodeById(parentId);

                if (parentNode == null)
                {
                    _rootNodes.Add(newNode);
                }
                else
                {
                    newNode.SetParent(parentNode);
                }
            }

            UpdateVisibleNodes();
            UpdateContentSize();
            UpdateFields(GetCurrentStartIndex());
        }

        private ViewNode FindNodeById(string id)
        {
            return NodeUtil.FindDown(_rootNodes, n => n.Children.ToList(), n => n.Id == id);
        }   


        private void ValidatePrefab()
        {
            object component = _item.GetComponent<IBinder<ViewNode>>();
            
            Assert.IsNotNull(component);
            
            component = _item.GetComponent<RectTransform>();
            
            Assert.IsNotNull(component);
            
        }
        protected override void OnValidate()
        {
            ValidatePrefab();
        }


        protected override void OnDestroy()
        {
            _scrollRect.onValueChanged.RemoveListener(OnScroll);
        }
    }
}


using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Features.Binding.Scripts;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Features.UI.ScrollViews.Scripts
{
    
    public class VirtualScrollView : UIBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private GameObject _itemPrefab; // HierarchyField 프리팹
        [SerializeField] private int _poolSize = 20;

        private bool _isInitialized = false;
        private RectTransform _content; // Content 오브젝트
        private Dictionary<GameObject, IBinder> _itemPool = new();
        private List<object> _dataSource = new ();
        private int _lastStartIndex = -1;
        private float _itemHeight;

        public void Initialize()
        {
            if (_isInitialized) return;
            _isInitialized = true;
            
            _content = _scrollRect.content;
            _itemHeight = _itemPrefab.GetComponent<RectTransform>().sizeDelta.y;
            
            InitializePool();
            UpdateContentSize();
            _scrollRect.onValueChanged.AddListener(OnScroll);
            UpdateFields(0); // 초기 설정
        }

        private void InitializePool()
        {
            for (int i = 0; i < _poolSize; i++)
            {
                var go = Instantiate(_itemPrefab, _content);
                var binder = go.GetComponent<IBinder>();
                _itemPool.Add(go, binder);
            }
        }

        private void UpdateContentSize()
        {
            var contentHeight = _dataSource.Count * _itemHeight; 
            _content.sizeDelta = new Vector2(_content.sizeDelta.x, contentHeight); 
        }

        private void OnScroll(Vector2 scrollPos)
        {
            var scrollY = _content.anchoredPosition.y; // Content의 현재 Y 위치
            var startIndex = Mathf.FloorToInt(scrollY / _itemHeight); // 시작 데이터 인덱스
            startIndex = Mathf.Clamp(startIndex, 0, _dataSource.Count - _poolSize);
            UpdateFields(startIndex);
        }

        private void UpdateFields(int startIndex)
        {
            var items = _itemPool.Keys.ToList();
            var binders = _itemPool.Values.ToList();
            if (_lastStartIndex == -1 || Mathf.Abs(startIndex - _lastStartIndex) >= 1)
            {
                for (int i = 0; i < _itemPool.Count; i++)
                {
                    int dataIndex = startIndex + i;
                    if (dataIndex >= _dataSource.Count) break;

                    var item = items[i];
                    item.transform.localPosition = new Vector3(0, -dataIndex * _itemHeight, 0);
                    binders[i].Bind(_dataSource[dataIndex]);
                }
                
                _lastStartIndex = startIndex;
            }
        }
        
        public void InsertData(int insertIndex, object data)
        {
            if (insertIndex >= 0 && insertIndex <= _dataSource.Count)
            {
                _dataSource.Insert(insertIndex, data); // 중간에 데이터 삽입

                // Content 크기 갱신
                UpdateContentSize();

                // 현재 스크롤 위치 유지하면서 풀링 갱신
                var currentScrollY = _content.anchoredPosition.y;
                var startIndex = Mathf.FloorToInt(currentScrollY / _itemHeight);
                var maxStartIndex = Mathf.Max(0, _dataSource.Count - _poolSize);
                startIndex = Mathf.Clamp(startIndex, 0, maxStartIndex);
                // startIndex = Mathf.Clamp(startIndex, 0, _dataSource.Count - _poolSize);
                _lastStartIndex = -1; // 강제 갱신 유도
                UpdateFields(startIndex);
            }
            
        }

        protected override void OnValidate()
        {
            object component = _itemPrefab.GetComponent<IBinder>();
            
            Assert.IsNotNull(component);
        }

        protected override void Awake()
        {
            Initialize();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Features.Binding.Scripts;
using Features.UI.ScrollViews.Scripts;
using UnityEngine;
using UnityEngine.UIElements;

namespace Features.UI.ScrollViews.Tests
{
    public class VirtualScrollViewTester : MonoBehaviour
    {
        [SerializeField] private VirtualScrollView _scrollView;

        private IEnumerable<ReactiveProperty<string>> GetData()
        {
            for (int i = 0; i < 50; i++)
            {
                yield return new ReactiveProperty<string>($"value {i}");
            }
        }
        
        private void Start()
        {
            var data = GetData().ToList();
            
            for (var i = 0; i < data.Count; i++)
            {
                _scrollView.InsertData(i, data[i]);
            }
        }
    }
}
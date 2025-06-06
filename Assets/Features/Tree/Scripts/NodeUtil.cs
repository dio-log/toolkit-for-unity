using System;
using System.Collections.Generic;
using System.Linq;

namespace Features.Tree.Scripts
{
    public static class NodeUtil
    {
        public static void ForEachDown<TSource>(TSource node, Func<TSource, List<TSource>> childrenSelector, Action<TSource> action, ITraversalMethod method = null)
        {
            method ??= Traversal.BFS;
            var children = method.Traverse(node, childrenSelector);

            foreach (var source in children)
            {
                action.Invoke(source);   
            }
        }

        public static void UntilDown<TSource>(TSource node, Func<TSource, List<TSource>> childrenSelector, Func<TSource, bool> shouldBreak, ITraversalMethod method = null)
        {
            method ??= Traversal.BFS;
            var children = method.Traverse(node, childrenSelector);

            foreach (var source in children)
            {
                if (shouldBreak(source)) return;
            }
        }

        public static void UntilDown<TSource>(List<TSource> nodes, Func<TSource, List<TSource>> childrenSelector,
            Func<TSource, bool> shouldBreak, ITraversalMethod method = null)
        {
            method ??= Traversal.BFS;
            var children = nodes.SelectMany(node => method.Traverse(node, childrenSelector));
            
            foreach (var source in children) 
            {
                if (shouldBreak(source)) return;
            }
        }
        
        public static TSource FindDown<TSource>(List<TSource> nodes, Func<TSource, List<TSource>> childrenSelector,
            Func<TSource, bool> finder, ITraversalMethod method = null)
        {
            method ??= Traversal.BFS;
            var children = nodes.SelectMany(node => method.Traverse(node, childrenSelector));
            
            foreach (var source in children)
            {
                if (finder(source)) return source;
            }

            return default;
        }

        public static List<TResult> SelectDown<TSource, TResult>(TSource node, Func<TSource, List<TSource>> childrenSelector, Func<TSource, TResult> selector, ITraversalMethod method)
        {
            var results = new List<TResult>();
            var children = method.Traverse(node, childrenSelector);
            
            foreach (var source in children)
            {
                results.Add(selector.Invoke(source));
            }
            
            return results;
        }

        public static List<TSource> WhereDown<TSource>(TSource node, Func<TSource, List<TSource>> childrenSelector, Func<TSource, bool> isMatch, ITraversalMethod method)
        {
            var results = new List<TSource>();
            var children = method.Traverse(node, childrenSelector);
            
            foreach (var source in children)
            {
                if(isMatch.Invoke(source)) results.Add(source);
            }
            
            return results;
        }
   
        
    }
}
using System;
using System.Collections.Generic;

namespace Features.Tree.Scripts
{
    public class BFS : ITraversalMethod 
    {
        public List<T> Traverse<T>(T source, Func<T, List<T>> childrenSelector)
        {
            var result = new List<T>();
            
            if (source == null) return result;

            var queue = new Queue<T>();
            
            queue.Enqueue(source);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                result.Add(current);

                foreach (var child in childrenSelector(current))
                {
                    queue.Enqueue(child);
                }
            }

            return result;            
        }
    }
}
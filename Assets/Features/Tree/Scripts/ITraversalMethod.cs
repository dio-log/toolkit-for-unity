using System;
using System.Collections.Generic;

namespace Features.Tree.Scripts
{
    public interface ITraversalMethod
    {
        List<T> Traverse<T>(T source, Func<T, List<T>> childrenSelector);
    }
}
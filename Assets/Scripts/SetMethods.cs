using System;
using System.Collections.Generic;

public class SetMethods
{
    /// <summary>
    /// Determines whether all elements of a sequence satisfy a condition.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static bool All<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        foreach (TSource item in source)
        {
            if (!predicate(item))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Determines whether any element of a sequence satisfies a condition.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static bool Any<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        foreach (TSource item in source)
        {
            if (predicate(item))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Determines whether a sequence contains a specified element by using the default equality comparer.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static bool Contains<TSource>(IEnumerable<TSource> source, TSource item)
    {
        foreach (TSource src in source)
        {
            if (EqualityComparer<TSource>.Default.Equals(src, item))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Determines whether a sequence contains a specified element by using a specified IEqualityComparer<T>.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="item"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static bool Contains<TSource>(IEnumerable<TSource> source, TSource item, IEqualityComparer<TSource> comparer)
    {
        comparer ??= EqualityComparer<TSource>.Default;

        foreach (TSource src in source)
        {
            if (comparer.Equals(src, item))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Returns distinct elements from a sequence by using the default equality comparer to compare values.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Distinct<TSource>(IEnumerable<TSource> source)
    {
        HashSet<TSource> alreadySeen = new HashSet<TSource>();

        foreach (TSource src in source)
        {
            if (alreadySeen.Add(src))
                yield return src;
        }
    }

    /// <summary>
    /// Returns distinct elements from a sequence by using a specified IEqualityComparer<T> to compare values.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Distinct<TSource>(IEnumerable<TSource> source,
        IEqualityComparer<TSource> comparer)
    {
        comparer ??= EqualityComparer<TSource>.Default;

        HashSet<TSource> alreadySeen = new HashSet<TSource>(comparer);

        foreach (TSource src in source)
        {
            if (alreadySeen.Add(src))
                yield return src;
        }
    }

    /// <summary>
    /// Returns the element at a specified index in a sequence.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static TSource ElementAt<TSource>(IEnumerable<TSource> source, int index)
    {
        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index));

        if (source is IList<TSource> list)
        {
            if (index >= list.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            return list[index];
        }

        int i = 0;
        foreach (TSource item in source)
        {
            if (i == index)
                return item;

            i++;
        }

        throw new ArgumentOutOfRangeException(nameof(index));
    }

    /// <summary>
    /// Produces the set difference of two sequences by using the default equality comparer to compare values.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source1"></param>
    /// <param name="source2"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Except<TSource>(IEnumerable<TSource> source1, IEnumerable<TSource> source2)
    {
        HashSet<TSource> excludeSet = source2 != null ? new HashSet<TSource>(source2) : new HashSet<TSource>();

        HashSet<TSource> alreadySeen = new HashSet<TSource>();

        foreach (TSource src in source1)
        {
            if (!excludeSet.Contains(src) && alreadySeen.Add(src))
                yield return src;
        }
    }

    /// <summary>
    /// Produces the set difference of two sequences by using the specified IEqualityComparer<T> to compare values.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source1"></param>
    /// <param name="source2"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Except<TSource>(IEnumerable<TSource> source1, IEnumerable<TSource> source2,
        IEqualityComparer<TSource> comparer)
    {
        comparer ??= EqualityComparer<TSource>.Default;

        HashSet<TSource> excludeSet =
            source2 != null ? new HashSet<TSource>(source2, comparer) : new HashSet<TSource>(comparer);

        HashSet<TSource> alreadySeen = new HashSet<TSource>(comparer);

        foreach (TSource src in source1)
        {
            if (!excludeSet.Contains(src) && alreadySeen.Add(src))
                yield return src;
        }
    }

    /// <summary>
    /// Returns the first element in a sequence that satisfies a specified condition.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static TSource First<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        foreach (TSource src in source)
        {
            if (predicate(src))
                return src;
        }
        
        throw new InvalidOperationException("No element satisfies the condition");

    }

    /// <summary>
    /// Returns the last element of a sequence that satisfies a specified condition.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static TSource Last<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        TSource last = default(TSource);
        bool found = false;

        foreach (TSource src in source)
        {
            if (predicate(src))
            {
                last = src;
                found = true;
            }
        }

        if (!found)
            throw new InvalidOperationException("No element satisfies the condition");

        return last;
    }

    /// <summary>
    /// Produces the set intersection of two sequences by using the default equality comparer to compare values.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source1"></param>
    /// <param name="source2"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Intersect<TSource>(IEnumerable<TSource> source1, IEnumerable<TSource> source2)
    {
        HashSet<TSource> includeSet = source2 != null ? new HashSet<TSource>(source2) : new HashSet<TSource>();

        HashSet<TSource> alreadySeen = new HashSet<TSource>();

        foreach (TSource src in source1)
        {
            if (includeSet.Contains(src) && alreadySeen.Add(src))
                yield return src;
        }
    }

    /// <summary>
    /// Produces the set intersection of two sequences by using the specified IEqualityComparer<T> to compare values.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source1"></param>
    /// <param name="source2"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Intersect<TSource>(IEnumerable<TSource> source1, IEnumerable<TSource> source2,
        IEqualityComparer<TSource> comparer)
    {
        comparer ??= EqualityComparer<TSource>.Default;

        HashSet<TSource> includeSet =
            source2 != null ? new HashSet<TSource>(source2, comparer) : new HashSet<TSource>(comparer);

        HashSet<TSource> alreadySeen = new HashSet<TSource>(comparer);

        foreach (TSource src in source1)
        {
            if (includeSet.Contains(src) && alreadySeen.Add(src))
                yield return src;
        }
    }

    /// <summary>
    /// Returns a number that represents how many elements in the specified sequence satisfy a condition.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static int Count<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        int count = 0;
        foreach (TSource item in source)
        {
            if (predicate(item))
                count++;
        }

        return count;
    }

    /// <summary>
    /// Determines whether two sequences are equal by comparing their elements by using a specified IEqualityComparer<T>.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source1"></param>
    /// <param name="source2"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static bool SequenceEqual<TSource>(IEnumerable<TSource> source1, IEnumerable<TSource> source2,
        IEqualityComparer<TSource> comparer)
    {
        comparer ??= EqualityComparer<TSource>.Default;

        using IEnumerator<TSource> src1 = source1.GetEnumerator();
        using IEnumerator<TSource> src2 = source2.GetEnumerator();

        while (src1.MoveNext())
        {
            if (!src2.MoveNext() || !comparer.Equals(src1.Current, src2.Current))
                return false;
        }

        return !src2.MoveNext();
    }

    /// <summary>
    /// Returns the only element of a sequence that satisfies a specified condition, and throws an exception if more than one such element exists.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static TSource Single<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        TSource result = default(TSource);
        bool found = false;

        foreach (TSource item in source)
        {
            if (predicate(item))
            {
                if (found)
                    throw new InvalidOperationException("More than one element satisfies the condition");

                result = item;
                found = true;
            }
        }

        if (!found)
            throw new InvalidOperationException("No element satisfies the condition");

        return result;
    }

    /// <summary>
    /// Bypasses elements in a sequence as long as a specified condition is true and then returns the remaining elements.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> SkipWhile<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        bool yielding = false;

        foreach (TSource item in source)
        {
            if (!yielding && !predicate(item))
                yielding = true;

            if (yielding)
                yield return item;
        }
    }

    /// <summary>
    /// Produces the set union of two sequences by using the default equality comparer.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source1"></param>
    /// <param name="source2"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Union<TSource>(IEnumerable<TSource> source1, IEnumerable<TSource> source2)
    {
        HashSet<TSource> alreadySeen = new HashSet<TSource>();

        foreach (TSource item in source1)
        {
            if (alreadySeen.Add(item))
                yield return item;
        }

        foreach (TSource item in source2)
        {
            if (alreadySeen.Add(item))
                yield return item;
        }
    }

    /// <summary>
    /// Produces the set union of two sequences by using a specified IEqualityComparer<T>.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source1"></param>
    /// <param name="source2"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Union<TSource>(IEnumerable<TSource> source1, IEnumerable<TSource> source2,
        IEqualityComparer<TSource> comparer)
    {
        comparer ??= EqualityComparer<TSource>.Default;

        HashSet<TSource> alreadySeen = new HashSet<TSource>(comparer);

        foreach (TSource item in source1)
        {
            if (alreadySeen.Add(item))
                yield return item;
        }

        foreach (TSource item in source2)
        {
            if (alreadySeen.Add(item))
                yield return item;
        }
    }
    
    /// <summary>
    /// Filters a sequence of values based on a predicate. Each element's index is used in the logic of the predicate function.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Where<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        foreach (TSource item in source)
        {
            if (predicate(item))
                yield return item;
        }
    }
    
}

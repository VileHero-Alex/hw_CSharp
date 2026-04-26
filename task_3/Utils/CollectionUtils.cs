using System;
using System.Collections.Generic;

public static class CollectionUtils {
    public static List<T> Distinct<T>(List<T> source) {
        var seen = new HashSet<T>();
        var result = new List<T>();
        foreach (var item in source) {
            if (seen.Add(item)) {
                result.Add(item);
            }
        }
        return result;
    }

    public static Dictionary<TKey, List<TValue>> GroupBy<TValue, TKey>(
        List<TValue> source,
        Func<TValue, TKey> keySelector) where TKey : notnull {
        var result = new Dictionary<TKey, List<TValue>>();
        foreach (var item in source) {
            var key = keySelector(item);
            if (!result.ContainsKey(key)) {
                result[key] = new List<TValue>();
            }
            result[key].Add(item);
        }
        return result;
    }

    public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
        Dictionary<TKey, TValue> first,
        Dictionary<TKey, TValue> second,
        Func<TValue, TValue, TValue> conflictResolver) where TKey : notnull {
        var result = new Dictionary<TKey, TValue>(first);
        foreach (var kv in second) {
            if (result.ContainsKey(kv.Key)) {
                result[kv.Key] = conflictResolver(result[kv.Key], kv.Value);
            } else {
                result[kv.Key] = kv.Value;
            }
        }
        return result;
    }

    public static T MaxBy<T, TKey>(List<T> source, Func<T, TKey> selector)
        where TKey : IComparable<TKey> {
        if (source.Count == 0) {
            throw new InvalidOperationException("Empty collection");
        }
        T maxItem = source[0];
        TKey maxValue = selector(maxItem);
        for (int i = 1; i < source.Count; i++) {
            var currentItem = source[i];
            var currentValue = selector(currentItem);
            if (currentValue.CompareTo(maxValue) > 0) {
                maxItem = currentItem;
                maxValue = currentValue;
            }
        }
        return maxItem;
    }
}

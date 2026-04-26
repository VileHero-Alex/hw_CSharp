using System;
using System.Collections.Generic;
using System.Linq;

public class Repository<T> where T : IEntity {
    private readonly Dictionary<int, T> Storage_ = new();

    public int Count => Storage_.Count;

    public void Add(T item) {
        if (Storage_.ContainsKey(item.Id)) {
            throw new InvalidOperationException($"Id={item.Id} already exists");
        }
        Storage_[item.Id] = item;
    }

    public bool Remove(int id) {
        return Storage_.Remove(id);
    }

    public T? GetById(int id) {
        Storage_.TryGetValue(id, out T? value);
        return value;
    }

    public IReadOnlyList<T> GetAll() {
        return Storage_.Values.ToList();
    }

    public IReadOnlyList<T> Find(Predicate<T> predicate) {
        var result = new List<T>();
        foreach (var item in Storage_.Values) {
            if (predicate(item)) {
                result.Add(item);
            }
        }
        return result;
    }
}

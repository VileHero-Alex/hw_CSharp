using System;
using System.Collections.Generic;

class Program {
    static void Main(string[] args) {
        Console.WriteLine("=== HW3 ===\n");

        Console.WriteLine("--- Repository<T> ---");
        var productRepo = new Repository<Product>();
        productRepo.Add(new Product(1, "Laptop", 999.99m));
        productRepo.Add(new Product(2, "Mouse", 25.50m));
        productRepo.Add(new Product(3, "Keyboard", 150.00m));
        productRepo.Add(new Product(4, "Monitor", 500.00m));
        productRepo.Add(new Product(5, "SSD 1TB", 2000.00m));

        Console.WriteLine($"Count: {productRepo.Count}");
        Console.WriteLine($"GetById(3): {productRepo.GetById(3)}");
        Console.WriteLine($"Find Price>1000: {string.Join(", ", productRepo.Find(p => p.Price > 1000))}");
        Console.WriteLine($"Remove(2): {productRepo.Remove(2)}");
        Console.WriteLine($"Count after remove: {productRepo.Count}");

        try {
            productRepo.Add(new Product(1, "Duplicate", 100m));
        } catch (InvalidOperationException ex) {
            Console.WriteLine($"Add duplicate exception: {ex.Message}");
        }

        var userRepo = new Repository<User>();
        userRepo.Add(new User(1, "alice", "alice@mail.com"));
        userRepo.Add(new User(2, "bob", "bob@mail.com"));
        userRepo.Add(new User(3, "charlie", "charlie@mail.com"));
        Console.WriteLine($"\nUserRepo Count: {userRepo.Count}");

        Console.WriteLine("\n--- CollectionUtils ---");
        var nums = new List<int> { 1, 2, 2, 3, 3, 3, 4, 5, 5 };
        Console.WriteLine($"Distinct ints: [{string.Join(", ", CollectionUtils.Distinct(nums))}]");

        var words = new List<string> { "apple", "banana", "apple", "cherry", "banana" };
        Console.WriteLine($"Distinct strings: [{string.Join(", ", CollectionUtils.Distinct(words))}]");

        var animals = new List<string> { "cat", "dog", "elephant", "ant", "bee" };
        var grouped = CollectionUtils.GroupBy(animals, w => w.Length);
        Console.WriteLine("GroupBy length:");
        foreach (var kv in grouped) {
            Console.WriteLine($"  {kv.Key}: [{string.Join(", ", kv.Value)}]");
        }

        var dict1 = new Dictionary<string, int> { ["apple"] = 3, ["banana"] = 2 };
        var dict2 = new Dictionary<string, int> { ["banana"] = 3, ["grape"] = 4 };
        var merged = CollectionUtils.Merge(dict1, dict2, (v1, v2) => v1 + v2);
        Console.WriteLine("Merge with sum:");
        foreach (var kv in merged) {
            Console.WriteLine($"  {kv.Key}: {kv.Value}");
        }

        var products = new List<Product> {
            new Product(1, "Laptop", 999.99m),
            new Product(2, "Phone", 599.99m),
            new Product(3, "Monitor", 1500.00m),
            new Product(4, "SSD", 2000.00m)
        };
        var maxProduct = CollectionUtils.MaxBy(products, p => p.Price);
        Console.WriteLine($"MaxBy Price: {maxProduct}");

        try {
            CollectionUtils.MaxBy(new List<Product>(), p => p.Price);
        } catch (InvalidOperationException ex) {
            Console.WriteLine($"MaxBy empty exception: {ex.Message}");
        }

        Console.WriteLine("\n=== Done ===");
    }
}

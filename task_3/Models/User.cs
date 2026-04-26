public class User : IEntity {
    public int Id { get; }
    public string Username { get; }
    public string Email { get; }

    public User(int id, string username, string email) {
        Id = id;
        Username = username;
        Email = email;
    }

    public override string ToString() {
        return $"User {{ Id={Id}, Username='{Username}', Email='{Email}' }}";
    }
}

using LeaderboardAPI.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace LeaderboardAPI.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(ILeaderboardDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UsersCollectionName);




            var options = new CreateIndexOptions() { Unique = true, Name = "Unique Username" };
            var field = new StringFieldDefinition<User>("UserName");
            var indexDefinition = new IndexKeysDefinitionBuilder<User>().Ascending(field);

            var indexModel = new CreateIndexModel<User>(indexDefinition, options);
            _users.Indexes.CreateOne(indexModel);


        }

        public List<User> Get() =>
            _users.Find(user => true).ToList();

        public User Get(string id) =>
            _users.Find<User>(user => user.Id == id).FirstOrDefault();

        public User GetByUsername(string username) =>
            _users.Find<User>(user => user.UserName == username).FirstOrDefault();

        public User Create(User user)
        {
            _users.InsertOne(user);
            return user;
        }

        public void Update(string id, User userIn) =>
            _users.ReplaceOne(user => user.Id == id, userIn);

        public void Remove(User userIn) =>
            _users.DeleteOne(user => user.Id == userIn.Id);

        public void Remove(string id) =>
            _users.DeleteOne(user => user.Id == id);
    }
}

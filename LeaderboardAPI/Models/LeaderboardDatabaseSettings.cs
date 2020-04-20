namespace LeaderboardAPI.Models
{
    public class LeaderboardDatabaseSettings : ILeaderboardDatabaseSettings
    {
        public string UsersCollectionName { get; set; }
        public string ScoresCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ILeaderboardDatabaseSettings
    {
        string UsersCollectionName { get; set; }
        string ScoresCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}

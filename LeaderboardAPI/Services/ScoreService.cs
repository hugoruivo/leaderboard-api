using LeaderboardAPI.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace LeaderboardAPI.Services
{
    public class ScorePage
    {
        public long _totalScores { get; set; }
        public List<Score> _items { get; set; }

        public ScorePage(long totalScores, List<Score> items)
        {
            _totalScores = totalScores;
            _items = items;
        }
    }
    public class ScoreService
    {
        const int ITEMS_PER_PAGE = 2;
        private readonly IMongoCollection<Score> _scores;

        public ScoreService(ILeaderboardDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _scores = database.GetCollection<Score>(settings.ScoresCollectionName);

            var options = new CreateIndexOptions() { Unique = true, Name = "Unique Username" };
            var field = new StringFieldDefinition<Score>("UserName");
            var indexDefinition = new IndexKeysDefinitionBuilder<Score>().Ascending(field);

            var indexModel = new CreateIndexModel<Score>(indexDefinition, options);
            _scores.Indexes.CreateOne(indexModel);
        }

        public List<Score> Get() =>
            _scores.Find(score => true).ToList();

        public Score Get(string id) =>
            _scores.Find<Score>(score => score.Id == id).FirstOrDefault();

        public ScorePage Get(int page, int numItems)
        {
            if (page < 1)
            {
                page = 1;
            }
            if (numItems < 1)
            {
                numItems = ITEMS_PER_PAGE;
            }
            //Page starts at 1 but we retrieve from a zero based, so subtract
            page -= 1;

            var scoresQuery = _scores.Find(score => true).SortByDescending(score => score.UserScore);
            long totalScores = scoresQuery.CountDocuments();
            List<Score> items = scoresQuery.Skip(page * numItems).Limit(numItems).ToList();
            return new ScorePage(totalScores, items);
        }

        public Score GetByUsername(string username) =>
            _scores.Find<Score>(score => score.UserName == username).FirstOrDefault();

        public Score Create(Score score)
        {
            _scores.InsertOne(score);
            return score;
        }

        public void Update(string id, Score scoreIn) =>
            _scores.ReplaceOne(score => score.Id == id, scoreIn);

        public void Remove(Score scoreIn) =>
            _scores.DeleteOne(score => score.Id == scoreIn.Id);

        public void Remove(string id) =>
            _scores.DeleteOne(score => score.Id == id);
    }
}

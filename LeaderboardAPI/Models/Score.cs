using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LeaderboardAPI.Models
{
    public class Score
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /*
         * Will save username
         * but maybe it would make sense of saving the user id instead.
        */
        [BsonRequired]
        public string UserName { get; set; }

        [BsonRequired]
        public int UserScore { get; set; }
    }
}

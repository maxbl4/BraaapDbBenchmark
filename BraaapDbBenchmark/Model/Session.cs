using System;

namespace BraaapDbBenchmark.Model
{
    public class Session
    {
        [LiteDB.BsonId]
        [MongoDB.Bson.Serialization.Attributes.BsonId]
        public Guid SessionId { get; set; }
        public string Name { get; set; }
    }
}
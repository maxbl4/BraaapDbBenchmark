using System;

namespace BraaapDbBenchmark.Model
{
    public class Checkpoint
    {
        [LiteDB.BsonId]
        [MongoDB.Bson.Serialization.Attributes.BsonId]
        public Guid CheckpointId { get; set; }
        public Guid? SessionId { get; set; }
        public Guid? RiderId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
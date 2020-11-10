using System;

namespace BraaapDbBenchmark.Model
{
    public class RiderSessionResult
    {
        [LiteDB.BsonId]
        [MongoDB.Bson.Serialization.Attributes.BsonId]
        public Guid RiderSessionResultId { get; set; }
        public Guid? SessionId { get; set; }
        public Guid? RiderId { get; set; }
        public string RiderName { get; set; }
        public int Position { get; set; }
        public string RiderNumber { get; set; }
    }
}
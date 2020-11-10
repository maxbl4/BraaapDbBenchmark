using System;
using System.Collections.Generic;

namespace BraaapDbBenchmark.Model
{
    public class Rider
    {
        [LiteDB.BsonId]
        [MongoDB.Bson.Serialization.Attributes.BsonId]
        public Guid RiderId { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
    }
}
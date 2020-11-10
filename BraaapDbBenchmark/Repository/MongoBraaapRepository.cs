using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BraaapDbBenchmark.Model;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace BraaapDbBenchmark.Repository
{
    public class MongoBraaapRepository : IBraaapRepository
    {
        private readonly string _connectionString;
        private readonly MongoClient _client;
        
        private IMongoDatabase Db => _client.GetDatabase("benchmark");

        public MongoBraaapRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Mongo");
            _client = new MongoClient(_connectionString);
        }

        public async Task Initialize(bool truncateDatabase)
        {
            if (truncateDatabase)
                await _client.DropDatabaseAsync("benchmark");
            await Db.GetCollection<Session>(nameof(Session)).Indexes.CreateOneAsync(new CreateIndexModel<Session>(Builders<Session>.IndexKeys.Ascending(x => x.Name)));
        }
        
        public async Task<Session> AddSession(Session session)
        {
            if (session.SessionId == Guid.Empty)
                session.SessionId = Guid.NewGuid();
            await Db.GetCollection<Session>(nameof(Session)).InsertOneAsync(session);
            return session;
        }
        
        public async Task<RiderSessionResult> AddRiderSessionResult(RiderSessionResult riderSessionResult, Session session, Rider rider)
        {
            if (riderSessionResult.RiderSessionResultId == Guid.Empty)
                riderSessionResult.RiderSessionResultId = Guid.NewGuid();
            riderSessionResult.SessionId = session?.SessionId;
            riderSessionResult.RiderId = rider?.RiderId;
            riderSessionResult.RiderName = rider?.Name;
            riderSessionResult.RiderNumber = rider?.Number;
            await Db.GetCollection<RiderSessionResult>(nameof(RiderSessionResult)).InsertOneAsync(riderSessionResult);
            return riderSessionResult;
        }

        public async Task<Checkpoint> AddCheckpoint(Checkpoint checkpoint, Session session, Rider rider)
        {
            if (checkpoint.CheckpointId == Guid.Empty)
                checkpoint.CheckpointId = Guid.NewGuid();
            checkpoint.SessionId = session?.SessionId;
            checkpoint.RiderId = rider?.RiderId;
            await Db.GetCollection<Checkpoint>(nameof(Checkpoint)).InsertOneAsync(checkpoint);
            return checkpoint;
        }

        public async Task<Rider> AddRider(Rider rider)
        {
            if (rider.RiderId == Guid.Empty)
                rider.RiderId = Guid.NewGuid();
            await Db.GetCollection<Rider>(nameof(Rider)).InsertOneAsync(rider);
            return rider;
        }
        
        public async Task<Session> GetSession(Guid sessionId)
        {
            var results = await Db.GetCollection<Session>(nameof(Session)).FindAsync(x => x.SessionId == sessionId);
            return await results.FirstOrDefaultAsync();
        }
        
        public async Task<Session> GetSessionByName(string name)
        {
            var results = await Db.GetCollection<Session>(nameof(Session)).FindAsync(x => x.Name == name);
            return await results.FirstOrDefaultAsync();
        }
        
        public async Task<List<(Session, Rider, RiderSessionResult)>> GetSessionResults(Guid sessionId)
        {
            var session = await GetSession(sessionId);
            var results = await (await Db.GetCollection<RiderSessionResult>(nameof(RiderSessionResult)).FindAsync(x => x.SessionId == sessionId)).ToListAsync();
            var riders = new List<(Rider, RiderSessionResult)>();
            foreach (var result in results)
            {
                var res = await (await Db.GetCollection<Rider>(nameof(Rider)).FindAsync(x => x.RiderId == result.RiderId)).FirstOrDefaultAsync();
                riders.Add((res, result));
            }
                
            return riders.Select(x => (session, x.Item1, x.Item2)).ToList();
        }
    }
}
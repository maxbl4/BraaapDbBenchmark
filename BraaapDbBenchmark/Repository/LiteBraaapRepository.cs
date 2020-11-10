using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BraaapDbBenchmark.Model;
using LiteDB;
using Microsoft.Extensions.Configuration;

namespace BraaapDbBenchmark.Repository
{
    public class LiteBraaapRepository : IBraaapRepository
    {
        private readonly string _connectionString;
        private LiteRepository repo;

        public LiteBraaapRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Lite");
        }
        
        public Task Initialize(bool truncateDatabase)
        {
            if (truncateDatabase)
                TruncateDatabase();
            repo = new LiteRepository(_connectionString);
            repo.Database.GetCollection<Session>().EnsureIndex(x => x.SessionId);
            repo.Database.GetCollection<Session>().EnsureIndex(x => x.Name);
            return Task.CompletedTask;
        }

        private void TruncateDatabase()
        {
            var logFileName = Path.GetFileNameWithoutExtension(_connectionString);
            if (logFileName != null)
            {
                logFileName = $"{logFileName}-log.litedb";
                if (File.Exists(logFileName)) File.Delete(logFileName);
            }
            if (File.Exists(_connectionString)) File.Delete(_connectionString);
        }
        
        public Task<Session> AddSession(Session session)
        {
            if (session.SessionId == Guid.Empty)
                session.SessionId = Guid.NewGuid();
            repo.Insert(session);
            return Task.FromResult(session);
        }

        public Task<RiderSessionResult> AddRiderSessionResult(RiderSessionResult riderSessionResult, Session session, Rider rider)
        {
            if (riderSessionResult.RiderSessionResultId == Guid.Empty)
                riderSessionResult.RiderSessionResultId = Guid.NewGuid();
            riderSessionResult.SessionId = session?.SessionId;
            riderSessionResult.RiderId = rider?.RiderId;
            riderSessionResult.RiderName = rider?.Name;
            riderSessionResult.RiderNumber = rider?.Number;
            repo.Insert(riderSessionResult);
            return Task.FromResult(riderSessionResult);
        }

        public Task<Checkpoint> AddCheckpoint(Checkpoint checkpoint, Session session, Rider rider)
        {
            if (checkpoint.CheckpointId == Guid.Empty)
                checkpoint.CheckpointId = Guid.NewGuid();
            checkpoint.SessionId = session?.SessionId;
            checkpoint.RiderId = rider?.RiderId;
            repo.Insert(checkpoint);
            return Task.FromResult(checkpoint);
        }

        public Task<Rider> AddRider(Rider rider)
        {
            if (rider.RiderId == Guid.Empty)
                rider.RiderId = Guid.NewGuid();
            repo.Insert(rider);
            return Task.FromResult(rider);
        }
        
        public Task<Session> GetSession(Guid sessionId)
        {
            return Task.FromResult(repo.FirstOrDefault<Session>(x => x.SessionId == sessionId));
        }
        
        public Task<Session> GetSessionByName(string name)
        {
            return Task.FromResult(repo.FirstOrDefault<Session>(x => x.Name == name));
        }
        
        public Task<List<(Session, Rider, RiderSessionResult)>> GetSessionResults(Guid sessionId)
        {
            var session = GetSession(sessionId).Result;
            var results = repo.Query<RiderSessionResult>().Where(x => x.SessionId == sessionId).ToList();
            var riders = new List<(Rider, RiderSessionResult)>();
            foreach (var result in results)
            {
                var res = repo.FirstOrDefault<Rider>(x => x.RiderId == result.RiderId);
                riders.Add((res, result));
            }
                
            return Task.FromResult(riders.Select(x => (session, x.Item1, x.Item2)).ToList());
        }
    }
}
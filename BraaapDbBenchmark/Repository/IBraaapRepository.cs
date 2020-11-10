using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BraaapDbBenchmark.Model;
using Microsoft.Extensions.Configuration;

namespace BraaapDbBenchmark.Repository
{
    public interface IBraaapRepository
    {
        Task Initialize(bool truncateDatabase);
        Task<Session> AddSession(Session session);
        Task<Session> GetSession(Guid sessionId);
        Task<Session> GetSessionByName(string name);
        Task<List<(Session, Rider, RiderSessionResult)>> GetSessionResults(Guid sessionId);
        
        Task<RiderSessionResult> AddRiderSessionResult(RiderSessionResult riderSessionResult, Session session, Rider rider);
        Task<Checkpoint> AddCheckpoint(Checkpoint checkpoint, Session session, Rider rider);
        Task<Rider> AddRider(Rider rider);
    }

    public static class IBraaapRepositoryExt
    {
        public static async Task<bool> CheckRoundTrip(this IBraaapRepository repo)
        {
            var sessionName = $"Round trip test session {DateTime.UtcNow:u}"; 
            var session = await repo.AddSession(new Session {Name = sessionName});
            var loadedSession = await repo.GetSession(session.SessionId);
            return loadedSession.Name == sessionName;
        }

        public static IEnumerable<IBraaapRepository> GetEngines(string useEngines, IConfiguration configuration)
        {
            return useEngines
                .Split(new[] {',', ';'}, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select<string, IBraaapRepository>(x => x switch
                {
                    "MySql" => new MySqlEfBraaapRepository(configuration),
                    "Lite" => new LiteBraaapRepository(configuration),
                    "Mongo" => new MongoBraaapRepository(configuration),
                    _ => throw new ArgumentException($"{x} is not supported engine")
                });
        }
    }
}
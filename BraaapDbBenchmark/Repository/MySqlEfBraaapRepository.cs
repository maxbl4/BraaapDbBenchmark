using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BraaapDbBenchmark.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BraaapDbBenchmark.Repository
{
    public class MySqlEfBraaapRepository : IBraaapRepository
    {
        private readonly string _connectionString;
        private DataContext ctx;

        public MySqlEfBraaapRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MySql");
            ctx = DataContextFactory.Create(_connectionString);
        }
            
        public async Task Initialize(bool truncateDatabase)
        {
            if (truncateDatabase)
                await ctx.Database.EnsureDeletedAsync();
            await ctx.Database.MigrateAsync();
        }

        public async Task<Session> AddSession(Session session)
        {
            if (session.SessionId == Guid.Empty)
                session.SessionId = Guid.NewGuid();
            ctx.Sessions.Add(session);
            await ctx.SaveChangesAsync();
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
            ctx.RiderSessionResults.Add(riderSessionResult);
            await ctx.SaveChangesAsync();
            return riderSessionResult;
        }

        public async Task<Checkpoint> AddCheckpoint(Checkpoint checkpoint, Session session, Rider rider)
        {
            if (checkpoint.CheckpointId == Guid.Empty)
                checkpoint.CheckpointId = Guid.NewGuid();
            checkpoint.SessionId = session?.SessionId;
            checkpoint.RiderId = rider?.RiderId;
            ctx.Checkpoints.Add(checkpoint);
            await ctx.SaveChangesAsync();
            return checkpoint;
        }

        public async Task<Rider> AddRider(Rider rider)
        {
            if (rider.RiderId == Guid.Empty)
                rider.RiderId = Guid.NewGuid();
            ctx.Riders.Add(rider);
            await ctx.SaveChangesAsync();
            return rider;
        }
        
        public async Task<Session> GetSession(Guid sessionId)
        {
            return await ctx.Sessions.FirstOrDefaultAsync(x => x.SessionId == sessionId);
        }

        public async Task<Session> GetSessionByName(string name)
        {
            return await ctx.Sessions.FirstOrDefaultAsync(x => x.Name == name);
        }
        
        public async Task<List<(Session, Rider, RiderSessionResult)>> GetSessionResults(Guid sessionId)
        {
            var session = await GetSession(sessionId);
            var results = await ctx.RiderSessionResults.Where(x => x.SessionId == sessionId).ToListAsync();
            var riders = new List<(Rider, RiderSessionResult)>();
            foreach (var result in results)
            {
                riders.Add((await ctx.Riders.FirstOrDefaultAsync(x => x.RiderId == result.RiderId), result));
            }
                
            return riders.Select(x => (session, x.Item1, x.Item2)).ToList();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BraaapDbBenchmark.Model;
using BraaapDbBenchmark.Repository;
using Microsoft.Extensions.Configuration;

namespace BraaapDbBenchmark
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = Config.Initialize(args);
            var options = configuration.GetSection(nameof(Options)).Get<Options>();
            var repositories = IBraaapRepositoryExt.GetEngines(options.UseEngines, configuration).ToList();
            foreach (var repo in repositories)
            {
                Console.WriteLine(repo.GetType().Name);
                await repo.Initialize(options.ClearData);

                if (options.InsertData)
                {
                    await InsertData(repo, options);
                }

                if (options.ReadData)
                {
                    await ReadData(repo, options);
                }
            }
            Console.WriteLine("Done");
        }

        private static async Task InsertData(IBraaapRepository repo, Options options)
        {
            List<Rider> riders;
            using (new Swatch($"Riders {options.RiderCount} created"))
            {
                riders = Enumerable.Range(1, options.RiderCount).Select(x => repo.AddRider(new Rider
                {
                    Name = $"Rider {x}",
                    Number = x.ToString(),
                })).Select(x => x.Result).ToList();
            }

            var sw = Stopwatch.StartNew();
            using (new Swatch($"Sessions {options.SessionCount} created"))
                for (var sessionIndex = 0; sessionIndex < options.SessionCount; sessionIndex++)
                {
                    var session = await repo.AddSession(new Session {Name = $"Session {sessionIndex}"});
                    for (var riderIndex = 0; riderIndex < riders.Count; riderIndex++)
                    {
                        var rider = riders[riderIndex];
                        for (var j = 0; j < options.CheckpointCount; j++)
                        {
                            await repo.AddCheckpoint(new Checkpoint(), session, rider);
                        }

                        await repo.AddRiderSessionResult(new RiderSessionResult
                        {
                            Position = int.Parse(rider.Number)
                        }, session, rider);
                        if (sw.ElapsedMilliseconds > options.ProgressInterval)
                        {
                            Console.WriteLine($"Inserted Sessions: {sessionIndex}, RiderResults: {riderIndex}");
                            sw.Restart();
                        }
                    }
                }
        }

        private static async Task ReadData(IBraaapRepository repo, Options options)
        {
            var sw = Stopwatch.StartNew();
            using (new Swatch($"ReadData {options.ReadIterations} iterations"))
            {
                for (var i = 0; i < options.ReadIterations; i++)
                {
                    for (var sessionIndex = 0; sessionIndex < options.SessionCount; sessionIndex++)
                    {
                        var session = await repo.GetSessionByName($"Session {sessionIndex}");
                        var results = await repo.GetSessionResults(session.SessionId);
                        
                        if (sw.ElapsedMilliseconds > options.ProgressInterval)
                        {
                            Console.WriteLine($"ReadData Iteration: {i}, Session: {sessionIndex}");
                            sw.Restart();
                        }
                    }
                }
            }
        }
    }
}
namespace BraaapDbBenchmark
{
    public class Options
    {
        public string UseEngines { get; set; } = "MySql, Lite, Mongo";
        public bool ClearData { get; set; } = true;
        public bool InsertData { get; set; } = true;
        public bool ReadData { get; set; } = true;
        public int RiderCount {get;set;} = 100;
        public int SessionCount {get;set;} = 10;
        public int CheckpointCount {get;set;} = 10;
        
        public int ReadIterations {get;set;} = 10;
        public long ProgressInterval { get; set; } = 10000;
    }
}
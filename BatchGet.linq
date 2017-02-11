<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Threading.dll</Reference>
  <NuGetReference>StackExchange.Redis</NuGetReference>
  <Namespace>StackExchange.Redis</Namespace>
  <Namespace>StackExchange.Redis.KeyspaceIsolation</Namespace>
  <Namespace>System.IO.Compression</Namespace>
</Query>

void Main()
{
	ConfigurationOptions config = new ConfigurationOptions
	{
		EndPoints =
		{
			{ "127.0.0.1", 6379 }
		},
		Password = "twMVC",
		DefaultVersion = new Version(3, 0)
	};

	ConnectionMultiplexer conn = ConnectionMultiplexer.Connect(config);
	IDatabase cache = conn.GetDatabase();
	
	
	
	var tasks = new System.Threading.Tasks.Task<SortedSetEntry[]>[5];
	var batch = cache.CreateBatch();
	tasks[0] = batch.SortedSetRangeByRankWithScoresAsync("BatchSortedSetTest", 3, 3);
	tasks[1] = batch.SortedSetRangeByRankWithScoresAsync("BatchSortedSetTest", 7, 7);
	tasks[2] = batch.SortedSetRangeByRankWithScoresAsync("BatchSortedSetTest", 10, 10);
	tasks[3] = batch.SortedSetRangeByRankWithScoresAsync("BatchSortedSetTest", 13, 13);
	tasks[4] = batch.SortedSetRangeByRankWithScoresAsync("BatchSortedSetTest", 18, 18);
	
	batch.Execute();
	cache.WaitAll(tasks);
	
	foreach (var item in tasks)
	{
		var result = item.Result.First().Element.ToString();
		result.Dump();
	}
}

// Define other methods and classes here
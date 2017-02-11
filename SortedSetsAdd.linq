<Query Kind="Program">
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
	
	var random = new Random(Guid.NewGuid().GetHashCode());
	var sortsetEnties = new List<SortedSetEntry>();
	//var batch = cache.CreateBatch();
	for (int i = 0; i < 20; i++)
	{
		var id = random.Next(1, 100);
		var score = id;
		
		var sortsetEntry = new SortedSetEntry(id.ToString(), score);
		sortsetEnties.Add(sortsetEntry);
	}
	
	cache.SortedSetAdd("SortedSets", sortsetEnties.ToArray());

	var result = cache.SortedSetRangeByRank("SortedSets", 0, -1);
	result.Select(item => item.ToString()).Dump();
	
	cache.SortedSetRemoveRangeByRank("SortedSets", 0, -1);
}

// Define other methods and classes here
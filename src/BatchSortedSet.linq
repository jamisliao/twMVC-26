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

	var batch = cache.CreateBatch();
	for (int i = 0; i < 20; i++)
	{
		batch.SortedSetAddAsync("BatchSortedSetTest", i.ToString().PadLeft(5, '0'), i);
	}

	batch.Execute();
	"Done".Dump();
}

// Define other methods and classes here
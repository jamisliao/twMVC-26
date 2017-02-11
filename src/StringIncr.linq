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
	
	var count = 0;
	var result = default(string);
	
	cache.StringSet("Count", count);
	cache.StringIncrement("Count", 1);
	result = cache.StringGet("Count");
	result.ToString().Dump();
	
	cache.StringDecrement("Count", 1);
	result = cache.StringGet("Count");
	result.ToString().Dump();
}

// Define other methods and classes here
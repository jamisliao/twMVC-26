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

	var counts = new HashEntry[] {
		new HashEntry("Visitors", 0),
		new HashEntry("Unique_Visitors", 0)
	};
	
	cache.HashSet("Counts", counts);
	cache.HashIncrement("Counts", "Visitors", 2);
	cache.HashIncrement("Counts", "Unique_Visitors", 1);

	var visitors = cache.HashGet("Counts", "Visitors").ToString();
	$"Visitors : {visitors}".Dump();

	var uniqueVisitors = cache.HashGet("Counts", "Unique_Visitors").ToString();
	$"Unique Visitors : {uniqueVisitors}".Dump();
	
	"------------------------------- Decrement ----------------------------".Dump();

	cache.HashDecrement("Counts", "Visitors", 2);
	cache.HashDecrement("Counts", "Unique_Visitors", 1);

	visitors = cache.HashGet("Counts", "Visitors").ToString();
	$"Visitors : {visitors}".Dump();

	uniqueVisitors = cache.HashGet("Counts", "Unique_Visitors").ToString();
	$"Unique Visitors : {uniqueVisitors}".Dump();
}

// Define other methods and classes here
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

	for (int i = 0; i < 50; i++)
	{
		cache.ListRightPush("List", i);
	}
	
	var result = cache.ListRange("List", 0, 20);
	foreach (var item in result)
	{
		$"Item : {item}".Dump();
	}
	
	cache.ListTrim("List", 9999, 0);
}

// Define other methods and classes here
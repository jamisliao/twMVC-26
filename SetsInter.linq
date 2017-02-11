<Query Kind="Program">
  <NuGetReference>StackExchange.Redis</NuGetReference>
  <Namespace>StackExchange.Redis</Namespace>
  <Namespace>StackExchange.Redis.KeyspaceIsolation</Namespace>
  <Namespace>System.IO.Compression</Namespace>
</Query>

private IDatabase cache;
private List<string> countryA;
private List<string> countryB;

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
	cache = conn.GetDatabase();

	InitialData();
	
	var intersect = cache.SetCombine(SetOperation.Intersect, "CountryA", "CountryB");
	var diff = cache.SetCombine(SetOperation.Difference, "CountryA", "CountryB");
	var union = cache.SetCombine(SetOperation.Union, "CountryA", "CountryB");
	
	"---------------- Intersect ----------------".Dump();
	Show(intersect);
	
	"---------------- Difference ---------------".Dump();
	Show(diff);
	
	"---------------- Union ------------------".Dump();
	Show(union);
	
	ClearData();
}

public void InitialData()
{

	countryA = new List<string> {
		"Taiwan", "Korea", "Japan", "China", "France", "Greece",
		"Germany", "Italy", "Brazil", "Cuba" 
	};

	countryB = new List<string> {
		"Taiwan", "Korea", "Japan", "China",  "France", "Greece",
		"Turkey", "Singapore", "Macao", "Poland"
	};

	var tmpA = countryA.Select(item => (RedisValue)item).ToArray();
	var tmpB = countryB.Select(item => (RedisValue)item).ToArray();

	cache.SetAdd("CountryA", tmpA);
	cache.SetAdd("CountryB", tmpB);
}

public void ClearData()
{
	var tmpA = countryA.Select(item => (RedisValue)item).ToArray();
	var tmpB = countryB.Select(item => (RedisValue)item).ToArray();
	cache.SetRemove("CountryA", tmpA);
	cache.SetRemove("CountryB", tmpB);
}

public void Show(RedisValue[] values)
{
	var temp = string.Join(",", values.Select(item => item.ToString()).ToArray());
	$"Item : {temp} \r\n".Dump();
}

// Define other methods and classes here
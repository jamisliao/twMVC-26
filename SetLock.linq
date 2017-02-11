<Query Kind="Program">
  <NuGetReference>StackExchange.Redis</NuGetReference>
  <Namespace>StackExchange.Redis</Namespace>
  <Namespace>StackExchange.Redis.KeyspaceIsolation</Namespace>
  <Namespace>System.IO.Compression</Namespace>
</Query>

private IDatabase cache;
private List<string> productStockIds = new List<string>();

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
	
	for (int i = 0; i < 14; i++)
	{
		$"第{i + 1}個使用者".Dump();
		var result = cache.SetPop("ProductStockIds");

		if (result.IsNull)
		{
			"Out of Stock \r\n".Dump();
		}
		else
		{
			$"第{i + 1}筆訂單".Dump();
			$"ProductStockId : {result.ToString()}".Dump();

			var tmp = cache.SetMembers("ProductStockIds");
			Show(tmp);
		}
	}
}

public void InitialData()
{
	for (int i = 0; i < 10; i++)
	{
		this.productStockIds.Add(Guid.NewGuid().ToString("N"));
	}

	var tmpA = productStockIds.Select(item => (RedisValue)item).ToArray();
	cache.SetAdd("ProductStockIds", tmpA);
}

public void Show(RedisValue[] values)
{
//	var temp = string.Join(",", values.Select(item => item.ToString()).ToArray());
//	$"Sets element : {temp} \r\n".Dump();

	int index = 0;
	"Sets element :".Dump();
	foreach (var item in values)
	{
		$"		{index}. {item} \r\n".Dump();
		index++;
	}
	
	"\r\n -------------------------------------------------------- \r\n".Dump();
}
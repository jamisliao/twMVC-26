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

	var weather = new Weather {
		Temperature = 28,
		Precipitation = 0,
		MaxTemperature = 32,
		MinTemperature = 25,
		Wind = 4,
		WindDirection = "South Wind"
	};
	
	var hashs = GetHash(weather);
	cache.HashSet("Weather_12", hashs);

	var temperature = cache.HashGet("Weather_12", "Temperature");
	$"Temperature : {temperature}".Dump();

	var maxTemperature = cache.HashGet("Weather_12", "MaxTemperature");
	$"Max Temperature : {maxTemperature}".Dump();
	
	"--------------------------------------------------------------------".Dump();
	
	var temp = cache.HashGetAll("Weather_12");
	foreach (var item in temp)
	{
		$"{item.Name} : {item.Value}".Dump();
	}
}

public HashEntry[] GetHash(Weather weather)
{
	var hashs = new HashEntry[] {
		new HashEntry("Temperature", weather.Temperature),
		new HashEntry("Precipitation", weather.Precipitation),
		new HashEntry("MaxTemperature", weather.MaxTemperature),
		new HashEntry("MinTemperature", weather.MinTemperature),
		new HashEntry("Wind", weather.Wind),
		new HashEntry("WindDirection", weather.WindDirection),
	};
	
	return hashs;
}

public class Weather
{
	//// 溫度
	public int Temperature { get; set; }
	
	//// 降雨機率
	public int Precipitation { get; set; }
	
	//// 最高溫度
	public int MaxTemperature { get; set; }
	
	//// 最低溫度
	public int MinTemperature { get; set; }

	//// 風力
	public double Wind { get; set; }
	
	//// 風向
	public string WindDirection { get; set; }
}
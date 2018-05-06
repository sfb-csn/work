<Query Kind="Program">
  <NuGetReference>Csn.Dto</NuGetReference>
  <NuGetReference>Elasticsearch.Net</NuGetReference>
  <NuGetReference>NEST</NuGetReference>
  <Namespace>Elasticsearch.Net</Namespace>
  <Namespace>Nest</Namespace>
  <Namespace>Purify</Namespace>
  <Namespace>Csn.Dto.Car.Ad</Namespace>
  <Namespace>Csn.Dto.Ad</Namespace>
  <Namespace>System.Security.Cryptography</Namespace>
</Query>


void Main()
{
	var pool = new SingleNodeConnectionPool(new Uri("https://search-latam-unu2atntk45nmutbq73sjkejq4.us-west-2.es.amazonaws.com"));
	ConnectionSettings connectionSettings = new ConnectionSettings(pool).DefaultIndex("car-2017-04-24");
	var client = new ElasticClient(connectionSettings);

	SearchDescriptor<dynamic> debugQuery = new SearchDescriptor<dynamic>()
					.TypedKeys(null)
					.Index("car-2017-04-24")
					.Type("Car")
					.Size(500)
					.Source(src => src.Includes(inc =>inc.Field("Seller.Type")));

	var searchResponse = client.Search<dynamic>(debugQuery);

	using (MemoryStream mStream = new MemoryStream())
	{
		client.RequestResponseSerializer.Serialize(debugQuery, mStream);
		string rawQueryText = Encoding.ASCII.GetString(mStream.ToArray());
		rawQueryText.Dump();
	}

	var count = searchResponse.Hits.First().Dump(); h
}
private Guid GetDeterministicGuid(string input)

{
	//use MD5 hash to get a 16-byte hash of the string:

	var provider = new MD5CryptoServiceProvider();

	byte[] inputBytes = Encoding.Default.GetBytes(input);

	byte[] hashBytes = provider.ComputeHash(inputBytes);

	//generate a guid from the hash:

	var hashGuid = new Guid(hashBytes);

	return hashGuid;
}
<Query Kind="Program">
  <NuGetReference>Csn.Dto</NuGetReference>
  <NuGetReference>Elasticsearch.Net</NuGetReference>
  <NuGetReference>NEST</NuGetReference>
  <Namespace>Elasticsearch.Net</Namespace>
  <Namespace>Nest</Namespace>
  <Namespace>Purify</Namespace>
  <Namespace>Csn.Dto.Car.Ad</Namespace>
  <Namespace>Csn.Dto.Industry.Ad</Namespace>
  <Namespace>Csn.Dto.Bike.Ad</Namespace>
</Query>

void Main()
{
	var pool = new SingleNodeConnectionPool(new Uri("https://search-latam-unu2atntk45nmutbq73sjkejq4.us-west-2.es.amazonaws.com"));
	var client = new ElasticClient(new ConnectionSettings(pool).DefaultIndex("car-2017-04-24"));

	SearchDescriptor<BikeAd> debugQuery = new SearchDescriptor<BikeAd>()
		.TypedKeys(null)
					.Index("bike-2017-04-20")
					.Type("Bike")
					.Size(50)
					.Source(src =>
						src.Includes(inc => inc.Field("Id")));

	using (MemoryStream mStream = new MemoryStream())
	{
		client.RequestResponseSerializer.Serialize(debugQuery, mStream);
		Encoding.ASCII.GetString(mStream.ToArray()).Dump();
	}
	var result = client.Search<BikeAd>(debugQuery);

	result.Documents.Dump();
}

<Query Kind="Program">
  <NuGetReference>Csn.Dto</NuGetReference>
  <NuGetReference>Elasticsearch.Net</NuGetReference>
  <NuGetReference>NEST</NuGetReference>
  <Namespace>Elasticsearch.Net</Namespace>
  <Namespace>Nest</Namespace>
  <Namespace>Purify</Namespace>
  <Namespace>Csn.Dto.Car.Ad</Namespace>
</Query>

void Main()
{
	var pool = new SingleNodeConnectionPool(new Uri("https://search-latam-unu2atntk45nmutbq73sjkejq4.us-west-2.es.amazonaws.com"));
	ConnectionSettings connectionSettings = new ConnectionSettings(pool).DefaultIndex("car-2017-04-24");
	var client = new ElasticClient(connectionSettings);
	// count
	var totalRecordCount = client.Count<CarAd>(s => s
		.Type("Car")
		.Query(q => q.MatchAll())).Count;

	long dealer = 0;
	long noDealer = 0;
	long skip = 0;
	long maxFetch=9999;
	long size = maxFetch;
	for (long i = 0; i < totalRecordCount;)
	{
		var searchResponse = client.Search<CarAd>(s => s
			.TypedKeys(null)
			.Size((int)size)
			.Skip((int)skip)
			.Type("Car")
			.Query(q => q.MatchAll()));

		dealer += searchResponse.Documents.Count(c => !string.IsNullOrWhiteSpace(c.Seller.DealerName));
		noDealer += searchResponse.Documents.Count(c => string.IsNullOrWhiteSpace(c.Seller.DealerName));
		skip += size;
		var remain = totalRecordCount-skip;
		size=remain>maxFetch?maxFetch:remain;
		i += size;
	}

	dealer.Dump();
	noDealer.Dump();
}

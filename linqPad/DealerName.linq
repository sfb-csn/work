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


	SearchDescriptor<CarAd> debugQuery = new SearchDescriptor<CarAd>()
			//var searchResponse = client.Search<CarAd>(s => s
			.TypedKeys(null)
			.Type("CarAd")
			.Query(q => q.Nested(n => n
								.Path(p => p.Seller)
								.Query(nq => nq.Bool(b => b.Must(m => m.Term(mt => mt.Field("SellerDealerName").Value("")))))));

	// q.Bool(b => b.Must(m1 => m1.Term(t => t.Field("Seller.DealerName").Value("")))));

	using (MemoryStream mStream = new MemoryStream())
	{
		client.RequestResponseSerializer.Serialize(debugQuery, mStream);
		string rawQueryText = Encoding.ASCII.GetString(mStream.ToArray());
		rawQueryText.Dump();
	}
	var searchResponse = client.Search<CarAd>(debugQuery);
	searchResponse.Documents.Dump();
}
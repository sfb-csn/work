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

		var scrollObservable = client.ScrollAll<CarAd>("1m", 8, s => s
			.MaxDegreeOfParallelism(8 / 2)
			.Search(search => search
				.Type("Car")
				.TypedKeys(null)
				.MatchAll())
		);

		var x = scrollObservable.DumpLatest();
}
<Query Kind="Program">
  <NuGetReference>Csn.Dto</NuGetReference>
  <NuGetReference>Elasticsearch.Net</NuGetReference>
  <NuGetReference>NEST</NuGetReference>
  <Namespace>Nest</Namespace>
  <Namespace>Elasticsearch.Net</Namespace>
  <Namespace>Csn.Dto.Car.Ad</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <IncludePredicateBuilder>true</IncludePredicateBuilder>
</Query>

public SingleNodeConnectionPool pool => new SingleNodeConnectionPool(new Uri("https://search-latam-unu2atntk45nmutbq73sjkejq4.us-west-2.es.amazonaws.com"));
public ConnectionSettings connectionSettings => new ConnectionSettings(pool).DefaultIndex("car-2017-04-24");
public ElasticClient _client => new ElasticClient(connectionSettings);

void Main()
{
	var carAds = new List<CarAd>();
	var seenDocuments = 0;
	var seenSlices = new ConcurrentBag<int>();
	var scrollObserver = this._client.ScrollAll<CarAd>("1m", 3, s => s
		.MaxDegreeOfParallelism(3)
		.Search(search => search
			.TypedKeys(null)
			.Index("car-2017-04-24")
			.Type("Car")
			.MatchAll()
		)
	).Wait(TimeSpan.FromMinutes(15), r =>
	{
		seenSlices.Add(r.Slice);
		Interlocked.Add(ref seenDocuments, r.SearchResponse.Hits.Count);
		carAds.AddRange(r.SearchResponse.Documents);
	});

	seenDocuments.Dump();
	var groups = seenSlices.GroupBy(s => s).ToList();
	// groups.Count().Should().Be(numberOfShards);
	// groups.Should().OnlyContain(g => g.Count() > 1);

}

// Define other methods and classes here
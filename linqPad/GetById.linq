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
	ConnectionSettings connectionSettings = new ConnectionSettings(pool);
	var client = new ElasticClient(connectionSettings);

//	client.Get<IndustryAd>("CL-AD-5791498", f => f.Index("industry-2017-04-20").Type("Industry")).Source.Dump();
//	client.Get<CarAd>("MX-SHRM-AD-679573120150330", f => f.Index("car-2017-04-24").Type("Car")).DebugInformation.Dump();
	//.Source.Dump();

	using (MemoryStream mStream = new MemoryStream())
	{
		client.RequestResponseSerializer.Serialize(client.Get<CarAd>("SA-AD-504762", f => f.Index("car-2017-04-24").Type("Car")), mStream);
		Encoding.ASCII.GetString(mStream.ToArray()).Dump();
	}
}
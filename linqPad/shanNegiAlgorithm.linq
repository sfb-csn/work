<Query Kind="Program">
  <NuGetReference>Csn.Dto</NuGetReference>
  <NuGetReference>Elasticsearch.Net</NuGetReference>
  <NuGetReference>NEST</NuGetReference>
  <IncludePredicateBuilder>true</IncludePredicateBuilder>
</Query>

void Main()
{
//	x.Dump();
//	var squares4 = new SquareBuilder().Build(4);
//	squares5.Dump();
	var squares3 = new SquareBuilder().Build(13);
	squares3.Dump();
}

public string x => "asdf";

public class SquareBuilder
{
	private int[,] Build(int size, int depth, int val, int[,] array = null)
	{
		if (array == null)
			array = new int[size, size];

		for (int i = 0; i < size; i++)
		{
			val = val + 1;
			array[depth, i + depth] = val;
		}

		for (int i = 0; i < (size - 1); i++)
		{
			val = val + 1;
			array[i + 1 + depth, size + depth - 1] = val;
		}

		for (int i = (size - 2); i >= 0; i--)
		{
			val = val + 1;
			array[size + depth - 1, i + depth] = val;
		}

		for (int i = (size - 2); i > 0; i--)
		{
			val = val + 1;
			array[depth + i, depth] = val;
		}

		if ((size - 2) >= 1)
		{
			//Recursive call for inner square
			Build(size - 2, depth + 1, val, array);
		}

		return array;
	}

	public int[,] Build(int size)
	{
		return Build(size, 0, 0);
	}
}


//Sample usage:

// Define other methods and classes here

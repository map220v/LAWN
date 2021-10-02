using System;

internal static class RandomNumbers
{
	private static Random r;

	internal static int NextNumber()
	{
		if (r == null)
		{
			Seed();
		}
		return r.Next();
	}

	internal static int NextNumber(int ceiling)
	{
		if (r == null)
		{
			Seed();
		}
		return r.Next(ceiling);
	}

	internal static float NextNumber(float ceiling)
	{
		if (r == null)
		{
			Seed();
		}
		return (float)r.NextDouble() * ceiling;
	}

	internal static void Seed()
	{
		r = new Random();
	}

	internal static void Seed(int seed)
	{
		r = new Random(seed);
	}
}

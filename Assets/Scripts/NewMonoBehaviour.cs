using UnityEngine;
using System.Collections;
using Random = System.Random;

public static class RandomUtils
{
    public static float Range(this Random random, float min, float max)
    {
        var d = random.NextDouble();
        return (float) (min + d * (max - min));
    }
}

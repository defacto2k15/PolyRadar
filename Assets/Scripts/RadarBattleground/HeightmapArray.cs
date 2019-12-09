using System.Linq;
using UnityEngine;

namespace Assets.Scripts.RadarBattleground
{
    public class HeightmapArray
    {
        private float[] _array;
        private Vector2Int _size;

        public HeightmapArray(float[] array, Vector2Int size)
        {
            _array = array;
            _size = size;
        }

        public float SampleWithFilter(Vector2 query)
        {
            var lower = query.FloorToInt();
            var upper = query.CeilToInt();
            var pixelSamples = new[]
            {
                new Vector2Int(lower.x, lower.y),
                new Vector2Int(upper.x, lower.y),
                new Vector2Int(lower.x, upper.y),
                new Vector2Int(upper.x, upper.y),
            }.Select(c => new Vector2Int(Mathf.Clamp((int) c.x,0, _size.x-1), Mathf.Clamp((int) c.y,0,_size.y-1)));

            var t = query - lower;
            var sampledColors = pixelSamples.Select(SamplePixel).ToList();
            return (1 - t.x) * (1 - t.y) * sampledColors[0] +
                   t.x * (1 - t.y) * sampledColors[1] +
                   (1 - t.x) * t.y * sampledColors[2] +
                   t.x * t.y * sampledColors[3];
        }

        public float SamplePixel(Vector2Int coords)
        {
            MyAssert.IsTrue(coords.x >= 0);
            MyAssert.IsTrue(coords.y >= 0);
            MyAssert.IsTrue(coords.x < _size.x);
            MyAssert.IsTrue(coords.y < _size.y);

            return _array[CoordsToArrayIndex(coords)];
        }

        private int CoordsToArrayIndex(Vector2Int coords)
        {
            return coords.y * _size.x + coords.x;
        }

        public void SetPixel(Vector2Int coords, float pixelValue)
        {
            _array[CoordsToArrayIndex(coords)] = pixelValue;
        }

        public float[] Array => _array;

        public Vector2Int Size => _size;
    }
}
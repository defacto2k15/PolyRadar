using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public static class VectorUtils
    {
        public static Vector2Int FloorToInt(this Vector2 vec)
        {
            return new Vector2Int(Mathf.FloorToInt(vec.x), Mathf.FloorToInt(vec.y));
        }

        public static Vector2Int CeilToInt(this Vector2 vec)
        {
            return new Vector2Int(Mathf.CeilToInt(vec.x), Mathf.CeilToInt(vec.y));
        }

        public static Vector2 MemberwiseMultiply(this Vector2 vec, Vector2 multipliers)
        {
            return new Vector2(vec.x*multipliers.x, vec.y*multipliers.y);
        }
    }
}

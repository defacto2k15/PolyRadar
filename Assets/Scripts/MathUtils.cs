using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public static class MathUtils
    {
        public static Vector2 PolarToCartesian(Vector2 rAndPhi)
        {
            return new Vector2(rAndPhi.x*Mathf.Cos(rAndPhi.y), rAndPhi.x*Mathf.Sin(rAndPhi.y));
        }

        public static Vector2 CartesianToPolar(Vector2 pos)
        {
            return new Vector2(pos.magnitude, Mathf.Atan2(pos.y, pos.x));
        }
    }

    public static class RectUtils
    {
        public static Vector2 UvInRect(this Rect rect, Vector2 pos)
        {
            var delta = pos - rect.min;
            return new Vector2(delta.x / rect.width, delta.y/rect.height);
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.RadarManipulators
{
    [Serializable]
    public class MyVector2Event : UnityEvent<Vector2>
    {
    }
}
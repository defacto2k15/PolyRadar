using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Vehicles
{
    public class VehicleMovementTrackerOC : MonoBehaviour
    {
        private Vector2 _lastFlatPosition;

        void Update()
        {
            _lastFlatPosition = new Vector2(transform.position.x, transform.position.z);
        }

        public Vector2 MovementDirection => (new Vector2(transform.position.x, transform.position.z) - _lastFlatPosition).normalized;
    }
}

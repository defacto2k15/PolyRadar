using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Vehicles
{
    public class VehicleMovementTrackerOC : MonoBehaviour
    {
        public int TrackedQueueSize = 10;
        private Queue<Vector2> _flatPositionsQueue;

        void Start()
        {
            _flatPositionsQueue = new Queue<Vector2>(TrackedQueueSize);
            for (int i = 0; i < TrackedQueueSize; i++)
            {
                _flatPositionsQueue.Enqueue(Vector2.zero);
            }
        }

        void FixedUpdate()
        {
            if (_flatPositionsQueue.Count >= TrackedQueueSize)
            {
                _flatPositionsQueue.Dequeue();
            }
            var flatPosition = new Vector2(transform.position.x, transform.position.z);
            _flatPositionsQueue.Enqueue(flatPosition);
        }

        public Vector2 MovementDirection
        {
            get
            {
                var flatPositions = _flatPositionsQueue.ToList();
                var deltasSum = Enumerable.Range(0, _flatPositionsQueue.Count - 1).Select(i => flatPositions[i+1] - flatPositions[i ])
                    .Aggregate(Vector2.zero, (a, b) => a + b);
                return (deltasSum / TrackedQueueSize).normalized;

            }
        }
    }
}

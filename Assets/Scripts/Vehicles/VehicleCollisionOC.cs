using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Vehicles
{
    public class VehicleCollisionOC : MonoBehaviour
    {
        public VehicleOC ParentVehicle;

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.RocketTag))
            {
                ParentVehicle.WasHit();
            }
        }
    }
}

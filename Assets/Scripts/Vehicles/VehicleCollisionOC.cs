using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Vehicles
{
    [RequireComponent(typeof(VehicleSoundOC))]
    public class VehicleCollisionOC : MonoBehaviour
    {
        public GameObject DestructionParticlePrefab;
        public VehicleOC ParentVehicle;
        private VehicleSoundOC _sound;

        void Start()
        {
            _sound = GetComponent<VehicleSoundOC>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.RocketTag))
            {
                _sound.StartVehicleExplosionSound();
                Instantiate(DestructionParticlePrefab, gameObject.transform.position, gameObject.transform.rotation);
                ParentVehicle.WasHit();
            }
        }
    }
}

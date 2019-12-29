using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Rocket
{
    public class RocketCollisionOC : MonoBehaviour
    {
        public GameObject DestructionParticlePrefab; 

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.FlyingVehicleTag))
            {
                DestroyRocket();
            }
        }

        private void DestroyRocket()
        {
            Instantiate(DestructionParticlePrefab, transform.position, transform.rotation);
            GameObject.Destroy(this.gameObject);
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.BattlegroundTag))
            {
                DestroyRocket();
            }
        }
    }
}

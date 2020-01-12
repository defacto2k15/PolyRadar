using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Rocket
{
    [RequireComponent(typeof(RocketSoundOC))]
    public class RocketCollisionOC : MonoBehaviour
    {
        public GameObject DestructionParticlePrefab;
        private RocketSoundOC _sound;

        void Start()
        {
            _sound = GetComponent<RocketSoundOC>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.FlyingVehicleTag))
            {
                DestroyRocket();
            }
        }

        private void DestroyRocket()
        {
            _sound.StartRocketExplosionSound();
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

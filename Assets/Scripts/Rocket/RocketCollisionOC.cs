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
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.FlyingVehicleTag))
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}

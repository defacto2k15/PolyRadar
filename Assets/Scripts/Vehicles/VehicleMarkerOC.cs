using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Vehicles
{
    public class VehicleMarkerOC : MonoBehaviour
    {
        public void SetMarkerVisible(bool visible)
        {
            GetComponent<MeshRenderer>().enabled = visible;
        }
    }
}

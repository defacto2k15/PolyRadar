using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Vehicles
{
    public class BattlegroundVehiclesRootOC : MonoBehaviour
    {
        public void SetVehiclesEnabled (bool isEnabled)
        {
            GetComponentsInChildren<VehicleOC>().ToList().ForEach(c => c.enabled = isEnabled);
        }
        public void SetVehiclesVisible(bool isVisible)
        {
            GetComponentsInChildren<VehicleOC>().ToList().ForEach(c => c.GetComponent<MeshRenderer>().enabled = isVisible);
        }

        public List<VehicleOC> AllVehicles => GetComponentsInChildren<VehicleOC>().ToList();
    }
}

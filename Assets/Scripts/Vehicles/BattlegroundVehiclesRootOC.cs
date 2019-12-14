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
            GetComponentsInChildren<DummyFlyingVehicleOC>().ToList().ForEach(c => c.enabled = isEnabled);
        }
        public void SetVehiclesVisible(bool isVisible)
        {
            GetComponentsInChildren<DummyFlyingVehicleOC>().ToList().ForEach(c => c.GetComponent<MeshRenderer>().enabled = isVisible);
        }

        public List<DummyFlyingVehicleOC> AllVehicles => GetComponentsInChildren<DummyFlyingVehicleOC>().ToList();
    }
}

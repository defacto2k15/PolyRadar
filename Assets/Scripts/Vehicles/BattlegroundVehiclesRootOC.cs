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
        public void SetVehiclesEnabled(bool isEnabled)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(isEnabled);
            }
        }

        public void SetMarkersVisible(bool visible)
        {
            GetComponentsInChildren<VehicleMarkerOC>().ToList().ForEach(c=>c.SetMarkerVisible(visible));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.RadarDisplay;
using Assets.Scripts.Vehicles;
using UnityEngine;

namespace Assets.Scripts
{
    public class RadarModeControllerOC : MonoBehaviour
    {
        public RadarMarkersManagerOC MarkersManager;

        public void Update()
        {
            int selectedMarkerIndex = 0;

            int selectedMarkerOffset = 0;
            if (Input.GetKeyDown(KeyCode.A))
            {
                selectedMarkerOffset=1;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                selectedMarkerOffset=-1;
            }

            if (MarkersManager.HasSelectableVehicle && selectedMarkerOffset!= 0)
            {
                MarkersManager.ChangeVehicleSelection(selectedMarkerOffset);
            }

            if (MarkersManager.HasSelectedVehicle)
            {
                VehicleAffinity affinity = VehicleAffinity.Unknown;
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    affinity = VehicleAffinity.Foe;
                }else if (Input.GetKeyDown(KeyCode.W))
                {
                    affinity = VehicleAffinity.Friend;
                }

                if (affinity != VehicleAffinity.Unknown)
                {
                    MarkersManager.SetVehicleAffinity(MarkersManager.SelectedVehicle, affinity);
                }
            }
        }

        public void SetVehicleAffinity()
        {

        }
    }

    public enum VehicleAffinity
    {
        Unknown, Friend, Foe
    }
}

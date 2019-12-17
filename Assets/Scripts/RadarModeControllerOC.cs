using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.RadarDisplay;
using Assets.Scripts.Rocket;
using Assets.Scripts.Vehicles;
using UnityEngine;

namespace Assets.Scripts
{
    public class RadarModeControllerOC : MonoBehaviour
    {
        public VehiclesSelectingManagerOC MarkersManager;
        public RocketSpawnerScript RocketSpawner;
        private RadarModeCondition _condition = RadarModeCondition.SearchingTarget;

        public void Update()
        {
            if (_condition == RadarModeCondition.SearchingTarget)
            {
                int selectedMarkerOffset = 0;
                if (Input.GetKeyDown(KeyCode.A))
                {
                    selectedMarkerOffset = 1;
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    selectedMarkerOffset = -1;
                }

                if (MarkersManager.HasSelectableVehicle && selectedMarkerOffset != 0)
                {
                    MarkersManager.ChangeVehicleSelection(selectedMarkerOffset);
                }

                if (MarkersManager.HasSelectedVehicle)
                {
                    VehicleAffinity affinity = VehicleAffinity.Unknown;
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        affinity = VehicleAffinity.Foe;
                    }
                    else if (Input.GetKeyDown(KeyCode.W))
                    {
                        affinity = VehicleAffinity.Friend;
                    }

                    if (affinity != VehicleAffinity.Unknown)
                    {
                        MarkersManager.SetVehicleAffinity(MarkersManager.SelectedVehicle, affinity);
                    }
                }


                if (Input.GetKeyDown(KeyCode.Space))
                {
                    RocketSpawner.SpawnRocket();
                    _condition = RadarModeCondition.SteeringRocket;
                }
            }
            else if (_condition == RadarModeCondition.SteeringRocket)
            {
                if (FindObjectOfType<RocketScript>() == null)
                {
                    _condition = RadarModeCondition.SearchingTarget;
                }
            }
        }
    }

    public enum RadarModeCondition
    {
        SearchingTarget, SteeringRocket
    }

    public enum VehicleAffinity
    {
        Unknown, Friend, Foe
    }
}

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
    public class RadarModeControllerOC : SingleGameModeController
    {
        public VehiclesSelectingManagerOC VehicleSelectionManager;
        public RocketSpawnerScript RocketSpawner;
        private RadarModeCondition _condition = RadarModeCondition.SearchingTarget;
        private bool _inputEnabled = false;

        public void Update()
        {
            if (_inputEnabled)
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

                    if (VehicleSelectionManager.HasSelectableVehicle && selectedMarkerOffset != 0)
                    {
                        VehicleSelectionManager.ChangeVehicleSelection(selectedMarkerOffset);
                    }

                    if (VehicleSelectionManager.HasSelectedVehicle)
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
                            VehicleSelectionManager.SetVehicleAffinity(VehicleSelectionManager.SelectedVehicle, affinity);
                        }
                    }


                    if (FindObjectOfType<RocketScript>()!=null)
                    {
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

        public override void DisableMode()
        {
            ChangeInputEnabled(false);
        }

        private void ChangeInputEnabled(bool isEnabled)
        {
            _inputEnabled = isEnabled;
            RocketSpawner.ChangeInputEnabled(isEnabled);
        }

        public override void EnableMode()
        {
            ChangeInputEnabled(true);
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

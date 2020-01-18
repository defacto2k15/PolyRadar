using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.RadarDisplay;
using Assets.Scripts.Rocket;
using Assets.Scripts.Sound;
using Assets.Scripts.Vehicles;
using UnityEngine;

namespace Assets.Scripts
{
    public class RadarModeControllerOC : SingleGameModeController
    {
        public TimeProviderGo TimeProvider;
        public SoundSourceMasterOC RadarSoundSource;
        public VehiclesSelectingManagerOC VehicleSelectionManager;
        public RocketSpawnerScript RocketSpawner;
        private RadarModeCondition _condition = RadarModeCondition.SearchingTarget;
        private bool _inputEnabled = false;

        public VehicleDetails SelectedVehicleDetails
        {
            get
            {
                if (!VehicleSelectionManager.HasSelectedVehicle)
                {
                    return null;
                }
                else
                {
                    return VehicleSelectionManager.SelectedVehicle.Details;
                }
            }
        }

        void Start()
        {
            RadarSoundSource.StartPerpetualSound(PerpetualSoundKind.RadarBackground);
        }

        public void Update()
        {
            if (_inputEnabled)
            {
                if (_condition == RadarModeCondition.SearchingTarget)
                {
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        ChangeSelectedTarget(-1);
                    }

                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        ChangeSelectedTarget(1);
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

                    if (FindObjectOfType<RocketScript>() != null)
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

        public void ChangeSelectedTarget(int selectedMarkerOffset)
        {
            if (VehicleSelectionManager.HasSelectableVehicle)
            {
                RadarSoundSource.StartOneShotSound(SingleShotSoundKind.TargetChange);
                VehicleSelectionManager.ChangeVehicleSelection(selectedMarkerOffset);
            }
        }

        public override void DisableMode()
        {
            ChangeInputEnabled(false);
            TimeProvider.TimeUpdateEnabled = false;
        }

        private void ChangeInputEnabled(bool isEnabled)
        {
            _inputEnabled = isEnabled;
            RocketSpawner.ChangeInputEnabled(isEnabled);
        }

        public override void EnableMode()
        {
            ChangeInputEnabled(true);
            TimeProvider.TimeUpdateEnabled = true;
        }

        public void UnveilAffinityOfSelectedVehicle()
        {
            if (VehicleSelectionManager.HasSelectedVehicle)
            {
                VehicleSelectionManager.SelectedVehicle.UnveilAffinity();
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

    public enum KnownVehicleAffinity
    {
         Friend, Foe
    }
}

using Assets.Scripts.sinewaves;
using Assets.Scripts.Sound;
using Assets.Scripts.Vehicles;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class SineGameModeControllerOC : SingleGameModeController
    {
        public SoundSourceMasterOC SoundSourceMaster;
        public GameObject SineUiRoot;
        public SineComparatorScript SineComparator;
        public SineMatchTextOC MatchText;

        public bool WasMatchAchieved;

        private VehicleDetails _currentVehicleDetails;
        public VehicleDetails CurrentVehicleDetails 
        {
            set {
                _currentVehicleDetails = value;
                SineComparator.GenerateTargetSineParameters(_currentVehicleDetails.ID);
            }
            get { return _currentVehicleDetails; }
        }

        void Start()
        {
            SoundSourceMaster.StartPerpetualSound(PerpetualSoundKind.OscilloscopeBackground);
        }

        public override void DisableMode()
        {
            SineUiRoot.SetActive(false);
            MatchText.ChangeIndicationMode(SineMatchTextIndicatorsMode.None);
            MatchText.enabled = false;
            SineComparator.enabled = false;
        }

        public override void EnableMode()
        {
            SineUiRoot.SetActive(true);
            MatchText.enabled = true;
            SineComparator.enabled = true;
            SineComparator.FrameCountSinceMatch = 0;
            MatchText.ChangeIndicationMode(SineMatchTextIndicatorsMode.None);
            WasMatchAchieved = false;
        }

        public void MatchWasAchieved()
        {
            WasMatchAchieved = true;
            if (CurrentVehicleDetails.KnownAffinity == KnownVehicleAffinity.Friend)
            {
                MatchText.ChangeIndicationMode(SineMatchTextIndicatorsMode.Friend);
            }
            else
            {
                MatchText.ChangeIndicationMode(SineMatchTextIndicatorsMode.Foe);
            }
        }

    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Sound;
using UnityEngine;

namespace Assets.Scripts
{
    public class MasterGameModesControllerOC : MonoBehaviour
    {
        public Camera Camera;
        public SineGameModeControllerOC SineComparisionModeRoot;
        public RadarModeControllerOC RadarModeRoot;
        public SoundSourceMasterOC SoundSourceMaster;

        private GameMode _currentMode;
        private bool _duringModeChange;

        public void Start()
        {
            StartCoroutine(ChangeMode(GameMode.RadarMode));
        }

        public void Update()
        {
            if (!_duringModeChange)
            {
                if (Input.GetKeyDown(KeyCode.P))
                {
                    ToggleMode();
                }
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                Camera.GetComponent<MainCameraAnimationControllerOC>().MoveToRadar();
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                Camera.GetComponent<MainCameraAnimationControllerOC>().MoveToSine();
            }
        }

        public void ToggleMode()
        {
            if (_currentMode == GameMode.RadarMode)
            {
                StartCoroutine(ChangeMode(GameMode.SineMode));
            }
            else
            {
                StartCoroutine(ChangeMode(GameMode.RadarMode));
            }
        }


        public IEnumerator ChangeMode(GameMode newMode)
        {
            _duringModeChange = true;

            var mainCameraAnimationController = Camera.GetComponent<MainCameraAnimationControllerOC>();
            if (newMode== GameMode.RadarMode)
            {
                SineComparisionModeRoot.DisableMode();

                mainCameraAnimationController.MoveToRadar();

                if (SineComparisionModeRoot.WasMatchAchieved)
                {
                    RadarModeRoot.UnveilAffinityOfSelectedVehicle();
                }
                RadarModeRoot.EnableMode();
            }
            else
            {
                var selectedVehicleDetails = RadarModeRoot.SelectedVehicleDetails;
                if (selectedVehicleDetails == null)
                {
                    _duringModeChange = false;
                    yield break;
                }
                RadarModeRoot.DisableMode();
                SineComparisionModeRoot.CurrentVehicleDetails = selectedVehicleDetails;

                mainCameraAnimationController.MoveToSine();

                SineComparisionModeRoot.EnableMode();
            }

            SoundSourceMaster.StartOneShotSound(SingleShotSoundKind.ModeChange);
            yield return mainCameraAnimationController.WaitForTransitionAnimationToEnd();


            _currentMode = newMode;
            _duringModeChange = false;
        }
    }

    public enum GameMode
    {
        RadarMode, SineMode
    }
}


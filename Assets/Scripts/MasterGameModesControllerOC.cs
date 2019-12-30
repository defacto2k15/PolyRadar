using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class MasterGameModesControllerOC : MonoBehaviour
    {
        public Camera Camera;
        public SingleGameModeController SineComparisionModeRoot;

        public SingleGameModeController RadarModeRoot;

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
                    if (_currentMode == GameMode.RadarMode)
                    {
                        StartCoroutine(ChangeMode(GameMode.SineMode));
                    }
                    else
                    {
                        StartCoroutine(ChangeMode(GameMode.RadarMode));
                    }
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

        public IEnumerator ChangeMode(GameMode newMode)
        {
            _duringModeChange = true;
            _currentMode = newMode;

            var mainCameraAnimationController = Camera.GetComponent<MainCameraAnimationControllerOC>();
            if (_currentMode == GameMode.RadarMode)
            {
                SineComparisionModeRoot.DisableMode();

                mainCameraAnimationController.MoveToRadar();
            }
            else
            {
                RadarModeRoot.DisableMode();

                mainCameraAnimationController.MoveToSine();
            }

            Debug.Log("Yield started");
            yield return mainCameraAnimationController.WaitForTransitionAnimationToEnd();
            Debug.Log("Yield ended");

            if (_currentMode == GameMode.RadarMode)
            {
                RadarModeRoot.EnableMode();
            }
            else
            {
                SineComparisionModeRoot.EnableMode();
            }

            _duringModeChange = false;
        }
    }

    public enum GameMode
    {
        RadarMode, SineMode
    }
}


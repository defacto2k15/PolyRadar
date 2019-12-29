using System;
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
        public GameObject SineComparisionModeCameraPosition;

        public SingleGameModeController RadarModeRoot;
        public GameObject RadarModeCameraPosition;

        private GameMode _currentMode;

        public void Start()
        {
            ChangeMode(GameMode.RadarMode);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (_currentMode == GameMode.RadarMode)
                {
                    ChangeMode(GameMode.SineMode);
                }
                else
                {
                    ChangeMode(GameMode.RadarMode);
                }
            }
        }

        public void ChangeMode(GameMode newMode)
        {
            _currentMode = newMode;

            if (_currentMode == GameMode.RadarMode)
            {
                RadarModeRoot.EnableMode();
                SineComparisionModeRoot.DisableMode();

                Camera.transform.position = RadarModeCameraPosition.transform.position;
                Camera.transform.rotation= RadarModeCameraPosition.transform.rotation;
            }
            else
            {
                SineComparisionModeRoot.EnableMode();
                RadarModeRoot.DisableMode();

                Camera.transform.position = SineComparisionModeCameraPosition.transform.position;
                Camera.transform.rotation= SineComparisionModeCameraPosition.transform.rotation;
            }
        }
    }

    public enum GameMode
    {
        RadarMode, SineMode
    }
}


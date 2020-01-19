using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.OscilloscopeDisplay;
using Assets.Scripts.RadarBattleground;
using UnityEngine;

namespace Assets.Scripts.RadarManipulators
{
    public class AutomaticRadarRotationToggleOC : MonoBehaviour
    {
        [SerializeField] private RadarAutomaticRotationDirectorOC rotationDirector;
        [SerializeField] private PushButtonOC pushButtonOc;

        void Update()
        {
            if (rotationDirector.AutomaticRotationEnabled != pushButtonOc.IsOn)
            {
                pushButtonOc.IsOn = rotationDirector.AutomaticRotationEnabled;
            }
        }

        public void ReactToToggleButtonClicked(bool isOn)
        {
            rotationDirector.AutomaticRotationEnabled = isOn;
        }
    }
}

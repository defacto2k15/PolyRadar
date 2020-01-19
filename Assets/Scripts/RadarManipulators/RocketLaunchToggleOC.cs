using UnityEngine;

namespace Assets.Scripts.RadarManipulators
{
    public class RocketLaunchToggleOC : MonoBehaviour
    {
        [SerializeField] private RocketSpawnerScript rocketSpawner;
        [SerializeField] private PushButtonOC pushButtonOc;

        void Update()
        {
            if(pushButtonOc.IsOn == rocketSpawner.RocketIsPresent)
            {
                pushButtonOc.IsOn = !rocketSpawner.RocketIsPresent;
            }
        }

        public void ReactToToggleButtonClicked(bool isOn)
        {
            if (!rocketSpawner.RocketIsPresent)
            {
                rocketSpawner.SpawnRocket();
            }
        }
    }
}
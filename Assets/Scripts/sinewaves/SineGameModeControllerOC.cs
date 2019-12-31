using UnityEngine;

namespace Assets.Scripts
{
    public class SineGameModeControllerOC : SingleGameModeController
    {
        public GameObject SineUiRoot; 

        public override void DisableMode()
        {
            SineUiRoot.SetActive(false);
        }

        public override void EnableMode()
        {
            SineUiRoot.SetActive(true);
        }
    }
}
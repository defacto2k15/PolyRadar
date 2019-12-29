using UnityEngine;

namespace Assets.Scripts
{
    public abstract class SingleGameModeController : MonoBehaviour
    {
        public abstract void DisableMode();
        public  abstract void EnableMode();
    }
}
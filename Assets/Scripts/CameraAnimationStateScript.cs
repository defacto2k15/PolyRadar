using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraAnimationStateScript : StateMachineBehaviour
    {
        public GameMode AchievedMode;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.gameObject.GetComponent<MainCameraAnimationControllerOC>().ModePlaceWasAchieved(AchievedMode);
        }
    }
}

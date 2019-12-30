using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class MainCameraAnimationControllerOC : MonoBehaviour
    {
        private UnityEvent _stateAchievedEvent;
        private Animator _animator;

        public void Start()
        {
            _stateAchievedEvent = new UnityEvent();
            _animator = GetComponent<Animator>();
        }

        public void MoveToRadar()
        {
            _animator.SetInteger("RequiredPosition", 0);
        }

        public void MoveToSine()
        {
            _animator.SetInteger("RequiredPosition", 1);
        }

        public IEnumerator WaitForTransitionAnimationToEnd()
        {
            return WaitUntilEvent(_stateAchievedEvent);
        }

        public void ModePlaceWasAchieved(GameMode mode)
        {
            _stateAchievedEvent.Invoke();
        }

        private IEnumerator WaitUntilEvent(UnityEvent unityEvent)
        {
            var trigger = false;
            Action action = () => trigger = true;
            unityEvent.AddListener(action.Invoke);
            yield return new WaitUntil(() => trigger);
            unityEvent.RemoveListener(action.Invoke);
        }
    }
}
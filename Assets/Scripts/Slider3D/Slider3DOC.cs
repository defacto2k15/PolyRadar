using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Slider3D
{
    public class Slider3DOC : MonoBehaviour
    {
        [SerializeField] private Transform _parentTransform;
        public float Value;
        public float MinValue;
        public float MaxValue;
        public MyFloatEvent OnValueChanged;

        void Start()
        {
            var initialXPosition = Mathf.Clamp01((Value - MinValue) / (MaxValue - MinValue));
            UpdateXPosition(initialXPosition);
        }

        void OnMouseDrag()
        {
            Plane plane = new Plane(transform.forward, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                var collisionPositionInWorldSpace = ray.GetPoint(distance);
                var collisionPositionInLocalSpace = _parentTransform.worldToLocalMatrix.MultiplyPoint(collisionPositionInWorldSpace);
                var newXPosition = Mathf.Clamp01(collisionPositionInLocalSpace.x);

                UpdateXPosition(newXPosition);

                Value = newXPosition * (MaxValue - MinValue) + MinValue;
                OnValueChanged.Invoke(Value);
            }
        }

        private void UpdateXPosition(float newXPosition)
        {
            transform.localPosition = new Vector3(newXPosition, transform.localPosition.y, transform.localPosition.z);
        }
    }

    [Serializable]
    public class MyFloatEvent : UnityEvent<float>
    {
    }
}

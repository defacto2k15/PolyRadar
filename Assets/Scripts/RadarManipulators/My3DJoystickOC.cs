using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.RadarManipulators
{
    public class My3DJoystickOC : MonoBehaviour
    {
        [SerializeField] private Transform immovableParentTransform;
        [SerializeField] private float extremeRotation;
        [SerializeField] private float positionToRotationMultiplier;
        [SerializeField] private float joystickSpringStrength;
        [SerializeField] private float centerDeadZoneSize;
        [SerializeField] private MyVector2Event OnJoystickUsage;

        private Vector2 _lastMouseOnPlanePosition;
        private bool _joystickIsDragged;

        void OnMouseDown()
        {
            var position = ComputeMousePositionOnPlane();
            _lastMouseOnPlanePosition = position.GetValueOrDefault(Vector2.zero);
            _joystickIsDragged = true;
        }

        void OnMouseUp()
        {
            _joystickIsDragged = false;
        }

        void Update()
        {
            var currentFlatRotation = CurrentFlatRotation();
            if (!_joystickIsDragged)
            {
                var newFlatRotation = new Vector2(Mathf.Lerp(currentFlatRotation.x, 0, Time.deltaTime * joystickSpringStrength),
                    Mathf.Lerp(currentFlatRotation.y, 0, Time.deltaTime * joystickSpringStrength));
                transform.rotation = Quaternion.Euler(newFlatRotation.x, newFlatRotation.y, transform.rotation.eulerAngles.z);
            }

            if (currentFlatRotation.magnitude > centerDeadZoneSize)
            {
                var normalizedJoystickSetting = currentFlatRotation / extremeRotation;
                OnJoystickUsage.Invoke(normalizedJoystickSetting);
            }
        }

        private Vector2? ComputeMousePositionOnPlane()
        {
            var plane = new Plane(transform.position,  immovableParentTransform.up);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            var raycastResult = plane.Raycast(ray, out distance);
            distance = Mathf.Abs(distance);
            if (distance < 0.0001)
            {
                return Vector2.zero;
            }

            var hitPoint = ray.GetPoint(distance);

                var xLocalAxis = immovableParentTransform.up;
                var yLocalAxis = immovableParentTransform.right;

                var x = Vector3.Dot(hitPoint, xLocalAxis)/distance;
                var y = Vector3.Dot(hitPoint, yLocalAxis)/distance;
                return new Vector2(x,y);
            
        }

        void OnMouseDrag()
        {
            Debug.Log("OMD");
            var newMousePosition = ComputeMousePositionOnPlane().GetValueOrDefault(_lastMouseOnPlanePosition);
            Debug.Log("NMP: "+newMousePosition.x+" "+newMousePosition.y);
            var delta = newMousePosition - _lastMouseOnPlanePosition;
            _lastMouseOnPlanePosition = newMousePosition;

            var deltaRotation = delta * positionToRotationMultiplier;
            deltaRotation.y *= -1;

            var oldRotation = CurrentFlatRotation();

            var newEulerRotation = oldRotation+deltaRotation;
            newEulerRotation = new Vector2(Mathf.Clamp(newEulerRotation.x, -extremeRotation, extremeRotation), Mathf.Clamp(newEulerRotation.y, -extremeRotation, extremeRotation));

            transform.rotation = Quaternion.Euler(newEulerRotation.x,  newEulerRotation.y, transform.rotation.eulerAngles.z);
        }

        private Vector2 CurrentFlatRotation()
        {
            var oldRotation = new Vector2(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y);
            if (oldRotation.x > 180)
            {
                oldRotation.x = oldRotation.x - 360;
            }

            if (oldRotation.y > 180)
            {
                oldRotation.y = oldRotation.y - 360;
            }

            return oldRotation;
        }
    }
}

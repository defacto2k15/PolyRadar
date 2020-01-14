using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.OscilloscopeDisplay;
using UnityEngine;

namespace Assets.Scripts.RadarManipulators
{
    public class ManualRadarRotationManipulatorOC : MonoBehaviour
    {
        [SerializeField] private RadarAutomaticRotationDirectorOC _rotationDirector;
        [SerializeField] private float sensitivity = 300f;
        private Vector3 _lastMousePos;

        void OnMouseDown()
        {
            _lastMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            if (_rotationDirector.AutomaticRotationEnabled)
            {
                _rotationDirector.AutomaticRotationEnabled = false;
            }
        }

        void OnMouseDrag()
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            var zAngle = (pos.y - _lastMousePos.y) * sensitivity;
            transform.Rotate(0, 0, zAngle);
            _lastMousePos = pos;
            _rotationDirector.ChangeRotationAngle(zAngle);
        }
    }
}

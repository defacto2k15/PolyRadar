using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.OscilloscopeDisplay;
using Assets.Scripts.RadarBattleground;
using UnityEngine;

namespace Assets.Scripts.RadarManipulators
{
    [RequireComponent(typeof(MaterialPropertyBlockColorSetterOC ))]
    public class AutomaticRadarRotationToggleOC : MonoBehaviour
    {
        [SerializeField] private Color _enabledColor;
        [SerializeField] private Color _disabledColor;

        [SerializeField] private RadarAutomaticRotationDirectorOC _rotationDirector;

        private MaterialPropertyBlockColorSetterOC _colorSetter;
        private bool _wasEnabled = false;

        void Start()
        {
            _colorSetter = GetComponent<MaterialPropertyBlockColorSetterOC>();
        }

        void Update()
        {
            if (_rotationDirector.AutomaticRotationEnabled != _wasEnabled)
            {
                _wasEnabled = _rotationDirector.AutomaticRotationEnabled;
                UpdateColor(_wasEnabled);
            }
        }

        void OnMouseDown()
        {
            var newEnabled = !_rotationDirector.AutomaticRotationEnabled;
            _rotationDirector.AutomaticRotationEnabled = newEnabled;
            UpdateColor(newEnabled);
        }

        private void UpdateColor(bool isEnabled)
        {
            if (isEnabled)
            {
                _colorSetter.ChangeColorAndUpdate(_enabledColor);
            }
            else
            {
                _colorSetter.ChangeColorAndUpdate(_disabledColor);
            }

        }
    }
}

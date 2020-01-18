using Assets.Scripts.RadarBattleground;
using UnityEngine;

namespace Assets.Scripts.RadarManipulators
{
    [RequireComponent(typeof(MaterialPropertyBlockColorSetterOC ))]
    public class PushButtonOC : MonoBehaviour
    {
        [SerializeField] private bool isOn = false;
        [SerializeField] private Color enabledColor;
        [SerializeField] private Color disabledColor;
        [SerializeField] private MyBoolEvent onChangeEvent;

        private MaterialPropertyBlockColorSetterOC _colorSetter;

        public bool IsOn
        {
            get => isOn;
            set
            {
                if (isOn != value)
                {
                    isOn = value;
                    UpdateColor();
                }

            }
        }

        void Start()
        {
            _colorSetter = GetComponent<MaterialPropertyBlockColorSetterOC>();
            UpdateColor();
        }

        void OnMouseDown()
        {
            isOn = !isOn;
            onChangeEvent.Invoke(isOn);

            UpdateColor();
        }

        private void UpdateColor()
        {
            if (isOn)
            {
                _colorSetter.ChangeColorAndUpdate(enabledColor);
            }
            else
            {
                _colorSetter.ChangeColorAndUpdate(disabledColor);
            }

        }
    }
}
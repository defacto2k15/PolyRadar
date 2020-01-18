using Assets.Scripts.RadarBattleground;
using UnityEngine;

namespace Assets.Scripts.RadarManipulators
{
    [RequireComponent(typeof(MaterialPropertyBlockColorSetterOC ))]
    public class HoldButtonOC : MonoBehaviour
    {
        [SerializeField] private Color enabledColor;
        [SerializeField] private Color disabledColor;
        [SerializeField] private MyVoidEvent OnClickedEvent;

        private MaterialPropertyBlockColorSetterOC _colorSetter;

        void Start()
        {
            _colorSetter = GetComponent<MaterialPropertyBlockColorSetterOC>();
            UpdateColor(false);
        }

        void OnMouseDrag()
        {
            UpdateColor(true);
            OnClickedEvent.Invoke();
        }

        void OnMouseUp()
        {
            UpdateColor(false);
        }

        private void UpdateColor(bool isOn)
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
using System.Collections;
using Assets.Scripts.RadarBattleground;
using UnityEngine;

namespace Assets.Scripts.RadarManipulators
{
    [RequireComponent(typeof(MaterialPropertyBlockColorSetterOC ))]
    public class  MomentaryPushButtonOC : MonoBehaviour
    {
        [SerializeField] private Color enabledColor;
        [SerializeField] private Color disabledColor;
        [SerializeField] private MyVoidEvent onClickedEvent;
        private int _colorChangeHandlersCount = 0;

        private MaterialPropertyBlockColorSetterOC _colorSetter;

        void Start()
        {
            _colorSetter = GetComponent<MaterialPropertyBlockColorSetterOC>();
            UpdateColor(false);
        }

        void OnMouseDown()
        {
            onClickedEvent.Invoke();
            StartCoroutine(HandleMomentaryColorChanges());
        }

        private IEnumerator HandleMomentaryColorChanges()
        {
            UpdateColor(true);
            _colorChangeHandlersCount++;
            yield return new WaitForSeconds(0.33f);
            _colorChangeHandlersCount--;
            if (_colorChangeHandlersCount == 0)
            {
                UpdateColor(false);
            }

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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.RadarBattleground;
using UnityEngine;

namespace Assets.Scripts.Vehicles
{
    public class VehicleMarkerOC : MonoBehaviour
    {
        public GameObject DirectionTail;
        public Color DefaultColor;
        public Color SelectedColor;
        private bool _isSelected;

        public void SetMarkerVisible(bool visible)
        {
            GetComponent<MeshRenderer>().enabled = visible;
        }

        public void UpdateDirection(Vector2 movementDelta)
        {
            var angle = Mathf.Atan2(-movementDelta.y, movementDelta.x);
            DirectionTail.transform.rotation = Quaternion.Euler(0, 270+angle*Mathf.Rad2Deg, 0);
        }

        public bool IsSelected  
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                var colorSetter = GetComponent<MaterialPropertyBlockColorSetterOC>();
                if (_isSelected)
                {
                    colorSetter.Color = SelectedColor;
                }
                else
                {
                    colorSetter.Color = DefaultColor;
                }

                colorSetter.UpdateColor();
            }
        }
    }
}

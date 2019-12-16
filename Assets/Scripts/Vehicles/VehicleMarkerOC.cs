using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.RadarBattleground;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Vehicles
{
    public class VehicleMarkerOC : MonoBehaviour
    {
        public GameObject DirectionTail;
        public Color DefaultColor;
        public Color FriendColor;
        public Color FoeColor;
        public Color SelectedColor;
        private bool _isSelected;
        private VehicleAffinity _affinity = VehicleAffinity.Unknown;

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
                Assert.IsTrue(_affinity == VehicleAffinity.Unknown);
                _isSelected = value;
                UpdateColor();
            }
        }

        public bool CanBeSelected => _affinity == VehicleAffinity.Unknown;

        public VehicleAffinity Affinity
        {
            get { return _affinity; }
            set
            {
                _affinity = value;
                _isSelected = false;
                UpdateColor();
            }
        }

        private void UpdateColor()
        {
            var colorSetter = GetComponent<MaterialPropertyBlockColorSetterOC>();
            if (_isSelected)
            {
                colorSetter.Color = SelectedColor;
            }
            else
            {
                if (_affinity == VehicleAffinity.Foe)
                {
                    colorSetter.Color = FoeColor;
                }
                else if (_affinity == VehicleAffinity.Friend)
                {
                    colorSetter.Color = FriendColor;
                }
                else
                {

                    colorSetter.Color = DefaultColor;
                }
            }

            colorSetter.UpdateColor();
        }
    }
}

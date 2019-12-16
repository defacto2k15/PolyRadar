using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.RadarBattleground;
using UnityEngine;

namespace Assets.Scripts.Vehicles
{
    public class VehicleOC : MonoBehaviour
    {
        public Color FriendRadarColor;
        public Color FoeRadarColor;
        public Color UnknownRadarColor;

        private Vector2 _lastFlatPosition;
        private VehicleAffinity _affinity;

        void Start()
        {
            Affinity = VehicleAffinity.Unknown;

        }

        public VehicleAffinity Affinity
        {
            get => _affinity;
            set
            {
                _affinity = value;
                UpdateColor();
            }
        }

        private void UpdateColor()
        {
            var colorChangeComponent = GetComponent<MaterialPropertyBlockColorSetterOC>();
            Color color= Color.magenta;
            if (_affinity == VehicleAffinity.Unknown)
            {
                color = UnknownRadarColor;
            }else if (_affinity == VehicleAffinity.Friend)
            {
                color = FriendRadarColor;
            }else if  (_affinity == VehicleAffinity.Foe)
            {
                color = FoeRadarColor;
            }

            colorChangeComponent.Color = color;
            colorChangeComponent.UpdateColor();
        }

        void Update()
        {
            _lastFlatPosition = new Vector2(transform.position.x, transform.position.z);
        }

        public Vector2 MovementDirection => (new Vector2(transform.position.x, transform.position.z) - _lastFlatPosition).normalized;
    }
}

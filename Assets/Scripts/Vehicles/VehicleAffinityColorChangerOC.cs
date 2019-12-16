using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.RadarBattleground;
using UnityEngine;

namespace Assets.Scripts.Vehicles
{
    public class VehicleAffinityColorChangerOC : MonoBehaviour
    {
        public Color FriendRadarColor;
        public Color FoeRadarColor;
        public Color UnknownRadarColor;

        public void UpdateColorByAffinity(VehicleAffinity affinity)
        {
            var colorChangeComponent = GetComponent<MaterialPropertyBlockColorSetterOC>();
            Color color= Color.magenta;
            if (affinity == VehicleAffinity.Unknown)
            {
                color = UnknownRadarColor;
            }else if (affinity == VehicleAffinity.Friend)
            {
                color = FriendRadarColor;
            }else if  (affinity == VehicleAffinity.Foe)
            {
                color = FoeRadarColor;
            }

            colorChangeComponent.Color = color;
            colorChangeComponent.UpdateColor();
        }
    }
}

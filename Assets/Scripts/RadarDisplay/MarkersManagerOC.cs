using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Vehicles;
using Assets.Scripts.Visibility;
using UnityEngine;

namespace Assets.Scripts.RadarDisplay
{
    public class MarkersManagerOC : MonoBehaviour
    {
        private List<MarkerOC> Markers => GetComponentsInChildren<MarkerOC>().ToList();

        public void ApplyVisibilityPackToAllMarkers(VisibilityChangePack pack)
        {
            Markers.ForEach(c=>c.ApplyMarkerVisiblityPack(pack));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.OscilloscopeDisplay;
using Assets.Scripts.RadarBattleground;
using Assets.Scripts.Visibility;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Vehicles
{
    public class VehicleOC : MonoBehaviour
    {
        public VehicleAffinityColorChangerOC ColorChanger;
        public VehicleMovementTrackerOC MovementTracker;
        public VehicleVisibilityOC Visibility;
        public VehicleMarkerOC Marker;

        private bool _isSelected;
        private VehicleAffinity _affinity;

        void Start()
        {
            Affinity = VehicleAffinity.Unknown;
        }

        public void MyUpdate(Vector2 flatCenter, RadarBeamSetting beamSetting, HeightmapArrayFromWorldSpaceSampler occlusionHeightmapArraySampler)
        {
            Marker.MyUpdate(flatCenter, Visibility.transform.position, MovementTracker.MovementDirection, beamSetting, occlusionHeightmapArraySampler);
        }

        void Update()
        {
            if (!Marker.CanBeVisible)
            {
                IsSelected = false;
            }
        }

        public VehicleAffinity Affinity
        {
            get => _affinity;
            set
            {
                Assert.IsTrue(_affinity == VehicleAffinity.Unknown);
                IsSelected = false;
                _affinity = value;
                ColorChanger.UpdateColorByAffinity(value);
                Marker.UpdateColor(_isSelected, _affinity);
            }
        }

        public void SetVisible(bool isVisible)
        {
            Visibility.SetVisibility(isVisible);
        }
        public bool IsSelected  
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                Marker.UpdateColor(_isSelected, _affinity);
            }
        }

        public bool CanBeSelected => _affinity == VehicleAffinity.Unknown && Marker.CanBeVisible;

        public void ApplyMarkerVisiblityPack(VisibilityChangePack pack)
        {
            Marker.ApplyMarkerVisiblityPack(pack);
        }

        public void WasHit()
        {
            Debug.Log("Plane of affinity "+Affinity+" was destroyed");
            Destroy(this.gameObject);
        }
    }
}

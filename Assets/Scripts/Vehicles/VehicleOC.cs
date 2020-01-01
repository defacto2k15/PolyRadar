using System;
using Assets.Scripts.OscilloscopeDisplay;
using Assets.Scripts.RadarBattleground;
using Assets.Scripts.Visibility;
using UnityEngine;
using UnityEngine.Assertions;
using Random = System.Random;

namespace Assets.Scripts.Vehicles
{
    public class VehicleOC : MonoBehaviour
    {
        public VehicleAffinityColorChangerOC ColorChanger;
        public VehicleMovementTrackerOC MovementTracker;
        public VehicleVisibilityOC Visibility;
        public VehicleMarkerOC Marker;

        public VehicleDetails Details;
        private bool _isSelected;
        private VehicleAffinity _affinity;

        private static int _lastVehicleId;

        private void Start()
        {
            Affinity = VehicleAffinity.Unknown;
            Details = VehicleDetails.GenerateRandomDetails(_lastVehicleId++);
        }

        public void MyUpdate(Vector2 flatCenter, RadarBeamSetting beamSetting, HeightmapArrayFromWorldSpaceSampler occlusionHeightmapArraySampler)
        {
            Marker.MyUpdate(flatCenter, Visibility.transform.position, MovementTracker.MovementDirection, beamSetting, occlusionHeightmapArraySampler);
        }

        private void Update()
        {
            if (!Marker.CanBeVisible)
            {
                IsSelected = false;
            }
        }

        public void SetVisible(bool isVisible)
        {
            Visibility.SetVisibility(isVisible);
        }

        public void ApplyMarkerVisiblityPack(VisibilityChangePack pack)
        {
            Marker.ApplyMarkerVisiblityPack(pack);
        }

        public void WasHit()
        {
            Debug.Log("Plane of affinity " + Affinity + " was destroyed");
            Destroy(this.gameObject);
        }

        public void UnveilAffinity()
        {
            if (Details.KnownAffinity == KnownVehicleAffinity.Friend)
            {
                Affinity = VehicleAffinity.Friend;
            }
            else
            {
                Affinity = VehicleAffinity.Foe;
            }
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
    }

    [Serializable]
    public class VehicleDetails
    {
        public KnownVehicleAffinity KnownAffinity;
        public int ID;

        public static VehicleDetails GenerateRandomDetails(int id)
        {
            var rand = new Random(id);

            var affinity = KnownVehicleAffinity.Foe;
            if (rand.Next(2) == 1)
            {
                affinity = KnownVehicleAffinity.Friend;
            }
            return new VehicleDetails()
            {
                ID = id,
                KnownAffinity = affinity
            };
        }
    }
}
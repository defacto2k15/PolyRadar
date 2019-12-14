using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.OscilloscopeDisplay;
using Assets.Scripts.RadarBattleground;
using Assets.Scripts.Vehicles;
using UnityEngine;

namespace Assets.Scripts.RadarDisplay
{
    public class RadarMarkersManagerOC : MonoBehaviour
    {
        public GameObject VehicleMarkerPrefab;
        public float MarkersYPosition;
        public float DurationToRemoveMarker;
        public BattlegroundVehiclesRootOC VehiclesRoot;

        private Dictionary<DummyFlyingVehicleOC, MarkerWithLastPingTime> _makers = new Dictionary<DummyFlyingVehicleOC, MarkerWithLastPingTime>();

        public void SetAllMarkersVisibility(bool visibility)
        {
            _makers.Values.Select(c=>c.Marker).ToList().ForEach(c=> c.SetMarkerVisible(visibility));
        }

        public void UpdateWithBeamSetting(RadarBeamSetting beamSetting, HeightmapArrayFromWorldSpaceSampler occlusionHeightmapArraySampler)
        {
            var flatCenter = new Vector2(transform.position.x, transform.position.z);
            var vehicles = VehiclesRoot.AllVehicles;

            var vehiclesInBeam = vehicles.Where(c => VehicleIsInBeam(beamSetting, c, flatCenter) && VehicleIsNotOccluded(occlusionHeightmapArraySampler, c)).ToList();

            foreach (var aVehicle in vehiclesInBeam)
            {
                if (!_makers.ContainsKey(aVehicle))
                {
                    var newMarker = Instantiate(VehicleMarkerPrefab);
                    _makers[aVehicle] = new MarkerWithLastPingTime()
                    {
                        Marker = newMarker.GetComponent<VehicleMarkerOC>(),
                        LastPingTime = Time.time
                    };
                }

                var newPosition = new Vector3(aVehicle.transform.position.x, MarkersYPosition, aVehicle.transform.position.z);
                var marker = _makers[aVehicle].Marker;
                marker.transform.position = newPosition;
                marker.UpdateDirection(aVehicle.MovementDirection);
                marker.transform.parent = transform;
                _makers[aVehicle].LastPingTime = Time.time;
            }

            var vehiclesOfMarkersToDelete = _makers.Where(c => Time.time - c.Value.LastPingTime > DurationToRemoveMarker).Select(c => c.Key).ToList();
            vehiclesOfMarkersToDelete.ForEach(c=>
            {
                GameObject.Destroy(_makers[c].Marker.gameObject);
                _makers.Remove(c);
            });
        }

        private static bool VehicleIsInBeam(RadarBeamSetting beamSetting, DummyFlyingVehicleOC vehicle, Vector2 flatCenter)
        {
            var flatDelta = new Vector2(vehicle.transform.position.x, vehicle.transform.position.z) - flatCenter;
            var thisMarkerAngle = Mathf.Atan2(flatDelta.y, flatDelta.x) * Mathf.Rad2Deg;
            return beamSetting.AngleIsInRange(thisMarkerAngle);
        }

        private static bool VehicleIsNotOccluded(HeightmapArrayFromWorldSpaceSampler heightmapSampler, DummyFlyingVehicleOC vehicle)
        {
            var vehicleHeight = vehicle.transform.position.y;
            var occlusionHeight = heightmapSampler.Sample(new Vector2(vehicle.transform.position.x, vehicle.transform.position.z));
            return vehicleHeight >= occlusionHeight;
        }

        public List<VehicleWithMarkerPair> VehicleMarkerPairs => _makers.Select(c => new VehicleWithMarkerPair(){ Marker = c.Value.Marker, Vehicle = c.Key}).ToList();
    }

    public class VehicleWithMarkerPair
    {
        public DummyFlyingVehicleOC Vehicle;
        public VehicleMarkerOC Marker;
    }

    public class MarkerWithLastPingTime
    {
        public VehicleMarkerOC Marker;
        public float LastPingTime;
    }
}

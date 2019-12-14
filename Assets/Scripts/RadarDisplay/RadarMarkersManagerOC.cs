using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.OscilloscopeDisplay;
using Assets.Scripts.Vehicles;
using UnityEngine;

namespace Assets.Scripts.RadarDisplay
{
    public class RadarMarkersManagerOC : MonoBehaviour
    {
        public GameObject VehicleMarkerPrefab;
        public float MarkersYPosition;
        public BattlegroundVehiclesRootOC VehiclesRoot;

        private Dictionary<DummyFlyingVehicleOC, VehicleMarkerOC> _makers = new Dictionary<DummyFlyingVehicleOC, VehicleMarkerOC>();

        public void UpdateWithBeamSetting(RadarBeamSetting beamSetting)
        {
            var flatCenter = new Vector2(transform.position.x, transform.position.z);
            var vehicles = VehiclesRoot.AllVehicles;

            var vehiclesInBeam = vehicles.Where(c =>
            {
                var flatDelta = new Vector2(c.transform.position.x, c.transform.position.z) - flatCenter;
                var thisMarkerAngle = Mathf.Atan2(flatDelta.y, flatDelta.x) * Mathf.Rad2Deg;
                return beamSetting.AngleIsInRange(thisMarkerAngle);
            }).ToList();

            foreach (var aVehicle in vehiclesInBeam)
            {
                if (!_makers.ContainsKey(aVehicle))
                {
                    var newMarker = Instantiate(VehicleMarkerPrefab);
                    _makers[aVehicle] = newMarker.GetComponent<VehicleMarkerOC>();
                }

                var newPosition = new Vector3(aVehicle.transform.position.x, MarkersYPosition, aVehicle.transform.position.z);
                _makers[aVehicle].transform.position = newPosition;
                _makers[aVehicle].UpdateDirection(aVehicle.MovementDirection);
            }
        }

        public List<VehicleWithMarkerPair> VehicleMarkerPairs => _makers.Select(c => new VehicleWithMarkerPair(){ Marker = c.Value, Vehicle = c.Key}).ToList();
    }

    public class VehicleWithMarkerPair
    {
        public DummyFlyingVehicleOC Vehicle;
        public VehicleMarkerOC Marker;
    }
}

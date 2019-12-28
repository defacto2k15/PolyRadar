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
    public class VehiclesSelectingManagerOC : MonoBehaviour
    {
        public BattlegroundVehiclesRootOC VehiclesRoot;

        public bool HasSelectableVehicle => Vehicles.Any(c=>c.CanBeSelected);
        public bool HasSelectedVehicle => Vehicles.Any(c => c.IsSelected);

        public VehicleOC SelectedVehicle => Vehicles.First(c => c.IsSelected);

        public void ChangeVehicleSelection(int selectedMarkerOffset)
        {
            var vehiclesList = Vehicles.Where(c=>c.CanBeSelected).ToList();
            int selectedMarkerIndex;
            var selectedVehicle = vehiclesList.Select((c, i) =>new {c, i}).FirstOrDefault(c => c.c.IsSelected);
            if (selectedVehicle == null)
            {
                selectedMarkerIndex = 0;
            }
            else
            {
                selectedVehicle.c.IsSelected = false;
                selectedMarkerIndex = selectedVehicle.i;
            }

            vehiclesList[(vehiclesList.Count + selectedMarkerIndex + selectedMarkerOffset) % vehiclesList.Count].IsSelected = true;
        }

        public void SetVehicleAffinity(VehicleOC vehicle, VehicleAffinity affinity)
        {
            vehicle.Affinity = affinity;
        }

        private List<VehicleOC> Vehicles => VehiclesRoot.AllVehicles;
    }

    public class VehicleWithMarkerPair
    {
        public VehicleOC Vehicle;
        public VehicleMarkerOC Marker;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.RadarDisplay;
using Assets.Scripts.Vehicles;
using UnityEngine;

namespace Assets.Scripts
{
    public class UserInputControllerOC : MonoBehaviour
    {
        public RadarMarkersManagerOC MarkersManager;

        public void Update()
        {
            var markerPairs = MarkersManager.VehicleMarkerPairs;
            int selectedMarkerIndex = 0;

            int selectedMarkerOffset = 0;
            if (Input.GetKeyDown(KeyCode.A))
            {
                selectedMarkerOffset=1;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                selectedMarkerOffset=-1;
            }

            if (selectedMarkerOffset != 0)
            {
                if (markerPairs.Any())
                {
                    var selectedMarker = markerPairs.Select((c, i) => new {c, i}).FirstOrDefault(c => c.c.Marker.IsSelected);
                    if (selectedMarker == null)
                    {
                        selectedMarkerIndex = 0;
                    }
                    else
                    {
                        selectedMarker.c.Marker.IsSelected = false;
                        selectedMarkerIndex = selectedMarker.i;
                    }

                    markerPairs[( markerPairs.Count + selectedMarkerIndex + selectedMarkerOffset) % markerPairs.Count].Marker.IsSelected = true;
                }
            }
        }
    }
}

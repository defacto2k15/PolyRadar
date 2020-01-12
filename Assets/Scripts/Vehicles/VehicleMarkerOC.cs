using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.OscilloscopeDisplay;
using Assets.Scripts.RadarBattleground;
using Assets.Scripts.Sound;
using Assets.Scripts.Visibility;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Vehicles
{
    [RequireComponent(typeof(VehicleSoundOC))]
    public class VehicleMarkerOC : MarkerOC
    {
        public TimeProviderGo TimeProvider;

        public GameObject DirectionTail;
        public float DurationToDisappearMarker;
        public Color DefaultColor;
        public Color FriendColor;
        public Color FoeColor;
        public Color SelectedColor;

        private float _lastPingTime;
        private VehicleSoundOC _sound;

        void Start()
        {
            _sound = GetComponent<VehicleSoundOC>();
            Debug.Log("STR: "+_sound);
        }

        private void UpdateDirection(Vector2 movementDelta)
        {
            var angle = Mathf.Atan2(-movementDelta.y, movementDelta.x);
            DirectionTail.transform.rotation = Quaternion.Euler(0, 270+angle*Mathf.Rad2Deg, 0);
        }

        public void MyUpdate(Vector2 flatCenter, Vector3 vehiclePosition, Vector2 movementDelta, RadarBeamSetting beamSetting, HeightmapArrayFromWorldSpaceSampler occlusionHeightmapArraySampler)
        {
            if (VehicleIsInBeam(beamSetting, vehiclePosition, flatCenter) && VehicleIsNotOccluded(occlusionHeightmapArraySampler, vehiclePosition))
            {
                UpdateDirection(movementDelta);
                transform.position = new Vector3(vehiclePosition.x, transform.position.y, vehiclePosition.z);
                _lastPingTime = TimeProvider.TimeSinceStart;
                ApplyMarkerVisiblityPack(new VisibilityChangePack(){ChangingObject = this, Visibility = true});
                _sound.StartBlipSound();
            }
        }

        void Update()
        {
            if (TimeProvider.TimeSinceStart - _lastPingTime > DurationToDisappearMarker)
            {
                ApplyMarkerVisiblityPack(new VisibilityChangePack(){ChangingObject = this, Visibility = false});
            }
        }


        private static bool VehicleIsInBeam(RadarBeamSetting beamSetting, Vector3 vehiclePosition, Vector2 flatCenter)
        {
            var flatDelta = new Vector2(vehiclePosition.x, vehiclePosition.z) - flatCenter;
            var thisMarkerAngle = Mathf.Atan2(flatDelta.y, flatDelta.x) * Mathf.Rad2Deg;
            return beamSetting.AngleIsInRange(thisMarkerAngle);
        }

        private static bool VehicleIsNotOccluded(HeightmapArrayFromWorldSpaceSampler heightmapSampler, Vector3 vehiclePosition)
        {
            var vehicleHeight = vehiclePosition.y;
            var occlusionHeight = heightmapSampler.Sample(new Vector2(vehiclePosition.x, vehiclePosition.z));
            return vehicleHeight >= occlusionHeight;
        }


        public void UpdateColor(bool isSelected, VehicleAffinity affinity)
        {
            var colorSetter = GetComponent<MaterialPropertyBlockColorSetterOC>();
            if (isSelected)
            {
                colorSetter.Color = SelectedColor;
            }
            else
            {
                if (affinity == VehicleAffinity.Foe)
                {
                    colorSetter.Color = FoeColor;
                }
                else if (affinity == VehicleAffinity.Friend)
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

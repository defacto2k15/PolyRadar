using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.OscilloscopeDisplay;
using Assets.Scripts.RadarBattleground;
using Assets.Scripts.Sound;
using UnityEngine;

namespace Assets.Scripts.Vehicles
{
    public class BattlegroundVehiclesRootOC : MonoBehaviour
    {
        public TimeProviderGo TimeProvider;
        public SoundSourceMasterOC SoundSourceMaster;

        public void Start()
        {
            GetComponentsInChildren<VehicleMarkerOC>().ToList().ForEach(c =>
            {
                c.TimeProvider = TimeProvider;
            });

            GetComponentsInChildren<VehicleSoundOC>().ToList().ForEach(c=> c.SoundMaster=SoundSourceMaster);
        }

        public void SetVehiclesEnabled (bool isEnabled)
        {
            GetComponentsInChildren<VehicleOC>().ToList().ForEach(c => c.enabled = isEnabled);
        }
        public void SetVehiclesVisible(bool isVisible)
        {
            GetComponentsInChildren<VehicleOC>().ToList().ForEach(c => c.SetVisible(isVisible));
        }

        public List<VehicleOC> AllVehicles => GetComponentsInChildren<VehicleOC>().ToList();

        public void UpdateWithBeamSetting(RadarBeamSetting radarBeamSetting, HeightmapArrayFromWorldSpaceSampler occlusionHeightmapArraySampler)
        {
            AllVehicles.ForEach(c=>c.MyUpdate(new Vector2(transform.position.x,transform.position.z), radarBeamSetting, occlusionHeightmapArraySampler ));
        }
    }
}

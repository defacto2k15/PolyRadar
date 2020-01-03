using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.OscilloscopeDisplay;
using UnityEngine;

namespace Assets.Scripts.RadarDisplay
{
    public class DistanceCameraOC : MonoBehaviour
    {
        public RadarTextureManagerOC TextureManager;
        public Shader DistanceToRadarShader;
        public Camera DistanceCamera;

        public void Start()
        {
            Shader.SetGlobalVector("_GlobalRadarPosition", transform.position);
            DistanceCamera.SetReplacementShader(DistanceToRadarShader, "");
        }

        public void Update()
        {
            transform.rotation = Quaternion.Euler(0, -TextureManager.BeamAngleInDegrees + 90, 0);
            //DistanceCamera.enabled = true;
            //DistanceCamera.RenderWithShader(DistanceToRadarShader, "");
            //DistanceCamera.enabled = false;
        }
    }
}

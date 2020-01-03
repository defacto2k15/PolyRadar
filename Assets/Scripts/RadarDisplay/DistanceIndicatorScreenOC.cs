using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.RadarDisplay
{
    public class DistanceIndicatorScreenOC : MonoBehaviour
    {
        [SerializeField] private Material LineRenderingMaterial;
        [SerializeField] private RenderTexture DistanceLineTexture;

        public void Update()
        {
            Graphics.Blit(RenderTexture.active, DistanceLineTexture, LineRenderingMaterial);
        }

    }
}

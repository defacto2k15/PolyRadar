using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Vehicles
{
    public class VehicleVisibilityOC : MonoBehaviour
    {
        public MeshRenderer Renderer;
        public void  SetVisibility(bool isVisible)
        {
            Renderer.enabled = isVisible;
        }
    }
}

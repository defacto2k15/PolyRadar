using System.Linq;
using Assets.Scripts.Visibility;
using UnityEngine;

namespace Assets.Scripts.Vehicles
{
    public class MarkerOC : MonoBehaviour
    {
        private VisibilityDecider _visibilityDecider = new VisibilityDecider();

        public bool ApplyMarkerVisiblityPack(VisibilityChangePack pack)
        {
            var isVisible = _visibilityDecider.ApplyVisibilityChangePack(pack);
            GetComponentsInChildren<MeshRenderer>().ToList().ForEach(c => c.enabled = isVisible);
            return isVisible;
        }

        public bool CanBeVisible => _visibilityDecider.FromPackOf(this);
    }
}
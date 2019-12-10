using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.RadarBattleground
{
    public class StaticBattlegroundPropsRootScriptOC : MonoBehaviour
    {
        public Material BlackMaterial;
        public Material BrightMarginMaterial;

        public void EnableBlackMaterial()
        {
            SetMaterialToAllProps(BlackMaterial);
        }

        public void EnableBrightMarginMaterial()
        {
            SetMaterialToAllProps(BrightMarginMaterial);
        }

        private void SetMaterialToAllProps(Material mat)
        {
            GetComponentsInChildren<MeshRenderer>().ToList().ForEach(c => { c.material = mat; });
            GetComponentsInChildren<MaterialPropertyBlockColorSetterOC>().ToList().ForEach(c => { c.UpdateColor(); });
        }
    }
}

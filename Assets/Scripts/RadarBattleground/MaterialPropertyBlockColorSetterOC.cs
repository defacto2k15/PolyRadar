using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.RadarBattleground
{
    [ ExecuteInEditMode ]
    public class MaterialPropertyBlockColorSetterOC : MonoBehaviour
    {
        public Color Color;
        private MaterialPropertyBlock _propBlock;
        private Renderer _renderer;

        void Awake()
        {
            AffirmFieldArePresent();
        }

        void Update()
        {
            AffirmFieldArePresent();
            _renderer.GetPropertyBlock(_propBlock);
            _propBlock.SetColor("_Color", Color);
            _renderer.SetPropertyBlock(_propBlock);
        }

        private void AffirmFieldArePresent()
        {
            if (_renderer == null)
            {
                _renderer = GetComponent<Renderer>();
            }
            if (_propBlock == null)
            {
                _propBlock = new MaterialPropertyBlock();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Vehicles;
using UnityEngine;

namespace Assets.Scripts.Visibility
{
    public class VisibilityDecider
    {
        private Dictionary<MonoBehaviour, bool> _packs = new Dictionary<MonoBehaviour, bool>();

        public bool ApplyVisibilityChangePack(VisibilityChangePack pack)
        {
            _packs[pack.ChangingObject] = pack.Visibility;
            return _packs.Values.All(c => c);
        }

        public bool FromPackOf(MonoBehaviour mb)
        {
            if (!_packs.ContainsKey(mb))
            {
                return false;
            }

            return _packs[mb];
        }
    }

    public class VisibilityChangePack
    {
        public MonoBehaviour ChangingObject;
        public bool Visibility;
    }
}

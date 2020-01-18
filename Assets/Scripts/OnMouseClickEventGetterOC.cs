using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.RadarManipulators;
using UnityEngine;

namespace Assets.Scripts
{
    public class OnMouseClickEventGetterOC : MonoBehaviour
    {
        [SerializeField] private MyVoidEvent OnClickEvent;

        void OnMouseDown()
        {
            OnClickEvent.Invoke();
        }
    }
}

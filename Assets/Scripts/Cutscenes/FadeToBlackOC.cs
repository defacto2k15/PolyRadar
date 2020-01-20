using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Cutscenes
{
    [RequireComponent(typeof(Image))]
    class FadeToBlackOC : MonoBehaviour
    {
        [SerializeField] private float alpha;
        private Image _image;

        void Start()
        {
            _image = GetComponent<Image>();
        }

        void Update()
        {
            _image.color = new Color(0,0,0, alpha);
        }
    }
}

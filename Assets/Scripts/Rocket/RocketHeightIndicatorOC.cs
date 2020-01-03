using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Rocket
{
    public class RocketHeightIndicatorOC : MonoBehaviour
    {
        [Range(0, 5)] [SerializeField] private float HarmonizationSpeed;
        [SerializeField] private Vector2 AvalibleHeightRange;

        public void Update()
        {
            float targetHeight = 0;
            var rocket = FindObjectOfType<RocketScript>();
            if (rocket != null)
            {
                targetHeight = (rocket.transform.position.y - AvalibleHeightRange.x) / (AvalibleHeightRange.y - AvalibleHeightRange.x);
            }

            float height = Mathf.Lerp(transform.localPosition.y, targetHeight, Time.deltaTime * HarmonizationSpeed);
            transform.localPosition= new Vector3(transform.localPosition.x, height, transform.localPosition.z);
        }

    }
}

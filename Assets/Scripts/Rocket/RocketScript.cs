using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Rocket
{
    public class RocketScript : MonoBehaviour
    {
        public Vector3 velocity;
        public float speed = 0.01f;
        public float rotationSpeed = 0.5f;

        void Update()
        {
            var rocket1 = this.gameObject;
            if (rocket1 != null)
            {
                Vector3 newPosition;
                var z = Vector3.forward;
                if (Input.GetKey("left"))
                {
                    velocity = Quaternion.AngleAxis(rotationSpeed, z) * velocity;
                    rocket1.transform.rotation = Quaternion.AngleAxis(rotationSpeed, z) * rocket1.transform.rotation;
                }

                if (Input.GetKey("right"))
                {
                    velocity = Quaternion.AngleAxis(-rotationSpeed, z) * velocity;
                    rocket1.transform.rotation = Quaternion.AngleAxis(-rotationSpeed, z) * rocket1.transform.rotation;
                }

                if (Input.GetKey("up"))
                {
                    newPosition = rocket1.transform.position + 2 * velocity;
                }
                else
                {
                    newPosition = rocket1.transform.position + velocity;
                }

                rocket1.transform.position = newPosition;
                if (Input.GetKey("down"))
                {
                    Destroy(rocket1);
                }
            }
        }

        public void SetVelocity(Vector3 newVelocity)
        {
            velocity = newVelocity * speed;
        }
    }
}

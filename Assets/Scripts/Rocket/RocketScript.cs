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
                Vector3 newPosition;
                var z = Vector3.up;
                if (Input.GetKey("left"))
                {
                    velocity = Quaternion.AngleAxis(rotationSpeed, z) * velocity;
                    transform.rotation = Quaternion.AngleAxis(rotationSpeed, z) *transform.rotation;
                }

                if (Input.GetKey("right"))
                {
                    velocity = Quaternion.AngleAxis(-rotationSpeed, z) * velocity;
                    transform.rotation = Quaternion.AngleAxis(-rotationSpeed, z) *transform.rotation;
                }

                if (Input.GetKey("up"))
                {
                    newPosition =transform.position + 2 * velocity;
                }
                else
                {
                    newPosition =transform.position + velocity;
                }

                transform.position = newPosition;
                if (Input.GetKey("down"))
                {
                    Destroy(this.gameObject);
                }
        }

        public void SetVelocity(Vector3 newVelocity)
        {
            velocity = newVelocity * speed;
            transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);
        }
    }
}

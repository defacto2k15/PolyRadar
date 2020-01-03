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
        public float HeightChangeSpeed = 1;

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

                newPosition =transform.position + velocity;


                if (Input.GetKey("up"))
                {
                    newPosition += new Vector3(0, HeightChangeSpeed*Time.deltaTime, 0); 
                }
                if (Input.GetKey("down"))
                {
                    newPosition += new Vector3(0, -HeightChangeSpeed*Time.deltaTime, 0); 
                }

                if (Input.GetKeyDown(KeyCode.M))
                {
                    Destroy(this.gameObject);
                }
                transform.position = newPosition;
        }

        public void SetVelocity(Vector3 newVelocity)
        {
            velocity = newVelocity * speed;
            transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);
        }
    }
}

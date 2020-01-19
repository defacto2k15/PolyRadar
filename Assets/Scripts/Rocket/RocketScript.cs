using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Rocket
{
    [RequireComponent(typeof(RocketSoundOC))]
    public class RocketScript : MonoBehaviour
    {
        public Vector3 velocity;
        public float speed = 0.01f;
        public float rotationSpeed = 0.5f;
        public float HeightChangeSpeed = 1;
        private RocketSoundOC _sound;

        public void Start()
        {
            _sound = GetComponent<RocketSoundOC>();
            _sound.StartIdleSound();
            _sound.StartRocketStartSound();
        }

        void Update()
        {
                transform.position =transform.position + velocity;

                if (Input.GetKey("right"))
                {
                    TurnRocket(1);
                }

                if (Input.GetKey("left"))
                {
                    TurnRocket(-1);
                }
                if (Input.GetKey("up"))
                {
                    MoveUp(1);
                }
                if (Input.GetKey("down"))
                {
                    MoveDown(1);
                }

                if (Input.GetKeyDown(KeyCode.M))
                {
                    Destroy(this.gameObject);
                }
        }

        private void MoveDown(float magnitude)
        {
            transform.position += new Vector3(0, -HeightChangeSpeed * Time.deltaTime, 0)*magnitude;
        }

        private void MoveUp(float magnitude)
        {
            transform.position += new Vector3(0, HeightChangeSpeed * Time.deltaTime, 0)*magnitude;
        }

        private void TurnRocket(float magnitude)
        {
            var z = Vector3.up;
            var oldVelocityMagnitude = velocity.magnitude;
            velocity = Quaternion.AngleAxis(rotationSpeed*magnitude, z) * velocity;
            velocity *=  oldVelocityMagnitude/velocity.magnitude ;
            transform.rotation = Quaternion.AngleAxis(rotationSpeed*magnitude, z) * transform.rotation;
        }

        public void SetVelocity(Vector3 newVelocity)
        {
            velocity = newVelocity * speed;
            transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);
        }

        public void Steer(Vector2 rocketMovementChange)
        {
            if (rocketMovementChange.x > 0)
            {
                MoveUp(rocketMovementChange.x);
            }
            else
            {
                MoveDown(-rocketMovementChange.x);
            }

            TurnRocket(rocketMovementChange.y*-1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour
{
    public float speed = 0.01f;
    public float rotationSpeed = 0.5f;
    public GameObject rocketSpawner;
    public int startDirectionX, startDirectionY, startPositionZ;
    GameObject rocket = null;
    Vector3 velocity;
    Vector3 _Z, _Y, _X, _0;
    


    void Start()
    {
        _Z = new Vector3(0, 0, 1);
        _X = new Vector3(1, 0, 0);
        _Y = new Vector3(0, 1, 0);
        _0 = new Vector3(0, 0, 0);
        if (startDirectionX == 0 && startDirectionY == 0) velocity = _Y;
        else
        {
            velocity = new Vector3(startDirectionX, startDirectionY, 0);
            velocity.Normalize();
        }
        velocity *= speed;
    }

    


    void Update()
    {
        if (rocket != null)
        {
            Vector3 newPosition;
            if (Input.GetKey("left"))
            {
                velocity = Quaternion.AngleAxis(rotationSpeed, _Z) * velocity;
                rocket.transform.rotation = Quaternion.AngleAxis(rotationSpeed, _Z) * rocket.transform.rotation;
            }
            if (Input.GetKey("right"))
            {
                velocity = Quaternion.AngleAxis(-rotationSpeed, _Z) * velocity;
                rocket.transform.rotation = Quaternion.AngleAxis(-rotationSpeed, _Z) * rocket.transform.rotation;
            }
            if (Input.GetKey("up"))
            {
                newPosition = rocket.transform.position + 2 * velocity;
            }
            else
            {
                newPosition = rocket.transform.position + velocity;
            }
            rocket.transform.position = newPosition;
            if (Input.GetKey("down"))
            {
                Destroy(rocket);
                rocket = null;
            }
        }

        if (rocket == null)
        {
            if (Input.GetKey("space"))
            {
                rocket = GameObject.CreatePrimitive(PrimitiveType.Cube);
                rocket.name = "Rocket";
                rocket.transform.position = rocketSpawner.transform.position + new Vector3(0,0,-rocketSpawner.transform.position.z + startPositionZ);
                if (startDirectionX == 0 && startDirectionY == 0) velocity = _Y;
                else
                {
                    velocity = new Vector3(startDirectionX, startDirectionY, 0);
                    velocity.Normalize();
                }
                velocity *= speed;
            }
        }
    }
}

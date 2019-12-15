using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Rocket;
using UnityEngine;

public class RocketSpawnerScript : MonoBehaviour
{
    public GameObject RocketPrefab;
    public int startDirectionX, startDirectionY, startPositionZ;
    GameObject rocket = null;
    Vector3 velocity;
    
    void Start()
    {
        if (startDirectionX == 0 && startDirectionY == 0) velocity = Vector3.up;
        else
        {
            velocity = new Vector3(startDirectionX, startDirectionY, 0);
            velocity.Normalize();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            rocket = Instantiate(RocketPrefab);
            rocket.transform.SetParent(transform);
            rocket.name = "Rocket";
            rocket.transform.position = transform.position + new Vector3(0, 0, -transform.position.z + startPositionZ);
            if (startDirectionX == 0 && startDirectionY == 0) velocity = Vector3.up;
            else
            {
                velocity = new Vector3(startDirectionX, startDirectionY, 0);
                velocity.Normalize();
            }

            rocket.GetComponent<RocketScript>().SetVelocity(velocity);
        }
    }
}

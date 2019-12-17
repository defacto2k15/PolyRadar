using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Rocket;
using UnityEngine;

public class RocketSpawnerScript : MonoBehaviour
{
    public bool EnableDebugRocketSpawn = true;
    public GameObject RocketPrefab;
    public int startDirectionX, startDirectionY, startPositionZ;
    GameObject rocket = null;
    Vector3 velocity;
    
    void Start()
    {
        if (startDirectionX == 0 && startDirectionY == 0) velocity = Vector3.forward;
        else
        {
            velocity = new Vector3(startDirectionX, 0, startDirectionY);
            velocity.Normalize();
        }
    }

    void Update()
    {
        if(EnableDebugRocketSpawn && Input.GetKeyDown("space"))
        {
            SpawnRocket();
        }
    }

    public void SpawnRocket()
    {
        rocket = Instantiate(RocketPrefab);
        rocket.transform.SetParent(transform);
        rocket.name = "Rocket";
        rocket.transform.position = transform.position + new Vector3(0, startPositionZ, 0);
        if (startDirectionX == 0 && startDirectionY == 0) velocity = Vector3.forward;
        else
        {
            velocity = new Vector3(startDirectionX, 0, startDirectionY);
            velocity.Normalize();
        }

        rocket.GetComponent<RocketScript>().SetVelocity(velocity);
    }
}

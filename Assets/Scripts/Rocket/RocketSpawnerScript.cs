using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Rocket;
using UnityEngine;

public class RocketSpawnerScript : MonoBehaviour
{
    public bool EnableDebugRocketSpawn = true;
    public GameObject RocketPrefab;
    public GameObject RocketStartMarker;
    public float RocketStartAngle;
    GameObject rocket = null;

    void Update()
    {
        if (EnableDebugRocketSpawn && Input.GetKeyDown("space"))
        {
            SpawnRocket();
        }

        if (Input.GetKey(KeyCode.T))
        {
            RocketStartAngle += 0.01f;
        }
        else if (Input.GetKey(KeyCode.Y))
        {
            RocketStartAngle -= 0.01f;
        }

        var oldRotation = RocketStartMarker.transform.rotation.eulerAngles;
        RocketStartMarker.transform.rotation = Quaternion.Euler(oldRotation.x, RocketStartAngle*Mathf.Rad2Deg*-1, oldRotation.z);
    }

    public void SpawnRocket()
    {
        rocket = Instantiate(RocketPrefab);
        rocket.transform.SetParent(transform);
        rocket.transform.localPosition= Vector3.zero;
        rocket.name = "Rocket";

        var initialVelocity2D = RadianToVector2(RocketStartAngle+Mathf.PI*0.5f);
        var velocity = new Vector3(initialVelocity2D.x,0, initialVelocity2D.y);
        rocket.GetComponent<RocketScript>().SetVelocity(velocity);
    }

    private static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
}

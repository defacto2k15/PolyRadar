﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Rocket;
using Assets.Scripts.Sound;
using Assets.Scripts.Vehicles;
using UnityEngine;

public class RocketSpawnerScript : MonoBehaviour
{
    public SoundSourceMasterOC SoundSourceMaster;
    public GameObject RocketPrefab;
    public GameObject RocketStartMarker;
    public float RocketStartAngle;
    private bool _inputEnabled;

    void Update()
    {
        if (_inputEnabled)
        {
            if (Input.GetKeyDown("space"))
            {
                if (FindObjectOfType<RocketScript>() == null)
                {
                    SpawnRocket();
                }
            }

            if (Input.GetKey(KeyCode.T))
            {
                SoundSourceMaster.PlaySustainedSound(SustainedSoundKind.RocketDirectionChange);
                RocketStartAngle += 0.01f;
            }
            else if (Input.GetKey(KeyCode.Y))
            {
                SoundSourceMaster.PlaySustainedSound(SustainedSoundKind.RocketDirectionChange);
                RocketStartAngle -= 0.01f;
            }

            var oldRotation = RocketStartMarker.transform.rotation.eulerAngles;
            RocketStartMarker.transform.rotation = Quaternion.Euler(oldRotation.x, RocketStartAngle * Mathf.Rad2Deg * -1, oldRotation.z);
        }
    }

    private void SpawnRocket()
    {
        var rocket = Instantiate(RocketPrefab);
        rocket.transform.SetParent(transform);
        rocket.transform.localPosition= new Vector3(0,0.1f, 0);
        rocket.name = "Rocket";

        var initialVelocity2D = RadianToVector2(RocketStartAngle+Mathf.PI*0.5f);
        var velocity = new Vector3(initialVelocity2D.x,0, initialVelocity2D.y);
        rocket.GetComponent<RocketScript>().SetVelocity(velocity);
        rocket.GetComponent<RocketSoundOC>().SoundMaster = SoundSourceMaster;
    }


    private static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public void ChangeInputEnabled(bool isEnabled)
    {
        _inputEnabled = isEnabled;
    }
}

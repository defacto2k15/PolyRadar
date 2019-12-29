using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineComparatorScript : MonoBehaviour
{
    public SinewaveScript target, response;
    public bool matching, match;
    public float Epsilon = 0.2f;
    public float Tolerance = 0.25f;
    public float AttunementFrames = 10;
    public Color dotColorSuccess, dotColorNeutral;

    int frameCount = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int sum = 0;
        print(target.getDotPosition(0).y + " " + response.getDotPosition(0).y);
        for (int i = 0; i < target.DotsCount; i++)
        {
            if (Mathf.Abs(target.getDotPosition(i).y - response.getDotPosition(i).y) < Epsilon)
            {
                sum++;
                response.setDotColor(i, dotColorSuccess);
            }
            else
            {
                response.setDotColor(i, dotColorNeutral);
            }
        }
        if (target.DotsCount-sum/target.DotsCount < Tolerance)
        {
            frameCount++;
            matching = true;
        }
        else
        {
            frameCount = 0;
            matching = false;
        }
        if (frameCount >= AttunementFrames) match = true;
        else match = false;
    } 
}

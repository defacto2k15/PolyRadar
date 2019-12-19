using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineComparatorScript : MonoBehaviour
{
    public SinewaveScript target, response;
    public bool match;
    public float Epsilon = 0.1f;
    public Color dotColorSuccess, dotColorNeutral;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //print(target.getDotPosition(0).y + " " + response.getDotPosition(0).y);
        for (int i = 0; i < target.DotsCount; i++)
        {
            if (Mathf.Abs(target.getDotPosition(i).y - response.getDotPosition(i).y) < Epsilon)
            {
                response.setDotColor(i, dotColorSuccess);
            }
            else
            {
                response.setDotColor(i, dotColorNeutral);
            }
        }
    } 
}

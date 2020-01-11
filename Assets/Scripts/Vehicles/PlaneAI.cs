using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class PlaneAI : MonoBehaviour
{
    private Rigidbody rb;
    private int previous_state = 0;
    private int mode = 0;
    private int timer = 0;
    private float rotor = 0;
    private float rotation = 0;
    private System.Random randomGenerator;

    private static int LastPlaneIndex = 0;

    public TimeProviderGo TimeProvider;
    public Vector3 r_center = new Vector3(30, 0.2f, 30);
    public float r_radius = 5;
    public float inner_radius = 1;
    public float outer_radius = 4;
    public float velocity = 0.33f;
    public float max_turning_angle = 0.01f;
    public float max_rotor_change = 0.001f;
    public int max_time = -1; /** -1 For infinite time
    
    /* 
    0 - Get into zone
    1 - Fly in the zone
    2 - Correct into the zone
    3 - Get out of the zone
    */

    // Zone checker
    private int CheckZone()
    {
        float distance = (r_center - rb.position).magnitude;
        if (distance < inner_radius)                             { return -1; } // Too close
        else if (distance > outer_radius)                        { return 1; }  // Too far
        else                                                     { return 0; }  // Just right
    }

    void Start()
    {
        TimeProvider = FindObjectOfType<TimeProviderGo>(); //TODO remporary bad solution
        randomGenerator = new System.Random(LastPlaneIndex++);

        rb = GetComponent<Rigidbody>();
        var starting_angle = randomGenerator.Range(-Mathf.PI, Mathf.PI);
        var direction = new Vector3(Mathf.Cos(starting_angle), 0, Mathf.Sin(starting_angle));
        var flatPosition = r_center - direction * r_radius;
        rb.position = new Vector3(flatPosition.x, rb.position.y, flatPosition.z);
        rb.velocity = direction * velocity;
        rotation = starting_angle;
        rb.rotation = Quaternion.LookRotation(new Vector3(Mathf.Cos(starting_angle), 0, Mathf.Sin(starting_angle)), new Vector3(0, 1, 0));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!TimeProvider.TimeUpdateEnabled)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        var state = CheckZone();
        if (max_time > 0) { if (timer < max_time) { timer += 1; } }
        switch (mode)
        {
            case 0:
                if ((previous_state != 0) & (state == 0))
                {
                    mode = 1;
                }
                break;
            case 1:
                rotor = Mathf.Clamp(rotor + randomGenerator.Range(-max_rotor_change, max_rotor_change), -max_turning_angle, max_turning_angle);
                if (state != 0)
                {
                    if ((timer == max_time) & (state == 1)) { mode = 3; }
                    else { mode = 2; }
                }
                break;
            case 2:
                var to_center = r_center - rb.position;
                var dot = Vector2.Dot(new Vector2(-to_center.z, to_center.x), new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation)));
                if (dot * state < 0) { rotor = max_turning_angle; }
                else { rotor = -max_turning_angle; }
                if (state == 0) { mode = 1; rotor = randomGenerator.Range(-max_rotor_change, max_rotor_change); }
                break;
            case 3:
                rotor = 0;
                break;
        }
        rotation += rotor;
        if (rotation < -Mathf.PI) { rotation += 2 * Mathf.PI; }
        else if (rotation > Mathf.PI) { rotation -= 2 * Mathf.PI; }
        rb.MoveRotation(Quaternion.LookRotation(new Vector3(Mathf.Cos(rotation), 0, Mathf.Sin(rotation)), new Vector3(0, 1, 0)));
        rb.velocity = new Vector3(Mathf.Cos(rotation), 0, Mathf.Sin(rotation)) * velocity;
        previous_state = state;
    }
}

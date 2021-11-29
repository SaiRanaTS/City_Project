using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public float Sun_intensity;
    public float orbit_speed = 0.5f;    //The velocity of which the directional light should move
    private int maxheight = 500;
    private float rotational_multiplier;
    void Update()
    {
        this.transform.RotateAround(Vector3.zero,Vector3.right,orbit_speed);    //Defines position at (0,0,0), rotate around right axis, and rotational speed
        rotational_multiplier = Mathf.Sin((float)(this.transform.position.y / maxheight) * (float)Mathf.PI / 2);
        if(rotational_multiplier >= 0)
        {
            Sun_intensity = rotational_multiplier;
        }
        else
        {
            Sun_intensity = 0;
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intensity : MonoBehaviour
{
    public int IntensityScore;
    private float Sun_intensity;
    private Vector3 MaxScale;
    private Vector3 InitPos;
    private Vector3 CurrentScale;
    private Vector3 CurrentPos;
    private Vector3 Sun_pos;
    // Start is called before the first frame update
    void Start()
    {
        MaxScale = this.transform.localScale;
        InitPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Sun_pos = GameObject.Find("light").transform.position;
        if (Physics.Raycast(InitPos,Sun_pos - InitPos))
        {
            Sun_intensity = 0;
            Debug.DrawLine(InitPos, Sun_pos, Color.red, 5f);
        }
        else
        {
            Sun_intensity = GameObject.Find("light").GetComponent<Orbit>().Sun_intensity;
            Debug.DrawLine(InitPos, Sun_pos,Color.white, 5f);
        }

        CurrentScale = new Vector3(MaxScale[0], MaxScale[1] * Sun_intensity, MaxScale[2]);
        CurrentPos = new Vector3(InitPos[0], MaxScale[1] * Sun_intensity, InitPos[2]);
        IntensityScore = (int)(Sun_intensity * 100);
        this.transform.localScale = CurrentScale;
        this.transform.localPosition = CurrentPos;
    }
}

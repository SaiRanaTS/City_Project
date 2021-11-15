using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;
using TMPro;

public class heatmap : MonoBehaviour
{
    public TMP_Text DayCycle;
    public TMP_Text mode;
    int[,] average_shadow_intensity = new int[162,162]; //Array that contains the average intensity of the shadow at a certain point
    int iterations = 0;     //Keep track on how many times the code has ran
    bool button;     //Button to switch between average shadow and current shadow
    Vector3 calc_point = new Vector3(0,0,0);
    int[,] shadow_list = new int[162,162]; //Array that contains 1 if shadow and 0 if no shadow
    int[,] shadow_intensity = new int[162,162]; //Array that contains the intensity of the shadow at a certain point
    Color[] colors = { new Color32(0, 255, 0, 255),new Color32(100, 255, 0, 255), new Color32(185,255,100, 255), new Color32(230,255,0, 255), new Color32(255,200,0, 255), new Color32(255,100,0, 255), new Color32(255,0,0, 255), new Color32(100,100,90, 255), new Color32(70,70,70, 255), new Color32(0,0,0, 255) }; //green, green-yellow, yellow, orange, brown, red, dark grey, black, black
    public Button shadow_average;
    public Button shadow_current;
    bool new_day = false; //true if new day, else false
    int days = 0; //total number of days simulated
    void Start()
    {
		shadow_average.onClick.AddListener(TaskOnClick);
        shadow_current.onClick.AddListener(() => ButtonClicked(42));
        DayCycle.text = "Number of days passed: "+ days.ToString();
    }
    void Update()
    {
        var Ground = GameObject.Find("Ground");     //Ground GameObject
        var sun_position = GameObject.Find("Sun").transform.position.y;        //Sun GameObject
        var Ground_bounds = Ground.GetComponent<Renderer>().bounds.size*0.15f;
        var length = Ground_bounds[0];
        var width = Ground_bounds[2];
        var pointeri = 1;
        if(sun_position>0)      
        {
            shadow_intensity = new int[162,162];      //If testing option 2, remove this line.   
            if(new_day)
            {
                days++;
                DayCycle.text = "Number of days passed: "+ days.ToString();
                new_day = false;
            }
        
            for(var i = -(length); i < length-length*0.0125f; i+= length*0.0125f)
            {
                calc_point = new Vector3(i,0,calc_point[2]);
                var pointerj = 1;

                for(var j = -(width); j < width-width*0.0125f; j+= width*0.0125f)
                {
                    RaycastHit hit_obj;
                    calc_point = new Vector3(calc_point[0],0,j);
                    if(Physics.Raycast(this.transform.position,calc_point-this.transform.position,out hit_obj))
                    {
                        if(hit_obj.transform == Ground.transform)
                        {
                            shadow_list[pointeri,pointerj] = 0;
                        } 
                        else
                        {
                            shadow_list[pointeri,pointerj] = 1;
                        }
//Two options for calculating shadow_intensity and average_shadow_intensity
//
//Option 1
                        shadow_intensity[pointeri,pointerj] += shadow_list[pointeri,pointerj];
                        shadow_intensity[pointeri,pointerj+1] += shadow_list[pointeri,pointerj];
                        shadow_intensity[pointeri,pointerj-1] += shadow_list[pointeri,pointerj];
                        shadow_intensity[pointeri+1,pointerj] += shadow_list[pointeri,pointerj];
                        shadow_intensity[pointeri+1,pointerj+1] += shadow_list[pointeri,pointerj];
                        shadow_intensity[pointeri+1,pointerj-1] += shadow_list[pointeri,pointerj];
                        shadow_intensity[pointeri-1,pointerj] += shadow_list[pointeri,pointerj];
                        shadow_intensity[pointeri-1,pointerj+1] += shadow_list[pointeri,pointerj];
                        shadow_intensity[pointeri-1,pointerj-1] +=shadow_list[pointeri,pointerj];
                        average_shadow_intensity[pointeri,pointerj] += shadow_list[pointeri,pointerj];
                        average_shadow_intensity[pointeri,pointerj+1] += shadow_list[pointeri,pointerj];
                        average_shadow_intensity[pointeri,pointerj-1] += shadow_list[pointeri,pointerj];
                        average_shadow_intensity[pointeri+1,pointerj] += shadow_list[pointeri,pointerj];
                        average_shadow_intensity[pointeri+1,pointerj+1] += shadow_list[pointeri,pointerj];
                        average_shadow_intensity[pointeri+1,pointerj-1] += shadow_list[pointeri,pointerj];
                        average_shadow_intensity[pointeri-1,pointerj] += shadow_list[pointeri,pointerj];
                        average_shadow_intensity[pointeri-1,pointerj+1] += shadow_list[pointeri,pointerj];
                        average_shadow_intensity[pointeri-1,pointerj-1] +=shadow_list[pointeri,pointerj];
/*
//Option 2
                        if(pointeri > 1 & pointerj > 1)
                        {
                            shadow_intensity[pointeri-1,pointerj-1] = ((shadow_list[pointeri,pointerj] + shadow_list[pointeri,pointerj+1] + shadow_list[pointeri,pointerj-1] + shadow_list[pointeri+1,pointerj] + shadow_list[pointeri+1,pointerj+1] + shadow_list[pointeri+1,pointerj-1] + shadow_list[pointeri-1,pointerj] + shadow_list[pointeri-1,pointerj+1] + shadow_list[pointeri-1,pointerj-1]));
                            average_shadow_intensity[pointeri-1,pointerj-1] += shadow_intensity[pointeri,pointerj];
                        }
*/
                    }
                    pointerj++;
                }
                pointeri++;
            }
            iterations++;
        }

        else
        {
            new_day = true;
        }

        if(button)
        {
            CreateHeatMap(shadow_intensity, 160, 160, "Heatmap",1,button);
            mode.text = "Active mode is: Current Shadow";
        }
        else
        {
            CreateHeatMap(average_shadow_intensity, 160, 160, "Heatmap",iterations,button);
            mode.text = "Active mode is: Average Shadow";
        }
    }
    
    void CreateHeatMap(int[,] shadow_i, int W, int L, string Name,int iteration, bool button)
    {
        Color _orange = new Color(1.0f, 0.64f, 0.0f);
        GameObject textureObject;
        textureObject = GameObject.Find(Name);
        Texture2D texture;
        texture = new Texture2D(W-2, L-2, TextureFormat.ARGB32, false);
        for (int i = 1; i < L-2; i++)
        {
            for (int j = 1; j < W-2; j++)
            {   
                int color = shadow_i[i,j]/iteration;
                if(color >9)
                {
                    color = 9;
                }
                texture.SetPixel(i, j, colors[color]);                   
            }
        }
        texture.Apply();
        textureObject.GetComponent<Renderer>().material.mainTexture = texture;
    }

    void TaskOnClick()
    {
        button = false;
    }
    void ButtonClicked(int buttonNo)
    {
        button = true;
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;
using TMPro;

//Heatmap class, containing both a average heatmap of the shadow in the city and also ability to showcase shadow heatmap based on user defined frequencys. 
public class heatmap : MonoBehaviour
{

    List<int>[,] shadow_list_CFH = new List<int>[162,162]; //162x162 array containg List<int>
    public TMP_Text DayCycle;
    public TMP_Text mode;

    int[,] average_shadow_intensity = new int[162,162]; //Array that contains the average intensity of the shadow at a certain point
    int[,] CFH_shadow = new int[162,162]; //Array that contains the average intensity of the shadow at a certain point

    int[,] average_CFH_shadow = new int[162,162];
    int iterations = 0;     //Keep track on how many times the code has ran

    bool button;     //Button to switch between average shadow and current shadow
    Vector3 calc_point = new Vector3(0,0,0);
    int[,] shadow_list = new int[162,162]; //Array that contains 1 if shadow and 0 if no shadow
    int[,] shadow_intensity = new int[162,162]; //Array that contains the intensity of the shadow at a certain point

    Color[] colors = { new Color32(0, 255, 0, 255),new Color32(100, 255, 0, 255), new Color32(185,255,100, 255), new Color32(230,255,0, 255), new Color32(255,200,0, 255), new Color32(255,100,0, 255), new Color32(255,0,0, 255), new Color32(100,100,90, 255), new Color32(70,70,70, 255), new Color32(0,0,0, 255) }; //green, green-yellow, yellow, orange, brown, red, dark grey, black, black
   // public Button shadow_average;
    //public Button shadow_current;

    private bool average = false;
    private bool current = false;
    private string user_input = "nope";
    bool new_day = false; //true if new day, else false
    bool reset = false;
    int days = 0; //total number of days simulated
    bool first_round = true; //Is only true for the first iteration. 
    bool valid_input_recived = false;
  

    int Current_CFH = 0;    
    int Previous_CFH = 0;   //Three variables for the sliding shadow frequency window. 
    int first_CFH = 0;  

    void Start()
    {
        DayCycle.text = "Number of days passed: "+ days.ToString();
    }

    void Update()
    {   
        //Initilizing shadow_list_CFH array with 3x1 List
        if(iterations <=0 || reset == true)
        {        
            for(var i = 1; i < 160; i+= 1)
            {
                for(var j =1; j < 160; j+= 1)
                {
                    shadow_list_CFH[i,j] = new List<int>(){0,0,0};   
                }
            }
            Debug.Log("List has been reseted");
        }
        if(reset)
        {
            average_CFH_shadow = new int[162,162];
            CFH_shadow = new int[162,162];
        }

        var Ground = GameObject.Find("Ground");     //Ground GameObject
        var sun_position = GameObject.Find("light").transform.position.y;        //Sun GameObject
        var Ground_bounds = Ground.GetComponent<Renderer>().bounds.size*0.15f;
        var length = Ground_bounds[0];
        var width = Ground_bounds[2];
        var pointeri = 1;

        if(sun_position>=0)      
        {
            // shadow_intensity = new int[162,162];      //If testing option 2, Uncomment this line.    
            if(new_day)
            {
                days++;
                DayCycle.text = "Number of days passed: "+ days.ToString();     //Number of days simulated, for visualizasing purposes. 
                new_day = false;
            }
            for(var i = -(length); i < length-length*0.0125f; i+= length*0.0125f)
            {
                calc_point = new Vector3(i,0,calc_point[2]);
                var pointerj = 1;
                for(var j = -(width); j < width-width*0.0125f; j+= width*0.0125f)
                {
                    var indentifier = Convert.ToString(pointeri)+Convert.ToString(pointerj);

                    RaycastHit hit_obj;
                    calc_point = new Vector3(calc_point[0],0,j);
                    if(Physics.Raycast(this.transform.position,calc_point-this.transform.position,out hit_obj))
                    {

                        var cfh_values = shadow_list_CFH[pointeri,pointerj];
                        Current_CFH = cfh_values[2];        //Variable to store the third value in the frequency
                        Previous_CFH = cfh_values[1];       //Variable to store the second value in the frequency
                        first_CFH = cfh_values[0];          //Variable to store the first value in the frequency
                        string checker = first_CFH.ToString()+Previous_CFH.ToString()+Current_CFH.ToString();  // Check if current frequency is same as user defined

                        if(hit_obj.transform == Ground.transform)//set to 0 // higher value --> more shadow
                        {
                            shadow_list_CFH[pointeri,pointerj].Insert(0,Previous_CFH);
                            shadow_list_CFH[pointeri,pointerj].Insert(1,Current_CFH);
                            shadow_list_CFH[pointeri,pointerj].Insert(2,0);
                            shadow_list[pointeri,pointerj] = 0;
                        }
                        //set to 1
                        else
                        {  
                            if(first_round)
                            {
                              shadow_list_CFH[pointeri,pointerj] = new List<int>(){1,1,1};   
                            }
                            shadow_list_CFH[pointeri,pointerj].Insert(0,Previous_CFH);
                            shadow_list_CFH[pointeri,pointerj].Insert(1,Current_CFH);
                            shadow_list_CFH[pointeri,pointerj].Insert(2,1);            
                            shadow_list[pointeri,pointerj] = 1;
                        }

                            if(checker==user_input)
                            {
                                if(reset)
                                {
                                    CFH_shadow[pointeri,pointerj] = 0;
                                    average_CFH_shadow[pointeri-1,pointerj-1] = 0;
                                }
                                if(!reset)
                                {
                                CFH_shadow[pointeri,pointerj] = CFH_shadow[pointeri,pointerj]+ 1;
                                average_CFH_shadow[pointeri-1,pointerj-1] += ((CFH_shadow[pointeri,pointerj] + CFH_shadow[pointeri,pointerj+1] + CFH_shadow[pointeri,pointerj-1] + CFH_shadow[pointeri+1,pointerj] + CFH_shadow[pointeri+1,pointerj+1] + CFH_shadow[pointeri+1,pointerj-1] + CFH_shadow[pointeri-1,pointerj] + CFH_shadow[pointeri-1,pointerj+1] + CFH_shadow[pointeri-1,pointerj-1]));
                                }
                                //iteration_cfh +=1;
                            }
                        
                        
   
  
                        //Option 2/ Used in development. Provides same result as Option 1. 
                        if(pointeri > 1 & pointerj > 1)
                        {
                            shadow_intensity[pointeri-1,pointerj-1] = ((shadow_list[pointeri,pointerj] + shadow_list[pointeri,pointerj+1] + shadow_list[pointeri,pointerj-1] + shadow_list[pointeri+1,pointerj] + shadow_list[pointeri+1,pointerj+1] + shadow_list[pointeri+1,pointerj-1] + shadow_list[pointeri-1,pointerj] + shadow_list[pointeri-1,pointerj+1] + shadow_list[pointeri-1,pointerj-1]));
                            average_shadow_intensity[pointeri-1,pointerj-1] += shadow_intensity[pointeri,pointerj];

                        }
                        
//Two options for calculating shadow_intensity and average_shadow_intensity
//
//Option 1
  /*                    shadow_intensity[pointeri,pointerj] += shadow_list[pointeri,pointerj];
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
/**/                    
                    }
                    pointerj++;
                }

                pointeri++;
            }
            reset = false;
            first_round = false;
            iterations++;
        }

        else
        {
            new_day = true;
        }

        if(average)
        {
            CreateHeatMap(average_shadow_intensity, 160, 160, "Heatmap",iterations,button);
            mode.text = "Active mode is: Average Shadow";
        }
        if(current)
        {
            CreateHeatMap(shadow_intensity, 160, 160, "Heatmap",1,button);
            mode.text = "Active mode is: Current Shadow with freq 010";
        }
        if(valid_input_recived){
            CreateCFH(average_CFH_shadow,160,160,"Heatmap",CFH_shadow,button);
        }
    }
    
    void CreateHeatMap(int[,] shadow_i, int W, int L, string Name, int iteration,bool button)
    {
        GameObject textureObject;
        textureObject = GameObject.Find(Name);
        Texture2D texture;
        texture = new Texture2D(W-2, L-2, TextureFormat.ARGB32, false);
        
        for (int i = 1; i < L-2; i++)
        {
            for (int j = 1; j < W-2; j++)
            {   
                int color = shadow_i[i,j]/iteration;
                //Debug.Log(color);
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

    void CreateCFH(int[,] shadow_i, int W, int L, string Name,int[,] iteration, bool reset)
    {
        GameObject textureObject;
        textureObject = GameObject.Find(Name);
        Texture2D texture;
        texture = new Texture2D(W-2, L-2, TextureFormat.ARGB32, false);
        for (int i = 1; i < L-2; i++)
        {
            for (int j = 1; j < W-2; j++)
            {   
                int color = shadow_i[i,j]/(1+iteration[i,j]); //To avoid dividing on 0, add 1. 


                if(iteration[i,j]==0)   //if 0 set color to 0 
                {
                    color = 0;
                }
                if(color >9)
                {
                    color = 9;
                }
                if(reset)
                {
                    color = 0;
                }

                texture.SetPixel(i, j, colors[color]);                  
            }
        }
        texture.Apply();        //Apply texture. 
        textureObject.GetComponent<Renderer>().material.mainTexture = texture;
    }
   

   /* void TaskOnClick()
    {
        button = false;
    }
    void ButtonClicked(int buttonNo)
    {
        button = true;
    }*/

    public void SetCurrentMode(bool b)
    {
        Debug.Log("Current pressed");
        current = true;
        average = false;
        valid_input_recived = false;

    }
    public void SetAverageMode(bool b)
    {
        Debug.Log("Average pressed");
        average = true;
        valid_input_recived = false;
        current = false;
    }

    public void ReadStringInput(string s)
    {
        if( s == "111" || s == "110"|| s == "101"||s == "011"||s == "001"||s == "010"||s == "100"||s == "000")
        {
            user_input = (string)s;
            reset = true;
            valid_input_recived = true;
            average = false;
            current = false;
        }
    }
}

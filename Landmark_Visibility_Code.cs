using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coll : MonoBehaviour
{

	bool isDone = false; //false while no calculations are done, true when calculation is done
    // Update is called once per frame
    void Update()
    {
    if (!isDone) {
    	int hit = 0;    //Variable to store number of hits
		int miss = 0;   //Variable to store number of misses
        float number_of_rays = 360;  //Amount of rays in each height
        float totalAngle = 360; //Total angle to cast rays
        float delta = totalAngle / number_of_rays;  //Increment between rays in circle
		var Landmark = GameObject.Find("Landmark");
        var Landmark_pos = Landmark.transform.position;
    	var Landmark_h = Landmark.transform.position.y*2;
		var Landmark_bounds = Landmark.GetComponent<Renderer>().bounds.size/2 + new Vector3(0.5f,0,0.5f);
    	var ray_length = 50;	//Length of ray
		var delta_x = 0f;	//
		var delta_z = 0f;	//
		var delta_position = new Vector3();
		Color reds = Color.red;	//
		float k = Landmark_h*0.0495f;	//
		reds[3] = 0.8f;
		var divisor = 10;

    	for(var i = Landmark_h; i > Landmark_h*0.005f; i=i-k){
    		if (i == Landmark_h){
    		this.transform.position = new Vector3(Landmark_pos.x,Landmark_pos.y+(Landmark_h/2)-Landmark_h*0.005f,Landmark_pos.z);
    		}else{
    		this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y-k,this.transform.position.z);	
    		}
			for (int s = 0; s < 4;s++){
				if (s == 0){
					delta_x = 0f;
					delta_z = Landmark_bounds[2]/divisor;
					this.transform.position = new Vector3(Landmark_pos.x - Landmark_bounds[0],this.transform.position.y,Landmark_pos.z -Landmark_bounds[2]);
				}else if(s == 1){
					delta_z = 0f;
					delta_x = Landmark_bounds[0]/divisor;
				}else if (s == 2){
					delta_x = 0f;
					delta_z = -Landmark_bounds[2]/divisor;
				}else if(s == 3){
					delta_z = 0f;
					delta_x = -Landmark_bounds[0]/divisor;
				}
				for (int l = 0;l < divisor * 2 + 1;l++)
				{
					if (l == 0){
						delta_position = new Vector3(0,0,0);	
					}else{
						delta_position = new Vector3(delta_x,0,delta_z);
					}
					this.transform.position = this.transform.position + delta_position;
					for (int u = 0; u < number_of_rays; u++)
						{
							RaycastHit hit_obj;
							var dir = Quaternion.Euler(0, u * delta, 0) * transform.right;         
							if(Physics.Raycast(this.transform.position,dir,out hit_obj,ray_length)){
								if(hit_obj.transform != Landmark.transform){
									hit++;
									//Debug.DrawLine(this.transform.position,hit_obj.point,reds,60);
									//Debug.DrawRay(this.transform.position, dir * 2.5f, reds,60);
								}
								}else{
								miss++;
								Debug.DrawRay(this.transform.position, dir * 2.5f, Color.green,60);
								}
						}
				}
    	}
    	}
		float total = hit + miss;	
		float vis_score = miss*100f/(total);//Calculation of Accuracy
    	Debug.Log("Hits = " + hit);
    	Debug.Log("Misses = " + miss);
    	Debug.Log("Visibility Score = " + vis_score+ "%");
        isDone = true;
    }
        
}
}

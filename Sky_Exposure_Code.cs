using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{
    bool isDone = false;
    // Update is called once per frame
    void Update()
    {
    	int hit = 0;
		int miss = 0;
    if (!isDone) {
			float number_of_rays = 50;
			float totalAngle = 180;
			float ray_length = 20;
			float delta = totalAngle / number_of_rays;
			Vector3 pos = this.transform.position;
			const float magnitude = 50;
			for (int u = 0; u < number_of_rays; u++)
			{
				for (int i = 0; i < number_of_rays; i++)
				{
				    var dir = Quaternion.Euler(0, u*delta-(3.14f/2f), i * delta) * transform.right;         
				    if(Physics.Raycast(pos, dir,ray_length)){
				    	hit++;
				    	Debug.DrawRay(pos, dir * magnitude, Color.red,60);
				    	}else{
				    	miss++;
				    	Debug.DrawRay(pos, dir * magnitude, Color.green,60);
				    	}
				    
				}
			}
			float vis_score = miss/(number_of_rays*number_of_rays)*100;
    		Debug.Log("Hits = " + hit);
    		Debug.Log("Misses = " + miss);
    		Debug.Log("Visibility Score = " + vis_score+ "%");
    		isDone = true;
	    }
    } 
}

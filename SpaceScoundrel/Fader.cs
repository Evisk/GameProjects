using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour {

    bool fade;
   public  float alpha = 1.0f;
    Color blinky = Color.white;

	void FixedUpdate () {
        if (alpha <= 0.0f )
        {
            fade = false;
        }
        if (alpha >= 250.0f)
        {
            fade = true;
        }
        if (!fade)
        {
            alpha += Time.deltaTime * 20;

        }else
        {

            alpha -= Time.deltaTime * 20;
        }
        

        blinky.a = alpha;
       // GetComponent<LineRenderer>().material.color = blinky;
 


	    
	}
}

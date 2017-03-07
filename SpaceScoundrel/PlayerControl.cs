using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

public class PlayerControl : BasicControl {


	public void CharacterMovement(){
		GetComponent<Rigidbody2D> ().AddForce (transform.up * Speed);


	}


	void Update () {



		LookAt (Camera.main.ScreenToWorldPoint (Input.mousePosition));
		if(!Input.GetKey(PlayerPrefs.Instance.keyBinds["Forward"]) || !Input.GetKey(PlayerPrefs.Instance.keyBinds["Backward"])){
			Decelerate();
		}
		if(Input.GetKey(PlayerPrefs.Instance.keyBinds["Forward"])){
			MoveForward();
		}
		if (Input.GetKey (PlayerPrefs.Instance.keyBinds ["Backward"])) {
			MoveBackward();

		}
		CharacterMovement ();
		if (!anim.isPlaying) {
			ChangeLayer(setlayer);
			
		}

	
		if (Input.GetKeyDown (PlayerPrefs.Instance.keyBinds["Up"]) && !anim.isPlaying) {
				UpDepth ();
				DecreaseLayer();
			}

        if(Input.GetKeyDown (PlayerPrefs.Instance.keyBinds["Down"]) && !anim.isPlaying)
        {
            DownDepth();
            IncreaseLayer();

        }

		
	
	}
}


using UnityEngine;
using System.Collections;

public class BasicControl : MonoBehaviour {

	public float Speed = 0f;
	public float topMaxSpeed = 50;
	public float topMinSpeed = -50;
	public float noSpeed = 0;
	public int setlayer = 8;
	public int animNum = 0;
	private GameObject Indicator;
	public bool layerChange = false;
	public Animation anim;
	public float rotSpeed = 90;

	public void LookAt(Vector3 pos){
		Vector3 direction = pos - transform.position;
		Quaternion desiredRotation = Quaternion.Euler (0, 0, Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg - 90);
		transform.rotation = Quaternion.RotateTowards (transform.rotation, desiredRotation, rotSpeed * Time.deltaTime);



	}

	public void MoveForward(){
		if(Speed<=topMaxSpeed)
			Speed+=100*Time.deltaTime;
	}

	public void MoveBackward(){
		if(Speed>=topMinSpeed)
			Speed-=10*Time.deltaTime;



	}

	public void Decelerate(){
		if(Speed >0)
			Speed-=20*Time.deltaTime;
		if(Speed<0)
			Speed+=20*Time.deltaTime;

	}

	public void UpDepth(){

		if ((animNum - 1) == -1)
			return;


		animNum--;
		anim.Play ("DepthScaleUp" + (animNum));
		

		
	}

	public void DecreaseLayer(){
		if ((setlayer) - 1 == 7)
			return;
		setlayer--;
	}

	public void IncreaseLayer(){
		if ((setlayer) + 1 == 12)
			return;
		setlayer++;


	}

	public void DownDepth(){
		if ((animNum + 1) == 4)
			return;

		animNum++;
		anim.Play ("DepthScaleDown" + (animNum));
		

	}
	


		public void ChangeLayer(int i){
		gameObject.layer = setlayer;
		layerChange = false;

	}


	
	void Awake(){
		Indicator = transform.Find ("LayerIndicator").transform.Find ("depthMeter").transform.Find ("indicator").gameObject;
		anim = GetComponent<Animation> ();

	}

}
	
	
	
using UnityEngine;
using System.Collections;

public class DepthIndicator : MonoBehaviour {
	Quaternion rotation;
	public Animation anim;
	Vector3 scale;
	void Awake(){
		rotation = transform.rotation;
		anim = transform.Find ("indicator").GetComponent<Animation> ();
	}


	void Update () {
		transform.rotation = rotation;
		anim.Play("Depth"+(transform.parent.transform.parent.gameObject.layer-7));

	}
}

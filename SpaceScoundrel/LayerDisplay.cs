using UnityEngine;
using System.Collections;

public class LayerDisplay : MonoBehaviour{
	

	void Start(){
		transform.position = transform.parent.position;
		transform.Find ("UpperLeft").transform.localPosition = new Vector2 ((transform.parent.GetComponent<BoxCollider2D> ().size.x/2)*-1 , (transform.parent.GetComponent<BoxCollider2D> ().size.y/2));
		transform.Find ("UpperRight").transform.localPosition = new Vector2 ((transform.parent.GetComponent<BoxCollider2D> ().size.x/2) , (transform.parent.GetComponent<BoxCollider2D> ().size.y/2));
		transform.Find ("BottomLeft").transform.localPosition = new Vector2 ((transform.parent.GetComponent<BoxCollider2D> ().size.x/2)*-1 , (transform.parent.GetComponent<BoxCollider2D> ().size.y/2)*-1);
		transform.Find ("BottomRight").transform.localPosition = new Vector2 ((transform.parent.GetComponent<BoxCollider2D> ().size.x/2) , (transform.parent.GetComponent<BoxCollider2D> ().size.y/2)*-1);
		transform.Find ("depthMeter").transform.localPosition = new Vector2((transform.parent.GetComponent<BoxCollider2D> ().size.x/2)+0.35f,(transform.parent.GetComponent<BoxCollider2D> ().size.y/2)+0.35f);
	}




}




using UnityEngine;
using System.Collections;

public class Moon : MonoBehaviour {
	public float PlanetRotateSpeed = -1f;
	public float OrbitSpeed;

	
	// Update is called once per frame
	void Update () {
		transform.RotateAround (transform.parent.transform.position, Vector3.forward, OrbitSpeed* Time.deltaTime);
	}
}

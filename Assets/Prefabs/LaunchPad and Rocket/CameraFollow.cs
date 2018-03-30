using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	Transform rocket;

	float rocketYpos, rocketXpos;
	// Use this for initialization
	void Start () {

		rocket = GameObject.FindObjectOfType<Rocket>().transform;
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		rocketYpos = rocket.transform.position.y;
		rocketXpos = rocket.transform.position.x;
		transform.position = new Vector3(rocketXpos, rocketYpos, transform.position.z);
	}
}

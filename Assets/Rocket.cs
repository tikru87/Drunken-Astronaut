using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rocket : MonoBehaviour {

	Rigidbody rB;
	AudioSource aS;
		int startPitch = 0;
		int pitchTime = 2;

		[SerializeField] float maxVelocity = 4f;
		[SerializeField]float rocketThrust = 100f;
		[SerializeField]float mainThrust = 50f;

	
	// Use this for initialization
	void Start () {
		rB = GetComponent<Rigidbody>();
		aS = GetComponent<AudioSource>();
		aS.pitch = startPitch;



		
	}

	void OnCollisionEnter(Collision other)
	{
		switch(other.gameObject.tag)
		{
			case "Friendly":
			print("Safe");
			break;
			default:
			print("BOOM!");
			break;

		}
	}
	
	// Update is called once per frame
	void Update () {
		Thrust();
		Rotate();
		}

	private void Thrust()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			if (rB.velocity.y <= maxVelocity)
			{
			rB.AddRelativeForce(Vector3.up*mainThrust);
			}
			if (!aS.isPlaying)
			{
				aS.Play();
			} 
		} else
		{
			aS.Stop();
		}
		if (aS.isPlaying && aS.pitch <= 1)
		{
			aS.pitch += Time.deltaTime * pitchTime;
		}
	}

	private void Rotate()
	{
		rB.freezeRotation = true;
		
		if (Input.GetKey(KeyCode.A))
		{
			float thisFrameRot = rocketThrust * Time.deltaTime;
			transform.Rotate(Vector3.forward * thisFrameRot); 
		}
		else if (Input.GetKey(KeyCode.D))
		{
			float thisFrameRot = rocketThrust * Time.deltaTime;
			transform.Rotate(-Vector3.forward * thisFrameRot); 

		}
		if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
		{
			print("You are doing it Wrong!");
		}
		rB.freezeRotation = false;
	}
}

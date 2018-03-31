using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour {

	[Header("Moving Settings")]
	[SerializeField] Vector3 movementVector;
	[Tooltip("Higher value = faster")][Range(0.1f, 10F)][SerializeField] float duration = 2f;
	[Range(0,1)]private float movementFactor;
	Vector3 startingPos;
	// Use this for initialization
	void Start () {
		startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		HandleMovement();
	}
	private void HandleMovement()
	{	
		
		float cycles = Time.time / duration;;

		const float tau = Mathf.PI * 2;
		float rawSinWave = Mathf.Sin(cycles * tau);

		movementFactor = rawSinWave / 2f + 0.5f;
		Vector3 offset = movementFactor * movementVector;
		transform.position = startingPos + offset;
	}
}

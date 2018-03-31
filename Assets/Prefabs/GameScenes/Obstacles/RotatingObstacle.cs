using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObstacle : MonoBehaviour {

	private enum RotationDirection{Left, Right};
	[Header("Rotating Settings")]
	[SerializeField]RotationDirection rotDir;
	[Range(0.1f, 1000f)][SerializeField] float rotSpeed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		HandleRotation();
	}

		private void HandleRotation()
	{
		switch(rotDir.ToString())
		{
			case "Right":
			transform.Rotate(Vector3.forward * Time.deltaTime * rotSpeed);
			break;
            case "Left":
			transform.Rotate(Vector3.back * Time.deltaTime * rotSpeed);
                break;
            default:
				print("Should Not Happen");
                break;

        }
	}
}
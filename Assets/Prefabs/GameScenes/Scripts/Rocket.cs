using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Rocket : MonoBehaviour {

	Rigidbody rB;
	AudioSource aS;
	ParticleSystem pS;
	enum State {Alive, Dying, Transcending}
	State state = State.Alive;
		int startPitch = 0;
		int pitchTime = 2;
		[SerializeField] AudioClip engine, explosion, bgMusic;
		[SerializeField] ParticleSystem engineParticle, explosionParticle, succeedParticle;
		[SerializeField] float maxVelocity = 4f;
		[SerializeField]float rocketThrust = 100f;
		[SerializeField]float mainThrust = 50f;
		[SerializeField] static float timeToLoadScene = 3f;

		[SerializeField] float maxFuel = 50f;
		[SerializeField] float fuel;

		[SerializeField] Image fuelBar;

	
	// Use this for initialization
	void Start () {
		rB = GetComponent<Rigidbody>();
		aS = GetComponent<AudioSource>();
		fuel = maxFuel;
	}

	private void LoadNextScene()
	{
		int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
		SceneManager.LoadScene(nextScene);
	}
	private void Reload()
	{
		int currentScene = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(currentScene);
	}

	void OnCollisionEnter(Collision other)
	{

		if (state != State.Alive)
		{
			return;
		}
		switch(other.gameObject.tag)
		{
			case "Friendly":
			// nothing here
			break;
            case "Finish":
                StartSuccesSequence();
                break;
            default:
                StartDeathSequence();
                break;

        }
	}

    private void StartSuccesSequence()
    {
        state = State.Transcending;
		engineParticle.Stop();
		succeedParticle.Play();
        Invoke("LoadNextScene", timeToLoadScene);
    }

    private void StartDeathSequence()
    {
        state = State.Dying;;
        aS.clip = explosion;
        aS.loop = false;
        aS.Play();
		engineParticle.Stop();
		explosionParticle.Play();
        Invoke("Reload", timeToLoadScene);
    }

    // Update is called once per frame
    void Update () {
		if (state == State.Alive && fuel > 0)
		{
		RespondToThrustInput();
		RespondToRotateInput();
		HandleFuel();
		}
	}

	private void HandleFuel()
	{
		if(aS.isPlaying && aS.clip == engine)
		{
			fuel -= Time.deltaTime * 10;
			fuelBar.fillAmount = fuel / maxFuel;
		}

		if (fuel <=0 )
		{
			StartDeathSequence();
		}
	}
	private void RespondToThrustInput()
    {
        ApplyThrust();
    }

    private void ApplyThrust()
    {
        if (Input.GetKey(KeyCode.Space) && maxFuel >= 0)
        {
            if (rB.velocity.y <= maxVelocity)
            {
                rB.AddRelativeForce(Vector3.up * mainThrust);
            }
            if (!aS.isPlaying)
            {
				aS.clip = engine;
				aS.loop = true;
                aS.Play();
				engineParticle.Play();
				
            } 
        } else
		{
			aS.Stop();
			engineParticle.Stop();
		}

        if (aS.isPlaying && aS.pitch <= 1)
        {
            aS.pitch += Time.deltaTime * pitchTime;
        }
    }

    private void RespondToRotateInput()
	{
		rB.angularVelocity = Vector3.zero;
		
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

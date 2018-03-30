using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

	
	// Use this for initialization
	void Start () {
		rB = GetComponent<Rigidbody>();
		aS = GetComponent<AudioSource>();
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
		succeedParticle.Play();
        Invoke("LoadNextScene", timeToLoadScene);
    }

    private void StartDeathSequence()
    {
        state = State.Dying;;
        aS.clip = explosion;
        aS.loop = false;
        aS.Play();
		explosionParticle.Play();
        Invoke("Reload", timeToLoadScene);
    }

    // Update is called once per frame
    void Update () {
		if (state == State.Alive)
		{
		RespondToThrustInput();
		RespondToRotateInput();
		}
	}

	private void RespondToThrustInput()
    {
        ApplyThrust();
    }

    private void ApplyThrust()
    {
        if (Input.GetKey(KeyCode.Space))
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

using UnityEngine;
using System.Collections;

public class CheckpointScript : MonoBehaviour {

	public LevelManagerScript levelManager;

	public bool isactived = false;



	// Use this for initialization
	void Start () 
	{
		levelManager = FindObjectOfType<LevelManagerScript> ();
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (levelManager.currentCheckpoint == gameObject) 
		{
			
				gameObject.GetComponentInChildren<ParticleSystem> ().Play ();
				gameObject.GetComponentInChildren<Light> ().enabled = true;

		}
		else 
		{
			gameObject.GetComponentInChildren<ParticleSystem> ().Stop ();
			gameObject.GetComponentInChildren<Light> ().enabled = false;
		}

	

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.name == "Player") 
		{
			levelManager.currentCheckpoint = gameObject;


		}
	}
}

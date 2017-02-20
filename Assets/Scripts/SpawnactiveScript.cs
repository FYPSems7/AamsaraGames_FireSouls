using UnityEngine;
using System.Collections;

public class SpawnactiveScript : MonoBehaviour {

	public GameObject sparkParticle;
	public GameObject firecamp;
	public bool spawnactived = false;
	public int firespark = 0;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (spawnactived)
		{
			

			if (firespark == 1) 
			{
				GameObject Temporary_spark_Handler = null;
				Temporary_spark_Handler = Instantiate (sparkParticle, firecamp.transform.position, Quaternion.Euler (-90, 0, 0)) as GameObject;
				spawnactived = false;
			}

		}


	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player"))
		{
			spawnactived = true;
			firespark += 1;
		}
	}

}

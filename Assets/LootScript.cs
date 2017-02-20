using UnityEngine;
using System.Collections;

public class LootScript : MonoBehaviour {




	// Use this for initialization
	void Start () 
	{
		CharacterStatsScript.Itemnumbers = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.name == "Player") 
		{
			CharacterStatsScript.Itemnumbers += 1;
			Destroy (gameObject);

		}
	}
}

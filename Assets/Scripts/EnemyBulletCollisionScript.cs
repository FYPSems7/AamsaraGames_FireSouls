using UnityEngine;
using System.Collections;

public class EnemyBulletCollisionScript : MonoBehaviour {

	//public CharacterStatsScript characterstatsscript;
	public bool isDestoryed;
	public static float ememyBulletDmg = 20.0f;
	private float attackDamage = 5.0f;

	// Use this for initialization
	void Start ()
	{
		//characterstatsscript = GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterStatsScript> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player")) 
		{
			other.GetComponent<CharacterStatsScript> ().DmgTaken ((int)attackDamage);
			isDestoryed = true;
		} 
	else 
		{
			isDestoryed = false;
		}

	if (isDestoryed)
		{
			Destroy (gameObject, 0.05f);
		}
	}
}

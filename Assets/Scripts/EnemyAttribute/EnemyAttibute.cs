using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAttibute : MonoBehaviour {

	public GameObject Canvas;
	public GameObject DmgTextPrefab;
	public EnemyAttributeListScript enemyAttributeList;
	public EnemyAttribute eA;

	// Use this for initialization
	void Start () {
		Canvas = GameObject.FindGameObjectWithTag ("UI");
		enemyAttributeList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EnemyAttributeListScript>();
		foreach (EnemyAttribute n in enemyAttributeList.enemysattribute) {
			if(n.EnemyName == gameObject.name) {
				eA = n.Clone ();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (eA.EnemyHealth);
		if (eA.EnemyHealth <= 0) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log ("Run");
		if (other.gameObject.tag == "PlayerAttack") {
			//eA.EnemyHealth -= (int)other.GetComponent<CharacterStatsScript> ().MeleeAttack;
			DmgTaken((int)other.GetComponent<CharacterStatsScript> ().MeleeAttack);
		}
		if (other.gameObject.tag == "PlayerBulletAttack") {
			//eA.EnemyHealth -= 60;
			DmgTaken (60);
		}
	}

	void DmgTaken(int Dmg)
	{
		eA.EnemyHealth -= Dmg;
		Vector3 playerPos = new Vector3 (transform.position.x, transform.position.y + 2.2f, transform.position.z);
		Vector3 pos = Camera.main.WorldToScreenPoint (playerPos);

		GameObject DmgText = (GameObject)Instantiate (DmgTextPrefab, pos, Quaternion.identity);
		DmgText.transform.SetParent (Canvas.transform);
		DmgTextPrefab.GetComponent<Text>().text = Dmg.ToString ();
		Destroy (DmgText, 1.0f);

	}
}

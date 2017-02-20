using UnityEngine;
using System.Collections;

[System.Serializable]

public class EnemyAttribute {

	public string EnemyName;
	public int EnemyHealth;
	public float EnemyDamage;
	public float EnemyMovementSpeed;
	public float EnemyAttackRange;
	public float EnemyAttackSpeed;


	public enum EnemyType
	{
		Melee,
		Range,
		Boss
	}

	public EnemyAttribute Clone()
	{
		EnemyAttribute newEnemyAttribute = new EnemyAttribute ();
		newEnemyAttribute.EnemyName = this.EnemyName;
		newEnemyAttribute.EnemyHealth = this.EnemyHealth;
		newEnemyAttribute.EnemyDamage = this.EnemyDamage;
		newEnemyAttribute.EnemyMovementSpeed = this.EnemyMovementSpeed;
		newEnemyAttribute.EnemyAttackRange = this.EnemyAttackRange;
		newEnemyAttribute.EnemyAttackSpeed = this.EnemyAttackSpeed;

		return newEnemyAttribute;
	}


}

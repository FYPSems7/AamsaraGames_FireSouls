using UnityEngine;
using System.Collections;

public class GameManager_Reference : MonoBehaviour 
{
	public string playerTag;
	public static string _playerTag;

	public string enemyTag;
	public static string _enemyTag;

	public static GameObject _player;

	void OnEnable()
	{
		if (playerTag == "") 
		{
			Debug.LogWarning ("Please key in player tag");
		}

		if (enemyTag == "") 
		{
			Debug.LogWarning ("Please key in enemy tag");
		}

		_playerTag = playerTag;
		_enemyTag = enemyTag;

		_player = GameObject.FindGameObjectWithTag(_playerTag);

			
	}


}

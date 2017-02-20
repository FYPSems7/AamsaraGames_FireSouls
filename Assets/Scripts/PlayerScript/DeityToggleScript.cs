using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeityToggleScript : MonoBehaviour {

	void Start()
	{
	}

	public void DeityToggle()
	{
		if (CharacterStatsScript.deityModeEnable == false) {
			CharacterStatsScript.deityModeEnable = true;
		} else if (CharacterStatsScript.deityModeEnable) {
			CharacterStatsScript.deityModeEnable = false;
		}
	}
}

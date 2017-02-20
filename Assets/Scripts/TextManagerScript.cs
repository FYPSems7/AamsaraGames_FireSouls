using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextManagerScript : MonoBehaviour {

	public static bool conversationStarted = false;
	public Text conversationText;
	public TextAsset textFile;
	public string[] textLines;

	public int currentLine;
	public int endAtLine;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if (conversationStarted) 
		{
			if (textFile != null) {
				textLines = (textFile.text.Split('\n'));
			}
			if (endAtLine == 0) {
				endAtLine = textLines.Length - 1;
			}
			conversationText.gameObject.SetActive (true);
			conversationText.text = textLines [currentLine];
			if (Input.GetKeyDown (KeyCode.Space)) {
				currentLine += 1;
			}

			if (currentLine > endAtLine) {
				conversationText.gameObject.SetActive (false);
				conversationStarted = false;

			}
		}
	}

	void LateUpdate()
	{
		if (conversationStarted == false) {
			textFile = null;
			currentLine = 0;
			endAtLine = 0;
			textLines = null;
		}
	}
}
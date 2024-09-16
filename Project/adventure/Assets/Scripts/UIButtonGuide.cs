using UnityEngine;
using System.Collections;

/// <summary>
/// Open Github Button click handler.
/// </summary>
public class UIButtonGuide : MonoBehaviour {


	public void showInstructionCanvas(GameObject instructionCanvas)
	{
		GameObject.FindWithTag("MainMenu").SetActive(false);
		instructionCanvas.SetActive(true);
	}

}
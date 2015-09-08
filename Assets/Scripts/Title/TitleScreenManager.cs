using UnityEngine;
using System.Collections;

// TITLE SCREENS, AM I RIGHT
//
// This has stuff that needs to happen whenever the user opens the app for the first time
public class TitleScreenManager : MonoBehaviour {

	public void Awake() {

	}

	public void StartLocalMultiplayer() {
		Application.LoadLevel("CharacterSelect");
	}

}

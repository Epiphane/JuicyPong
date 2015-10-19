using UnityEngine;
using System.Collections;

// TITLE SCREENS, AM I RIGHT
//
// This has stuff that needs to happen whenever the user opens the app for the first time
public class TitleScreenManager : MonoBehaviour {

	public static bool didSetup = false;
	public static void InitialGameSetup() {
		if (didSetup) {
			return;
		}

		didSetup = true;
		CharacterLevels.LoadSavedData();
		JuicePointManager.LoadJuice();
	}

	public void StartLocalMultiplayer() {
		Application.LoadLevel("CharacterSelect");
	}

    public void StartSinglePlayer() {
        Constants.singlePlayer = true;
        Application.LoadLevel("CharacterSelect");
    }

}

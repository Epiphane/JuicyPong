using UnityEngine;
using System.Collections;

// Singleton class to help us keep track of the experience/level
//  of each character
public class CharacterLevels : MonoBehaviour {

	public static int[] characterLevels = new int[7];
	public static int[] characterExperience = new int[7];  // Current experience towards the next level

	// Load the saved experience points and levels
	public static void LoadSavedData() {
		var characters = System.Enum.GetValues(typeof(Character));
		foreach (Character guy in characters) {
			characterLevels[(int)guy] = PlayerPrefs.GetInt("CharacterLevel" + (int)guy, 1);
			characterExperience[(int)guy] = PlayerPrefs.GetInt("CharacterExp" + (int)guy, 0);
		}
	}

}

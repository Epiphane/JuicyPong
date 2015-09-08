using UnityEngine;
using System.Collections;

// Singleton class to help us keep track of the experience/level
//  of each character
public class CharacterLevels : MonoBehaviour {

	// Defines how much EXP it takes to go up a level, at each level.
	public static int[] experiencePerLevel = {
		100, 150, 200, 250, 300, 400, 500, 600, 700, 800
	};

	// Store each characters' level and EXP here
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

	// Updates the stored values with anything new we've added
	public static void SaveCurrentValues() {
		var characters = System.Enum.GetValues(typeof(Character));
		foreach (Character guy in characters) {
			PlayerPrefs.SetInt ("CharacterLevel" + (int)guy, characterLevels[(int)guy]);
			PlayerPrefs.SetInt ("CharacterExp" + (int)guy, characterExperience[(int)guy]);
		}
	}

	// Add experience from the Win Screen.  If we leveled up, return true, otherwise return false
	public static bool AddExperience(int whichCharacter, int howMuch) {
		characterExperience[whichCharacter] += howMuch;

		// Check if leveled up
		var currCharacterLevel = characterLevels[whichCharacter];
		var nextLevelEXP = experiencePerLevel[currCharacterLevel-1];
		if (characterExperience[whichCharacter] > nextLevelEXP) {
			var remainder = characterExperience[whichCharacter] - nextLevelEXP;
			characterLevels[whichCharacter]++;

			characterExperience[whichCharacter] = remainder;
		
			return true;
		}

		return false;
	}

}

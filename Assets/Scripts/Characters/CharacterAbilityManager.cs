using UnityEngine;
using System.Collections;

public enum Character {
	Paul = 1, George, John, Dave, Ringo, Buster
}

// Hangs on to a bunch of static stuff which lets us
//  apply each character's abilities
public class CharacterAbilityManager : MonoBehaviour {

	public static Character[] selectedCharacter = new Character[3] {Character.Paul, Character.Paul, Character.Paul}; // selectedCharacter[1] contains Player 1's choice, selectedCharacter[2] contains Player 2's choice

	public static float[] powerupProgressMod = new float[3] {0f, 1f, 1f};
	public static float[] powerupLengthMod = new float[3] {0f, 1f, 1f};

	public static float[] ballSpeedMod = new float[3] {0f, 1f, 1f};
    public static float[] ballAccellMod = new float[3] { 0f, 1f, 1f };
	public static float[] paddleSizeMod = new float[3] {0f, 1.5f, 1.5f};

    /** Powerup Modifiers **/
	public static bool[] coinMagnetEnabled = new bool[3] {false, false, false};
    public static bool[] flamingShield = new bool[3] { false, false, false };
    public static bool[] wildFireballs = new bool[3] { false, false, false };

    /** Powerup Chance Modifiers **/
    public static float[] moreMagnets = new float[3] {0f, 1f, 1f};

	public static float[] autoShieldChance = new float[3] {0f, 0f, 0f};

	// Reset abilities between matches
	public static void ResetAbilities() {
		powerupProgressMod = new float[3] {0f, 1f, 1f};
		powerupLengthMod = new float[3] {0f, 1f, 1f};
		
		ballSpeedMod = new float[3] {0f, 1f, 1f};
		paddleSizeMod = new float[3] {0f, 1.5f, 1.5f};
		
		coinMagnetEnabled = new bool[3] {false, false, false};
		moreMagnets = new float[3] {0f, 1f, 1f};
		
		autoShieldChance = new float[3] {0f, 0f, 0f};
	}

	// Set the global values that are used to apply character abilities
	public static void ActivateCharacter(int playerNum, Character whichCharcter) {
		selectedCharacter[playerNum] = whichCharcter;

		switch (whichCharcter) {
		case Character.Paul:
			CharacterAbilityManager.powerupLengthMod[playerNum] = 1.1f;
			break;
		case Character.George:
			CharacterAbilityManager.paddleSizeMod[playerNum] *= 1.3f;
			break;
		case Character.John:
			CharacterAbilityManager.coinMagnetEnabled[playerNum] = true;
			CharacterAbilityManager.moreMagnets[playerNum] = 1.3f;
			break;
		case Character.Dave:
			CharacterAbilityManager.ballSpeedMod[playerNum] = 1.15f;
			break;
		case Character.Ringo:
			CharacterAbilityManager.powerupProgressMod[playerNum] = 1.05f;
			break;
		case Character.Buster:
			CharacterAbilityManager.autoShieldChance[playerNum] = 0.05f;
			break;
		}
	}
}

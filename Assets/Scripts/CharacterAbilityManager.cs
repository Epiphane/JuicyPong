using UnityEngine;
using System.Collections;

public enum Character {
	Paul, George, John, Dave, Ringo, Buster
}

// Hangs on to a bunch of static stuff which lets us
//  apply each character's abilities
public class CharacterAbilityManager : MonoBehaviour {

	public static float[] powerupProgressMod = new float[3] {0f, 1f, 1f};
	public static float[] powerupLengthMod = new float[3] {0f, 1f, 1f};

	public static float[] ballSpeedMod = new float[3] {0f, 1f, 1f};
	public static float[] paddleSizeMod = new float[3] {0f, 1.5f, 1.5f};

	public static float[] magnetAmplifier = new float[3] {0f, 1f, 1f};
	public static float[] moreMagnets = new float[3] {0f, 1f, 1f};

	public static float[] autoShieldChance = new float[3] {0f, 0f, 0f};

	// Reset abilities between matches
	public static void ResetChoices() {
		powerupProgressMod = new float[3] {0f, 1f, 1f};
		powerupLengthMod = new float[3] {0f, 1f, 1f};
		
		ballSpeedMod = new float[3] {0f, 1f, 1f};
		paddleSizeMod = new float[3] {0f, 1.5f, 1.5f};
		
		magnetAmplifier = new float[3] {0f, 1f, 1f};
		moreMagnets = new float[3] {0f, 1f, 1f};
		
		autoShieldChance = new float[3] {0f, 0f, 0f};
	}

	// Set the global values that are used to apply character abilities
	public static void ActivateCharacter(int playerNum, Character whichCharcter) {
		switch (whichCharcter) {
		case Character.Paul:
			CharacterAbilityManager.powerupLengthMod[playerNum] = 1.1f;
			break;
		case Character.George:
			CharacterAbilityManager.paddleSizeMod[playerNum] *= 1.2f;
			break;
		case Character.John:
			CharacterAbilityManager.magnetAmplifier[playerNum] = 1.3f;
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

using UnityEngine;
using System.Collections;

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


}

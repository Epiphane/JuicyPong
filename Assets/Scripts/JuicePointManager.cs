using UnityEngine;
using System.Collections;

public class JuicePointManager : MonoBehaviour {
	public static int totalJuice;

	public static bool firstWinAvailable = true;
	public static bool juiceBoostActive = false;

	public const int LOCAL_JUICE_BASE_REWARD = 50;

	public static void LoadJuice() {
		totalJuice = PlayerPrefs.GetInt("Juice", 0);
	}

	public static void GainJuice(int howMuch) {
		totalJuice += howMuch;
		PlayerPrefs.SetInt("Juice", totalJuice);
	}

	// Returns how much juice you get for a local multiplayer game.
	public static int LocalMultiplayerJuiceReward() {
		var result = LOCAL_JUICE_BASE_REWARD;
		if (firstWinAvailable) {
			result *= 2;
		}

		if (juiceBoostActive) {
			result *= 2;
		}

		return result;
	}
}

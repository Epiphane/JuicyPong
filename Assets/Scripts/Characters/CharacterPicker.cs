using UnityEngine;
using System.Collections;

public class CharacterPicker : MonoBehaviour {
	private bool[] chosen = new bool[3];
	private int[] selectedChar = new int[3];

	// Character was selected!  If both players have chosen, start the game.
	public void PlayerChoseCharacter(int whoChose, int characterIndex) {
		// (if local multiplayer)
		selectedChar[whoChose] = characterIndex;

		chosen[whoChose] = true;
		if (chosen[1] && chosen[2]) {
			Character player1Char = (Character) selectedChar[1];
			Character player2Char = (Character) selectedChar[2];

			CharacterAbilityManager.ActivateCharacter(1, player1Char);
			CharacterAbilityManager.ActivateCharacter(2, player2Char);
			Application.LoadLevel ("GameplayScene");
		}
	}
}

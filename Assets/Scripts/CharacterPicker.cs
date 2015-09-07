using UnityEngine;
using System.Collections;

public class CharacterPicker : MonoBehaviour {
	private bool[] chosen = new bool[2];

	public void Player1Chose1() {
		CharacterAbilityManager.ActivateCharacter(1, Character.Dave);
		PlayerChoseCharacter(1, 1);
	}

	public void Player1Chose2() {
		CharacterAbilityManager.ActivateCharacter(1, Character.Ringo);
		PlayerChoseCharacter(1, 2);
	}

	public void Player1Chose3() {
		CharacterAbilityManager.ActivateCharacter(1, Character.Buster);
		PlayerChoseCharacter(1, 3);
	}

	public void Player1Chose4() {
		CharacterAbilityManager.ActivateCharacter(1, Character.John);
		PlayerChoseCharacter(1, 4);
	}

	public void Player1Chose5() {
		CharacterAbilityManager.ActivateCharacter(2, Character.George);
		PlayerChoseCharacter(1, 5);
	}

	public void Player1Chose6() {
		CharacterAbilityManager.ActivateCharacter(2, Character.Paul);
		PlayerChoseCharacter(1, 6);
	}

	public void Player2Chose1() {
		CharacterAbilityManager.ActivateCharacter(2, Character.Dave);
		PlayerChoseCharacter(2, 1);
	}
	
	public void Player2Chose2() {
		CharacterAbilityManager.ActivateCharacter(2, Character.Ringo);
		PlayerChoseCharacter(2, 2);
	}
	
	public void Player2Chose3() {
		CharacterAbilityManager.ActivateCharacter(2, Character.Buster);
		PlayerChoseCharacter(2, 3);
	}
	
	public void Player2Chose4() {
		CharacterAbilityManager.ActivateCharacter(2, Character.John);
		PlayerChoseCharacter(2, 4);
	}
	
	public void Player2Chose5() {
		CharacterAbilityManager.ActivateCharacter(2, Character.George);
		PlayerChoseCharacter(2, 5);
	}
	
	public void Player2Chose6() {
		CharacterAbilityManager.ActivateCharacter(2, Character.Paul);
		PlayerChoseCharacter(2, 6);
	}

	// Character was selected!  If both players have chosen, start the game.
	public void PlayerChoseCharacter(int whoChose, int characterIndex) {
		// (if local multiplayer)
		var selectedButton = GameObject.Find ("MultiPlayerCanvas" + whoChose + "/Character Select Container/Character" + characterIndex);

		chosen[whoChose-1] = true;
		if (chosen[0] && chosen[1]) {
			Application.LoadLevel ("Scene");
		}
	}
}

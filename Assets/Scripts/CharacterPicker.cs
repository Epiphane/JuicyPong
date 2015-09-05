using UnityEngine;
using System.Collections;

public class CharacterPicker : MonoBehaviour {
	private int playersLeftChoosing = 2;

	public void Player1Chose1() {
		CharacterAbilityManager.ballSpeedMod[1] = 1.15f;
		NextScene();
	}
	public void Player1Chose2() {
		CharacterAbilityManager.powerupProgressMod[1] = 1.05f;
		NextScene();
	}
	public void Player1Chose3() {
		CharacterAbilityManager.autoShieldChance[1] = 0.05f;
		NextScene();
	}
	public void Player1Chose4() {
		CharacterAbilityManager.magnetAmplifier[1] = 1.3f;
		CharacterAbilityManager.moreMagnets[1] = 1.3f;
		NextScene();
	}
	public void Player1Chose5() {
		CharacterAbilityManager.paddleSizeMod[1] *= 1.2f;
		NextScene();
	}
	public void Player1Chose6() {
		CharacterAbilityManager.powerupLengthMod[1] = 1.1f;
		NextScene();
	}
	public void Player2Chose1() {
		CharacterAbilityManager.ballSpeedMod[2] = 1.15f;
		NextScene();
		
	}
	public void Player2Chose2() {
		CharacterAbilityManager.powerupProgressMod[2] = 1.05f;
		NextScene();
		
	}
	public void Player2Chose3() {
		CharacterAbilityManager.autoShieldChance[2] = 0.05f;
		NextScene();
		
	}
	public void Player2Chose4() {
		CharacterAbilityManager.magnetAmplifier[2] = 1.3f;
		CharacterAbilityManager.moreMagnets[2] = 1.3f;

		NextScene();
	}
	public void Player2Chose5() {
		CharacterAbilityManager.paddleSizeMod[2] *= 1.2f;
		NextScene();
		
	}
	public void Player2Chose6() {
		CharacterAbilityManager.powerupLengthMod[2] = 1.1f;
		NextScene();
	}

	public void NextScene() {
		playersLeftChoosing--;
		if (playersLeftChoosing == 0) {
			Application.LoadLevel ("Scene");
		}
	}
}

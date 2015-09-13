using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TouchScript.Gestures;
using System;

/// <summary>
/// Powerup UI handles all the UI for choosing and displaying powerups.
///   It has a progress bar at the bottom and handles the 'choice' buttons when they pop up.
///   Each player needs to have their own lil' version of the Powerup UI, one on 
///   each side of the screen.
/// </summary>

public class PowerupUI : MonoBehaviour {

	private int playerNum = 0;
	public PaddleObject playerObject;
	public Canvas myCanvas;
	public GameManager gameManager;
	public PowerupManager powerupManager;

	public EventSystem eventSystem;

	// Lets us change the width of the Green 'progress' indicator
	RectTransform innerBarTransform;
	Text choice1;
	PowerupType type1;
	Text choice2;
	PowerupType type2;
	int currChoice = 0;

	void Awake() {
		innerBarTransform = transform.Find("PowerupBar/Progress").GetComponent<RectTransform>();
		choice1 = transform.Find ("POWERUP 1").GetComponent<Text>();
		choice2 = transform.Find ("POWERUP 2").GetComponent<Text>();

		choice1.color = Color.clear;
		choice2.color = Color.clear;

		playerNum = playerObject.playerNum;
		playerObject.powerupUI = this;
	}

	// Update is called once per frame. YA DON'T SAY!!?
	void Update () {
		var progressFraction = Mathf.Min(1, playerObject.powerupProgress / 100);
		innerBarTransform.anchorMax = new Vector2(progressFraction, innerBarTransform.anchorMax.y);

		var dy = Input.GetAxisRaw("P" + playerNum + " Vertical");
		if (dy > 0.5) {
			eventSystem.SetSelectedGameObject(choice1.gameObject);
			choice1.color = Color.white;
			choice2.color = Color.white;
			currChoice = 1;
		}
		else if (dy < -0.5) {
			eventSystem.SetSelectedGameObject(choice2.gameObject);
			choice2.color = Color.white;
            choice1.color = Color.white;
			currChoice = 2;
		}

		if (choice1.GetComponent<Button>().interactable && Input.GetButtonDown ("SubmitPlayer" + playerNum)) {
			if (currChoice == 1) {
				ChosePowerup1();
			}
			else if (currChoice == 2) {
				ChosePowerup2();
			}
		}
	}

	/// <summary>
	/// Adds to the current powerup progress.
	/// </summary>
	/// <param name="amount">How much? (you get a powerup at 100)</param>
	public void AddPowerupProgress(float amount) {
		if (!GameManager.ShouldUpdate()) {
			return;
		}

		playerObject.powerupProgress += amount * CharacterAbilityManager.powerupProgressMod[playerNum];

		if (playerObject.powerupProgress >= 100) {
			// Powerup time!
			currChoice = 0;
			var choices = PowerupInfo.Choose2RandomPowerups(playerObject.playerNum);
			type1 = choices[0];
			type2 = choices[1];

			choice1.text = PowerupInfo.Name(type1);
			choice1.color = Color.white;
			
			choice2.text = PowerupInfo.Name(type2);
			choice2.color = Color.white;

            GameManager.gameState = GameState.ChoosePowerup;
			choice1.GetComponent<Button>().interactable = true;
			choice2.GetComponent<Button>().interactable = true;
        }
	}

	public void ChosePowerup1() {
		if (GameManager.gameState == GameState.ChoosePowerup && playerObject.powerupProgress >= 100f) {
			powerupManager.AddPowerup(playerNum, type1);
			StopChoosing();
		}
	}

	public void ChosePowerup2() {
		if (GameManager.gameState == GameState.ChoosePowerup && playerObject.powerupProgress >= 100f) {
			powerupManager.AddPowerup(playerNum, type2);
			StopChoosing();
		}
	}

	void StopChoosing() {
        GameManager.gameState = GameState.Playing;
		choice1.GetComponent<Button>().interactable = false;
		choice2.GetComponent<Button>().interactable = false;
		
		choice1.color = Color.clear;
		choice2.color = Color.clear;

        playerObject.powerupProgress = 0f;
    }
}

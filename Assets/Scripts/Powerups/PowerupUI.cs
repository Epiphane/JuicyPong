using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Powerup UI handles all the UI for choosing and displaying powerups.
///   It has a progress bar at the bottom and handles the 'choice' buttons when they pop up.
///   Each player needs to have their own lil' version of the Powerup UI, one on 
///   each side of the screen.
/// </summary>

public class PowerupUI : MonoBehaviour {

	public int player;
	public float powerupProgress = 0f;
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
	}
	
	// Update is called once per frame. YA DON'T SAY!!?
	void Update () {
		var totalWidth = Screen.width / 2;
		var progressFraction = Mathf.Min(1, powerupProgress / 100);
		var barWidth = totalWidth * progressFraction;
		var insetWidth = totalWidth - barWidth;

		innerBarTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, insetWidth, barWidth);
		
		var dy = Input.GetAxisRaw("P" + player + " Vertical");
		if (dy > 0.5) {
			eventSystem.SetSelectedGameObject(choice1.gameObject);
			choice1.color = Color.white;
			choice2.color = Color.gray;
			currChoice = 1;
		}
		else if (dy < -0.5) {
			eventSystem.SetSelectedGameObject(choice2.gameObject);
			choice2.color = Color.white;
			choice1.color = Color.gray;
			currChoice = 2;
		}

		if (choice1.GetComponent<Button>().interactable && Input.GetButtonDown ("SubmitPlayer" + player)) {
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
		if (!gameManager.ShouldUpdate()) {
			return;
		}

		powerupProgress += amount;

		if (powerupProgress >= 100) {
			// Powerup time!
			currChoice = 0;
			var choices = PowerupInfo.Choose2RandomPowerups();
			type1 = choices[0];
			type2 = choices[1];

			choice1.text = PowerupInfo.Name(type1);
			choice1.color = Color.gray;
			
			choice2.text = PowerupInfo.Name(type2);
			choice2.color = Color.gray;

			gameManager.gameState = GameState.ChoosePowerup;
			choice1.GetComponent<Button>().interactable = true;
			choice2.GetComponent<Button>().interactable = true;
		}
	}

	public void ChosePowerup1() {
		powerupManager.AddPowerup(player, type1);
		StopChoosing();
	}

	public void ChosePowerup2() {
		powerupManager.AddPowerup(player, type2);
		StopChoosing();
	}

	void StopChoosing() {
		gameManager.gameState = GameState.Playing;
		choice1.GetComponent<Button>().interactable = false;
		choice2.GetComponent<Button>().interactable = false;
		
		choice1.color = Color.clear;
		choice2.color = Color.clear;
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum WinScreenState {
	EXPScreenLevelupPlayer1,  // Player leveled up - must choose new skill
	EXPScreenLevelupPlayer2,

	EXPScreenIncrementPlayer1, // Adding EXP to the player
	EXPScreenIncrementPlayer2,

	EXPScreenContinueReady,

	JuiceScreen,
}

public class WinScreenUI : MonoBehaviour {

	public float testAnimation;
	public WinScreenState winScreenState;

	public Text player1WinLabel, player2WinLabel;
	public Image player1Portrait, player2Portrait;

	public Text player1LevelLabel, player2LevelLabel;
	public Text player1GainedExpLabel, player2GainedExpLabel;
	public Text player1TotalExpLabel, player2TotalExpLabel;

	public RectTransform player1ExpProgress, player2ExpProgress;

	public float[] animDelay = new float[3] {0f, 0.5f, 0.2f};  // # of seconds before the "+X Exp" drains into the progress bar
	private float[] tickTime = new float[3] {0f, 0f, 0f};

	private int[] pendingEXP = new int[3] {0, 0, 0};
	private float[] targetBarFraction = new float[3] {0, 0, 0};
	private float[] actualBarFraction = new float[3] {0, 0, 0};
	private RectTransform[] progressBarTransforms = new RectTransform[3];

	public Animator expPanelAnimation;

	private Character[] selectedChar;

	// Populate win screen info labels
	void Start () {
		Time.timeScale = 1;
		winScreenState = WinScreenState.EXPScreenIncrementPlayer1;
		TitleScreenManager.InitialGameSetup();

		if (ScoreManager.winner == 1){
			pendingEXP[1] = 50;
			pendingEXP[2] = 30;
		}
		else {
			pendingEXP[1] = 30;
			pendingEXP[2] = 50;
		}

		progressBarTransforms = new RectTransform[3] { null, player1ExpProgress, player2ExpProgress };

		Character player1 = CharacterAbilityManager.selectedCharacter[1];
		Character player2 = CharacterAbilityManager.selectedCharacter[2];
		selectedChar = new Character[3] {Character.Ringo, player1, player2};

		targetBarFraction[1] = actualBarFraction[1] = CharacterLevels.CharacterLevelProgress(player1, 0);
		targetBarFraction[2] = actualBarFraction[2] = CharacterLevels.CharacterLevelProgress(player2, 0);

		player1WinLabel.text = selectedChar[1].ToString() + ((ScoreManager.winner == 1) ? " WINS!" : " LOST!");
		player2WinLabel.text = selectedChar[2].ToString() + ((ScoreManager.winner == 2) ? " WINS!" : " LOST!");

		player1LevelLabel.text = "LEVEL " + CharacterLevels.characterLevels[1];
		player2LevelLabel.text = "LEVEL " + CharacterLevels.characterLevels[2];

		UpdateProgressBars();

		(FindObjectOfType(typeof(JuiceScreenUI)) as JuiceScreenUI).CalculateJuice();
	}
	
	// Update is called once per frame
	void Update () {
		if (winScreenState == WinScreenState.EXPScreenIncrementPlayer1) {
			UpdatePlayerEXP(1);
			UpdateProgressBars();
		}
		else if (winScreenState == WinScreenState.EXPScreenIncrementPlayer2) {
			UpdatePlayerEXP(2);
			UpdateProgressBars();
		}
		// .... TODO: all the rest of the states here :3
	}

	private void UpdatePlayerEXP(int playerNdx) {
		animDelay[playerNdx] -= Time.deltaTime;
		if (animDelay[playerNdx] <= 0) {

			tickTime[playerNdx] -= Time.deltaTime;
			if (tickTime[playerNdx] <= 0) {

				if (pendingEXP[playerNdx] <= 0) {
					int newState = (int) winScreenState; // increment winScreenState
					newState++;
					winScreenState = (WinScreenState) newState; 
				}
				else {
					tickTime[playerNdx] = 0.1f; // Decrement pending EXP and add it to the current player
					pendingEXP[playerNdx] -= 5;
					if (CharacterLevels.AddExperience(selectedChar[playerNdx], 5)) {
						targetBarFraction[playerNdx] = 1.0f;
						DoLevelUp(playerNdx);
					}
					else {
						targetBarFraction[1] = CharacterLevels.CharacterLevelProgress(selectedChar[1], pendingEXP[1]);
						targetBarFraction[2] = CharacterLevels.CharacterLevelProgress(selectedChar[2], pendingEXP[2]);
					}
				}
			}
		}
	}

	// Animate the progress bar to be closer to what it should be
	private void UpdateProgressBars() {
		for (int playerNdx = 1; playerNdx <= 2; playerNdx++) {
			var diff = targetBarFraction[playerNdx] - actualBarFraction[playerNdx];
			actualBarFraction[playerNdx] += diff/8;

			progressBarTransforms[playerNdx].anchorMax = new Vector2(actualBarFraction[playerNdx], progressBarTransforms[playerNdx].anchorMax.y);
		}

		player1GainedExpLabel.text = "+" + pendingEXP[1] + " EXP";
		player2GainedExpLabel.text = "+" + pendingEXP[2] + " EXP";

		var player1Level = CharacterLevels.characterLevels[(int) selectedChar[1]];
		var player2Level = CharacterLevels.characterLevels[(int) selectedChar[2]];
		player1TotalExpLabel.text = "EXP: " + CharacterLevels.characterExperience[(int) selectedChar[1]] + " / " + CharacterLevels.experiencePerLevel[player1Level];
		player2TotalExpLabel.text = "EXP: " + CharacterLevels.characterExperience[(int) selectedChar[2]] + " / " + CharacterLevels.experiencePerLevel[player2Level];
	}

	// User got to.. THE NEXT LEVEL OMG
	private void DoLevelUp(int toWhichPlayer) {
		if (toWhichPlayer == 1) {
			winScreenState = WinScreenState.EXPScreenLevelupPlayer1;
		}
		else {
			winScreenState = WinScreenState.EXPScreenLevelupPlayer2;
		}


	}
	

	public void ContinueButtonPressed() {
		if (winScreenState == WinScreenState.EXPScreenContinueReady) {
			expPanelAnimation.SetTrigger("ShowJuice");
			Invoke ("SqueezeJuice", 1.5f);
		}
	}

	public void SqueezeJuice() {
		(FindObjectOfType(typeof(JuiceScreenUI)) as JuiceScreenUI).SqueezeJuice();
	}
}

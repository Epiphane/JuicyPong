using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum WinScreenState {
	EXPScreenNoContinue, // Don't let user hit Continue if there's a pending level-up
	EXPScreenLevelUp,
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

	public Image player1ExpBorder, player2ExpBorder;
	public Image player1ExpProgress, player2ExpProgress;

	private float player1AnimDelay = 0.5f;  // # of seconds before the "+X Exp" drains into the progress bar
	private float player2AnimDelay = 1.2f;
	private float player1AnimProgress = 0f;
	private float player2AnimProgress = 0f;

	private float player1TargetBarPercentage = 0f, player2TargetBarPercentage = 0f;


	// Use this for initialization
	void Start () {


		GetComponent<Text>().text = "PLAYER " + ScoreManager.winner + " WINS AHHHHHH";
	}
	
	// Update is called once per frame
	void Update () {
		player1AnimDelay -= Time.deltaTime;
		player2AnimDelay -= Time.deltaTime;
	
		if (player1AnimDelay <= 0) {

		}

		if (player2AnimDelay <= 0) {

		}
	}

	

	public void ContinueButtonPressed() {

	}
}

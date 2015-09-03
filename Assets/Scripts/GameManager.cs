using UnityEngine;
using System.Collections;

public enum GameState {
	GetReady,
	Playing,
	ChoosePowerup,
	Win,
	Lose,
}

public class GameManager : MonoBehaviour {
	public Transform player1;
	public Transform player2;
	public BallScript ballScript;
	
	public GUIText CountdownGUI;
	public float timer = 3;
	private float lastTime;

	public GameState gameState = GameState.Playing;
	
	void Awake() {
//		Time.timeScale = 0;
	}
	
	public void Reset() {
		timer = 3;
//		Time.timeScale = 0;
		Time.timeScale = 1;
		
		lastTime = Time.realtimeSinceStartup;

		var paddleOffset = Constants.FIELD_WIDTH_2 * 0.8f;
		player1.position = new Vector3(-paddleOffset, 0, -10);
		player2.position = new Vector3(paddleOffset, 0, -10);

		player1.GetComponent<PaddleObject> ().PADDLE_X = -paddleOffset;
		player2.GetComponent<PaddleObject> ().PADDLE_X = paddleOffset;
		
		ballScript.Start();
	}
	
	public void Start () {
		Reset();
	}
	
	void Update () {
//		if (timer > -1) {
//			timer -= (Time.realtimeSinceStartup - lastTime);
//			lastTime = Time.realtimeSinceStartup;
//			
//			if (timer > 0) {
//				CountdownGUI.text = "" + Mathf.Ceil(timer);
//			}
//			else {
//				Time.timeScale = 1;
//				CountdownGUI.text = "GO!";
//			}
//		}
//		else {
//			CountdownGUI.text = "";
//		}
	}

	// Helps us stop movement if the user is choosing a powerup or the game is over
	public bool ShouldUpdate() {
		if (gameState == GameState.Playing) {
			return true;
		}
		return false;
	}
}

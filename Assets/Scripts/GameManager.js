#pragma strict

public var Player1 : Transform;
public var Player2 : Transform;
public var ballScript : BallScript;

public var CountdownGUI : GUIText;
public var timer : float = 3;
private var lastTime : float;

function Awake() {
	Time.timeScale = 0;
}

function Reset() {
	timer = 3;
	Time.timeScale = 0;
	
	lastTime = Time.realtimeSinceStartup;
	
	Player1.position.x = -5;
	Player1.position.y = 0;
	Player2.position.x = 5;
	Player2.position.y = 0;
	
	ballScript.Start();
}

public function Start () {
	Reset();
}

function Update () {
	if (timer > -1) {
		timer -= (Time.realtimeSinceStartup - lastTime);
		lastTime = Time.realtimeSinceStartup;
		
		if (timer > 0) {
			CountdownGUI.text = "" + Mathf.Ceil(timer);
		}
		else {
			Time.timeScale = 1;
			CountdownGUI.text = "GO!";
		}
	}
	else {
		CountdownGUI.text = "";
	}
}
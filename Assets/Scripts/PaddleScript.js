#pragma strict

public static var GAME_HEIGHT : float = 5.3;
public var speed : int = 10;
public var player : int = 1;

function Start () {
	
}

function FixedUpdate () {
	transform.position.y += Input.GetAxisRaw("P" + player + " Vertical") * Time.deltaTime * speed;
	
	if (transform.position.y < -GAME_HEIGHT / 2) {
		transform.position.y = -GAME_HEIGHT / 2;
	}
	else if (transform.position.y > GAME_HEIGHT / 2) {
		transform.position.y = GAME_HEIGHT / 2;
	}
}
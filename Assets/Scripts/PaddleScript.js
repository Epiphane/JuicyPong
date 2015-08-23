#pragma strict

public var speed : int = 10;
public var player : int = 1;
public var dy : float = 0;

function Start () {
}

function FixedUpdate () {
	dy = Input.GetAxisRaw("P" + player + " Vertical") * speed;
	transform.position.y += dy * Time.deltaTime;
	
	if (transform.position.y < -Constants.FIELD_HEIGHT_2) {
		transform.position.y = -Constants.FIELD_HEIGHT_2;
	}
	else if (transform.position.y > Constants.FIELD_HEIGHT_2) {
		transform.position.y = Constants.FIELD_HEIGHT_2;
	}
}
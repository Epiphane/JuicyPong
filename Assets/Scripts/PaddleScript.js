#pragma strict

public var speed : int = 0.5;
public var player : int = 1;

function Start () {
	
}

function FixedUpdate () {
	transform.position.y += Input.GetAxisRaw("P" + player + " Vertical") * Time.deltaTime * speed;
}
#pragma strict

public static var GAME_HEIGHT : float = 5.3;
public static var GAME_WIDTH : float = 12;
public var speed : int = 10;
private var direction : Vector2;

function Start () {
	direction = new Vector2(Random.value * 6 - 5, Random.value - 0.5);
	direction.Normalize();
	
	Debug.Log(direction);
}

function FixedUpdate () {
	transform.position.x += direction.x * Time.deltaTime * speed;
	transform.position.y += direction.y * Time.deltaTime * speed;
	
	if (transform.position.y < -GAME_HEIGHT / 2) {
		transform.position.y = -GAME_HEIGHT / 2;
		direction.y *= -1;
	}
	else if (transform.position.y > GAME_HEIGHT / 2) {
		transform.position.y = GAME_HEIGHT / 2;
		direction.y *= -1;
	}
	
	if (transform.position.x < -GAME_WIDTH / 2) {
		transform.position.x = -GAME_WIDTH / 2;
		direction.x *= -1;
	}
	else if (transform.position.x > GAME_WIDTH / 2) {
		transform.position.x = GAME_WIDTH / 2;
		direction.x *= -1;
	}
}

function OnTriggerEnter2D (other : Collider2D) {
	direction.x *= -1;
}
#pragma strict

public static var GAME_HEIGHT : float = 5.3;
public static var GAME_WIDTH : float = 12;
public var speed : int = 10;
private var direction : Vector2;

function NormalizeDirection () {
	if (Mathf.Abs(direction.x) * 3 < Mathf.Abs(direction.y) *  2) {
		direction.x = Mathf.Sign(direction.x) * 2 / 3 * Mathf.Abs(direction.y);
	}

	direction.Normalize();
}

function Start () {
	direction = new Vector2(Random.value * 6 - 5, Random.value - 0.5);
	NormalizeDirection();
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
	
	var boxCollider : BoxCollider2D = other as BoxCollider2D;
	if (boxCollider) {
		var dy = transform.position.y - boxCollider.transform.position.y * 2 / boxCollider.size.y;
		
		direction.y += Mathf.Pow(dy, 5);
		NormalizeDirection();
	}
}
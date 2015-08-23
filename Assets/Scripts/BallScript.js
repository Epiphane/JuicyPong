#pragma strict

public var ScoreManager : ScoreManager;
public var speed : int = 10;
private var direction : Vector2;

function NormalizeDirection () {
	if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y)) {
		direction.x = Mathf.Sign(direction.x) * Mathf.Abs(direction.y);
	}

	direction.Normalize();
}

public function Start () {
	direction = new Vector2(Random.value * 6 - 5, Random.value - 0.5);
	NormalizeDirection();
		
	transform.position.x = transform.position.y = 0;
}

function FixedUpdate () {
	transform.position.x += direction.x * Time.deltaTime * speed;
	transform.position.y += direction.y * Time.deltaTime * speed;
	
	if (transform.position.y < -Constants.FIELD_HEIGHT_2) {
		transform.position.y = -Constants.FIELD_HEIGHT_2;
		direction.y *= -1;
	}
	else if (transform.position.y > Constants.FIELD_HEIGHT_2) {
		transform.position.y = Constants.FIELD_HEIGHT_2;
		direction.y *= -1;
	}
	
	if (transform.position.x < -Constants.FIELD_WIDTH_2) {
		ScoreManager.GetPoint(2);
	}
	else if (transform.position.x > Constants.FIELD_WIDTH_2) {
		ScoreManager.GetPoint(1);
	}
}

function OnTriggerEnter2D (other : Collider2D) {
	direction.x *= -1;
	
	var boxCollider : BoxCollider2D = other as BoxCollider2D;
	if (boxCollider) {
		var dy = transform.position.y - boxCollider.transform.position.y * 2 / boxCollider.size.y;
		
		var paddle : PaddleScript = boxCollider.GetComponent("PaddleScript") as PaddleScript;
		if (paddle) {
			dy += Mathf.Sign(paddle.dy);
		}
		
		direction.y += dy / 2;
		NormalizeDirection();
	}
}
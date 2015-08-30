using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {
	public ScoreManager scoreManager;
	public float speed;
	public float baseSpeed;
	public Vector3 direction;
	private Animator animator;

	public GameManager manager;

	public ParticleSystem particles;

	private GameObject visibleSprite;

	public bool ghost = false;
	public bool flamin = false;
	public bool icy = false;

	public void NormalizeDirection () {
		if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y)) {
			direction.x = Mathf.Sign(direction.x) * Mathf.Abs(direction.y);
		}
		
		direction.Normalize();
	}

	public void Start () {
		direction = new Vector2(Random.value * 6f - 5f, Random.value - 0.5f);
		animator = GetComponent<Animator> ();
		
		NormalizeDirection();
		
		transform.position = new Vector3(0, 0, -10);
		visibleSprite = transform.Find("ballSprite").gameObject;

		baseSpeed = speed;
	}


	// Check if hit walls
	public void FixedUpdate () {
		if (!manager.ShouldUpdate()) {
			return;
		}

		if (flamin) { // flames > ice.  They're "cooler" ahahaha
			transform.position += direction * Time.deltaTime * speed * PowerupInfo.FIREBALL_SPEED_MULT;
		}
		else if (icy) {
			transform.position += direction * Time.deltaTime * speed * PowerupInfo.ICEBALL_SPEED_MULT;
		}
		else {
			transform.position += direction * Time.deltaTime * speed;
		}

		// Hit Bottom Ceiling
		if (transform.position.y < -Constants.FIELD_HEIGHT_2) {
			var pos = transform.position;
			pos.y = -Constants.FIELD_HEIGHT_2;
			transform.position = pos;
			direction.y *= -1;
			particles.Emit(30);
			animator.SetTrigger("BallHitCeiling");
		}
		// Hit Top Ceiling
		else if (transform.position.y > Constants.FIELD_HEIGHT_2) {
			var pos = transform.position;
			pos.y = Constants.FIELD_HEIGHT_2;
			transform.position = pos;

			direction.y *= -1;
			particles.Emit(30);
			animator.SetTrigger("BallHitCeiling");
		}

		// Hit score-zoooone
		if (transform.position.x < -Constants.FIELD_WIDTH_2) {
			scoreManager.GetPoint(2);  // get in the zooone
		}
		else if (transform.position.x > Constants.FIELD_WIDTH_2) {
			scoreManager.GetPoint(1);  // scoooore-o zooooone
		}
	}

	void LateUpdate() {
		// Make ball sprite-face point forward
		var angle = Mathf.Atan2(direction.y, direction.x);
		visibleSprite.transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.forward);

		if (ghost) {
			visibleSprite.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
		}
		else if (flamin) { // flames > ice.  They're "cooler" ahahaha
			visibleSprite.GetComponent<SpriteRenderer>().color = Color.red;
		}
		else if (icy) {
			visibleSprite.GetComponent<SpriteRenderer>().color = Color.blue;
		}
		else {
			visibleSprite.GetComponent<SpriteRenderer>().color = Color.white;
		}
	} 

	// Hit paddle
	public void OnTriggerEnter2D (Collider2D other) {
		if (!manager.ShouldUpdate()) {
			return;
		}


		var boxCollider = other as BoxCollider2D;
		var paddle = boxCollider.GetComponent<PaddleScript>();
		if (boxCollider && paddle) {

			direction.x *= -1;

			var dy = transform.position.y - boxCollider.transform.position.y * 2 / boxCollider.size.y;

			
			if (flamin) { flamin = false; }
			if (icy)    { icy = false; }

			if (paddle) {
				dy += Mathf.Sign(paddle.dy);
				paddle.HitBall();
				direction.y += dy / 2;
			}

			NormalizeDirection();
			
			animator.SetTrigger("BallHitPaddle");
		}
	}
}

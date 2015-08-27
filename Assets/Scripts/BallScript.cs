using UnityEngine;
using System.Collections;


public class BallScript : MonoBehaviour {
	public ScoreManager scoreManager;
	public int speed;
	private Vector3 direction;
	private Animator animator;

	public ParticleSystem particles;

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
	}

	public void FixedUpdate () {
		transform.position += direction * Time.deltaTime * speed;
		
//		transform.Rotate(Vector3.forward * -30);
		
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
		
		if (transform.position.x < -Constants.FIELD_WIDTH_2) {
			scoreManager.GetPoint(2);
		}
		else if (transform.position.x > Constants.FIELD_WIDTH_2) {
			scoreManager.GetPoint(1);
		}
	}

	public void OnTriggerEnter2D (Collider2D other) {
		direction.x *= -1;

		var boxCollider = other as BoxCollider2D;
		if (boxCollider) {
			var dy = transform.position.y - boxCollider.transform.position.y * 2 / boxCollider.size.y;
			
			var paddle = boxCollider.GetComponent<PaddleScript>();
			if (paddle) {
				dy += Mathf.Sign(paddle.dy);
			}
			
			direction.y += dy / 2;
			NormalizeDirection();
			
			animator.SetTrigger("BallHitPaddle");
		}
	}
}

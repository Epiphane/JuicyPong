using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallScript : MonoBehaviour {
	public ScoreManager scoreManager;
	public float speed;
	public float baseSpeed;
	public Vector3 direction;

	private Vector2 lastPosition;

	private Animator animator;

	public GameManager manager;
	public PaddleObject lastHitPlayer;
	private List<ParticleSystem> pendingParticles = new List<ParticleSystem>();

	private GameObject visibleSprite;

	public bool ghost = false;
	public bool flamin = false;
	public bool icy = false;

	// Farticles (lol farts)
	public GameObject impactParticles;

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

		// Release all pending particles
		foreach (var particle in pendingParticles) {
			particle.Play();
			animator.SetTrigger("BallHitPaddle");
		}

		pendingParticles.Clear();

		if (flamin) { // flames > ice.  They're "cooler" ahahaha
			transform.position += direction * Time.deltaTime * speed * PowerupInfo.FIREBALL_SPEED_MULT;
		}
		else if (icy) {
			transform.position += direction * Time.deltaTime * speed * PowerupInfo.ICEBALL_SPEED_MULT;
		}
		else {
			transform.position += direction * Time.deltaTime * speed;
		}

		// Hit score-zoooone
		if (transform.position.x < -Constants.FIELD_WIDTH_2) {
			scoreManager.GetPoint(2);  // get in the zooone
		}
		else if (transform.position.x > Constants.FIELD_WIDTH_2) {
			scoreManager.GetPoint(1);  // scoooore-o zooooone
		}
	}

	// The ball's trajectory can be modified by other stuff (magnets)
	//  Make sure its direction is correct  
	public void AdjustDirection() {

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

		lastPosition = transform.position;
	} 

	public void OnCollisionEnter2D(Collision2D collision) {
		if (!manager.ShouldUpdate()) {
			return;
		}

		// HIT CEILING
		if (collision.gameObject.tag == "Ceiling") {
			foreach (ContactPoint2D contact in collision.contacts) {
				var particles = Instantiate (impactParticles, contact.point, Quaternion.identity) as GameObject;
				var angle = Mathf.Atan2(contact.normal.x, contact.normal.y);
				particles.transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.forward);
				particles.GetComponent<ParticleSystem>().startColor = visibleSprite.GetComponent<SpriteRenderer>().color;
			}
			animator.SetTrigger("BallHitCeiling");
			direction.y *= -1;
		}
		
		// HIT PADDLE
		if (collision.gameObject.tag == "Player") {
			
			if (flamin) { flamin = false; }
			if (icy)    { icy = false; }
			
			var paddle = collision.gameObject.GetComponent<PaddleObject>();

			direction.x *= -1;
			lastHitPlayer = paddle;
			paddle.HitBall();
			
			var dy = transform.position.y - collision.gameObject.transform.position.y;
			direction.y = dy * 2;
			
			NormalizeDirection();

			foreach (ContactPoint2D contact in collision.contacts) {
				var particleObject = Instantiate (impactParticles, contact.point, Quaternion.identity) as GameObject;
				var particles = particleObject.GetComponent<ParticleSystem>();
				var angle = Mathf.Atan2(contact.normal.x, contact.normal.y);
				particleObject.transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * -angle, Vector3.forward);
				particles.startColor = visibleSprite.GetComponent<SpriteRenderer>().color;
				particles.Pause ();

				pendingParticles.Add(particles);
			}
		}
	}

	public void OnTriggerEnter2D (Collider2D other) {
		if (!manager.ShouldUpdate()) {
			return;
		}

		// HIT COIN
		var coin = other.GetComponent<CoinObject>();
		if (coin && lastHitPlayer) {
			if (lastHitPlayer.powerupUI) {
				lastHitPlayer.powerupUI.AddPowerupProgress(50);
			}
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallScript : MonoBehaviour {
	public ScoreManager scoreManager;
	public float speed {
		get {
			var abilitySpeedup = 1f;
			if (lastHitPlayer != null) {
				abilitySpeedup = CharacterAbilityManager.ballSpeedMod[lastHitPlayer.playerNum];
			}
			return baseSpeed * perGameSpeedup * perRoundSpeedup * abilitySpeedup;
		}
	}
	public float baseSpeed;

	// Speeds up the ball as the entire game progresses
	private float perGameSpeedup = 1f;
	// Speeds up the ball per POINT played
	private float perRoundSpeedup = 1f;

	public Vector3 direction;

	public ParticleSystem buttParticles; // hehe butts

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

	// Make sure ball doesn't bounce at too steep of an angle, that's annoying
	public Vector3 LimitAngle(Vector3 angleDirection) {
		if (Mathf.Abs(angleDirection.x) < Mathf.Abs(angleDirection.y)) {
			angleDirection.x = Mathf.Sign(angleDirection.x) * Mathf.Abs(angleDirection.y);
		}

		return angleDirection;
	}

	public void NormalizeDirection () {
		direction = LimitAngle(direction);
		direction.Normalize();
	}

	public void Start () {
		direction = new Vector2(Random.value * 6f - 5f, Random.value - 0.5f);
		animator = GetComponent<Animator> ();
		
		NormalizeDirection();
		
		transform.position = new Vector3(0, 0, -10);
		visibleSprite = transform.Find("ballSprite").gameObject;
	}


	// Check if hit walls
	public void FixedUpdate () {
		if (!GameManager.ShouldUpdate()) {
			buttParticles.Stop();
			return;
		}
		else {
			if (!buttParticles.isPlaying) {
				buttParticles.Play ();
			}
		}

		// Upgrade to magnet paddle makes ball attract coins - do that here
		if (lastHitPlayer && lastHitPlayer.magnetized && CharacterAbilityManager.coinMagnetEnabled[lastHitPlayer.playerNum]) {
			CoinManager coins = (CoinManager)(FindObjectOfType(typeof(CoinManager)));
			foreach (GameObject coin in coins.currentCoins) {
				var distance = Vector3.Distance(coin.transform.position, transform.position);

				if (distance < 3) {
					var influence = 3/(distance + 0.1f) * 0.01f;
					var diff = transform.position - coin.transform.position;

					coin.transform.position += diff * influence;
				}
			}
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
			if (Random.value < CharacterAbilityManager.autoShieldChance[1]) {
				direction.x *= -1;
				print ("AUTO SHIIIEEEEELLLLLD");
			}
			else {
				scoreManager.GetPoint(2);  // get in the zooone
				PointScored();
			}
		}
		else if (transform.position.x > Constants.FIELD_WIDTH_2) {
			if (Random.value < CharacterAbilityManager.autoShieldChance[2]) {
				direction.x *= -1;
				print ("AUTO SHIIIEEEEELLLLLD");
			}
			else {
				scoreManager.GetPoint(1);  // scoooore-o zooooone
				PointScored();
			}
		}
	}

	private void PointScored() {
		perRoundSpeedup = 1f;
		perGameSpeedup *= 1.03f;

		flamin = icy = false;

        FindObjectOfType<ScreenShaker>().ShakeScreen(0.7f);
	}

	// The ball's trajectory can be modified by other stuff (magnets)
	//  Influence goes from 0 -> 1
	public void MagnetTowardsPoint(Vector3 target, float influence) {
		Vector3 influenceDirection = LimitAngle(target - transform.position);

		if (Mathf.Sign (influenceDirection.x) != Mathf.Sign (direction.x)) {
			influenceDirection.x *= -1f;
		}

		influenceDirection.Normalize();
		influenceDirection *= influence;

		direction = (influenceDirection + direction).normalized;
		direction = LimitAngle(direction);
	}

	void LateUpdate() {
		// Make ball sprite-face point forward
		var angle = Mathf.Atan2(direction.y, direction.x);
		visibleSprite.transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.forward);

		if (ghost) {
			visibleSprite.GetComponent<SpriteRenderer>().color = PowerupInfo.GHOST_COLOR;
			buttParticles.startColor = PowerupInfo.GHOST_COLOR;
		}
		else if (flamin) { // flames > ice.  They're "cooler" ahahaha
			visibleSprite.GetComponent<SpriteRenderer>().color = Color.red;
			buttParticles.startColor = Color.red;
		}
		else if (icy) {
			visibleSprite.GetComponent<SpriteRenderer>().color = Color.blue;
			buttParticles.startColor = Color.blue;
		}
		else {
			visibleSprite.GetComponent<SpriteRenderer>().color = Color.white;
			buttParticles.startColor = Color.white;
        }

        GetComponent<JuicyShadow>().ghostlyShadow = ghost;
    } 

	public void OnCollisionEnter2D(Collision2D collision) {
		if (!GameManager.ShouldUpdate()) {
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

			perRoundSpeedup *= 1.05f;
			
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
		if (!GameManager.ShouldUpdate()) {
			return;
		}

		// HIT COIN
		var coin = other.GetComponent<CoinObject>();
		if (coin && lastHitPlayer) {
			if (lastHitPlayer.powerupUI) {
				lastHitPlayer.powerupUI.AddPowerupProgress(50);
			}
		}

		var shield = other.GetComponent<ShieldObject>();
		if (shield) {
			direction.x *= -1;
		}
	}
}

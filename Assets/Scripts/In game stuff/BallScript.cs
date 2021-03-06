﻿using UnityEngine;
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

    // Stop annoying glitch where it can get stuck in a wall
    private float BASE_BOUNCE_COOLDOWN = 1.0f;
    private float bounceCooldown = 0.0f;

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

	public bool flamin = false;
    public float iciness = 0.0f; // 0-1 how icy the ball is. 1 is fully blue + speed cut in half
    public float ghostliness = 0.0f; // 0-1 how invisible the ball is.

    public GameObject impactParticles;

    private Collider2D topCeiling, bottomCeiling;

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

    public void Start() {
        direction = new Vector2(Random.value * 6f - 5f, Random.value - 0.5f);
        animator = GetComponent<Animator>();

        NormalizeDirection();

        transform.position = new Vector3(0, 0, -10);
        visibleSprite = transform.Find("ballSprite").gameObject;

        topCeiling = GameObject.Find("TopCeiling").GetComponent<BoxCollider2D>();
        bottomCeiling = GameObject.Find("BottomCeiling").GetComponent<BoxCollider2D>();
    }


    // Check if hit walls
    public void FixedUpdate() {
        if (!GameManager.ShouldUpdate()) {
            buttParticles.Stop();
            return;
        }
        else {
            bounceCooldown -= Time.deltaTime;
            if (!buttParticles.isPlaying) {
                buttParticles.Play();
            }
        }

        // Upgrade to magnet paddle makes ball attract coins - do that here
        if (lastHitPlayer && lastHitPlayer.magnetized && CharacterAbilityManager.coinMagnetEnabled[lastHitPlayer.playerNum]) {
            CoinManager coins = (CoinManager)(FindObjectOfType(typeof(CoinManager)));
            foreach (GameObject coin in coins.currentCoins) {
                var distance = Vector3.Distance(coin.transform.position, transform.position);

                if (distance < 3) {
                    var influence = 3 / (distance + 0.1f) * 0.01f;
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

        var currSpeed = speed - speed * PowerupInfo.ICEBALL_SPEED_MULT * iciness;

        if (flamin) { // flames > ice.  They're "cooler" ahahaha
            transform.position += direction * Time.deltaTime * currSpeed * PowerupInfo.FIREBALL_SPEED_MULT;
        }
        else {
            transform.position += direction * Time.deltaTime * currSpeed;
        }

        if (topCeiling.IsTouching(GetComponent<BoxCollider2D>()) || bottomCeiling.IsTouching(GetComponent<BoxCollider2D>())) {
            direction.y *= -1; // Get out of the ceiling stupid ball, you don't belong there
        }

        // Hit score-zoooone
        if (transform.position.x < -Constants.FIELD_WIDTH_2) {
            if (Random.value < CharacterAbilityManager.autoShieldChance[1]) {
                direction.x *= -1;
                print("AUTO SHIIIEEEEELLLLLD");
            }
            else {
                scoreManager.GetPoint(2);  // get in the zooone
                PointScored();
            }
        }
        else if (transform.position.x > Constants.FIELD_WIDTH_2) {
            if (Random.value < CharacterAbilityManager.autoShieldChance[2]) {
                direction.x *= -1;
                print("AUTO SHIIIEEEEELLLLLD");
            }
            else {
                scoreManager.GetPoint(1);  // scoooore-o zooooone
                PointScored();
            }
        }

        // Get back in the field plz!
        var ballPos = transform.position;
        if (transform.position.y < -Constants.FIELD_HEIGHT_2) {
            ballPos.y = -Constants.FIELD_HEIGHT_2;
            transform.position = ballPos;
        }

        if (transform.position.y > Constants.FIELD_HEIGHT_2) {
            ballPos.y = Constants.FIELD_HEIGHT_2;
            transform.position = ballPos;
        }
    }

    public void Update() {
        iciness = 0; // Reset in case a paddle ended its iciness
        ghostliness = 0; // Same for ghostliness
    }

	private void PointScored() {
		perRoundSpeedup = 1f;
		perGameSpeedup *= 1.03f;

		flamin = false;

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

		if (flamin) {
            SetColor(Color.red);
		}
		else {
            SetColor(Color.white); // Baseline is white, if not flamin'
        }

        ApplyIciness();
        ApplyGhostliness();
    } 

    // Use the value of 'iciness' to change the ball's color
    private void ApplyIciness() {
        var currColor = visibleSprite.GetComponent<SpriteRenderer>().color;
        var iceColor = Color.Lerp(currColor, Color.blue, iciness * 0.8f);

        SetColor(iceColor);
    }

    // Use the value of 'ghostliness' to change the ball's color
    private void ApplyGhostliness() {
        var currColor = visibleSprite.GetComponent<SpriteRenderer>().color;
        var ghostColor = Color.Lerp(currColor, PowerupInfo.GHOST_COLOR, ghostliness);

        SetColor(ghostColor);
    }

    // Sets the color, also correcting the particle and shadow color.
    public void SetColor(Color newColor) {
        visibleSprite.GetComponent<SpriteRenderer>().color = newColor;
        buttParticles.startColor = newColor;

        var shadowRenderer = GetComponent<JuicyShadow>().shadow.GetComponent<SpriteRenderer>();
        var shadowColor = shadowRenderer.color;
        shadowColor.a = Mathf.Min(newColor.a, JuicyShadow.normalColor.a);
        shadowRenderer.color = shadowColor;
    }

    public void OnCollisionEnter2D(Collision2D collision) {
		if (!GameManager.ShouldUpdate()) {
			return;
		}

		// HIT CEILING
		if (collision.gameObject.tag == "Ceiling" && bounceCooldown <= 0) {
            bounceCooldown = BASE_BOUNCE_COOLDOWN;

			foreach (ContactPoint2D contact in collision.contacts) {
				var particles = Instantiate (impactParticles, contact.point, Quaternion.identity) as GameObject;
				var angle = Mathf.Atan2(contact.normal.x, contact.normal.y);
				particles.transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.forward);
				particles.GetComponent<ParticleSystem>().startColor = visibleSprite.GetComponent<SpriteRenderer>().color;
			}
			animator.SetTrigger("BallHitCeiling");
			direction.y *= -1;

            if (flamin && CharacterAbilityManager.wildFireballs[lastHitPlayer.playerNum]) {
                direction.x += Random.Range(-0.5f, 0.5f);
            }
		}
		
		// HIT PADDLE
		if (collision.gameObject.tag == "Player") {
			
			if (flamin) { flamin = false; }
			
			var paddle = collision.gameObject.GetComponent<PaddleObject>();

            perRoundSpeedup *= 1.05f * CharacterAbilityManager.ballAccellMod[paddle.playerNum];

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

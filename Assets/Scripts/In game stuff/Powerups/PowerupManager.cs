using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupManager : MonoBehaviour {

	public PowerupUI player1ui;
	public PowerupUI player2ui;
	public BallScript ballHandle; // Ball fondlers

	public PaddleObject player1; // Paddle fondlers
	public PaddleObject player2;
	PaddleObject[] players = new PaddleObject[3];

	/** How much powerup progress does a paddle hit give? */
	float hitIncrement = 20f;

	List<Powerup> activePowerups = new List<Powerup>();

	void Start() {
		players[1] = player1;
		players[2] = player2;

//		AddPowerup(2, PowerupType.Shield);
	}

	// Ball hit a paddle.  Increment powerup progress and activate
	//  the correct powerup effects.
	public void PlayerPaddleHit(int which) {
		if (which == 1) {
			player1ui.AddPowerupProgress(hitIncrement);
		}
		else if (which == 2) {
			player2ui.AddPowerupProgress(hitIncrement);
		}
		
		foreach (var powerup in activePowerups) {
			if (which == powerup.whichPlayer) {
				DoFriendlyHit(powerup.type);
			}
			else {
				DoEnemyHit(powerup.type);
			}
		}
	}

	public void AddPowerup(int player, PowerupType powerup) {
		activePowerups.Add(new Powerup(powerup, player));
		StartPowerup(powerup, players[player]);
	}

	void Update() {
		for (int i = activePowerups.Count - 1; i >= 0; i--) {
			var powerup = activePowerups[i];
			powerup.timeLeft -= Time.deltaTime;

			if (powerup.timeLeft <= 0) {
				EndPowerup(powerup.type, players[powerup.whichPlayer]);
				activePowerups.RemoveAt(i);
			}
		}
	}

	// Called when a powerup was just added to a player.
	void StartPowerup(PowerupType powerup, PaddleObject player) {
		switch (powerup) {
		case PowerupType.Fireball:  // ACTIVATE FIREBALL
            if (ballHandle.lastHitPlayer == player) {
                ballHandle.flamin = true;
            }
			break;
		case PowerupType.Magnet:
			player.magnetized = true;
			break;
		case PowerupType.Ghost:
			player.ghostly = true;
			break;
		case PowerupType.Shield:
			SpawnShield(player);
			break;
		case PowerupType.Portals:
			SpawnPortals(player);
			break;
		}
	}

	// Called when a powerup is over and done
	void EndPowerup(PowerupType powerup, PaddleObject player) {
		switch(powerup) {
		case PowerupType.Magnet:
			player.magnetized = false;
			break;
		case PowerupType.Ghost:
			player.ghostly = false;
			ballHandle.ghost = false;
			break;
		}
	}

	// Called when a player with a powerup just hit the ball
	void DoFriendlyHit(PowerupType powerup) {
		switch (powerup) {
		case PowerupType.Fireball:  // ACTIVATE FIREBALL
			ballHandle.flamin = true;
			break;
		}
	}

	// Called when a player with a powerup's ENEMY hit the ball
	void DoEnemyHit(PowerupType powerup) {
		switch(powerup) {
		case PowerupType.Fireball: // nothin
			break;
		case PowerupType.Iceball: // ACTIVATE ICE
			ballHandle.icy = true;
			break;
		}
	}

	void SpawnShield(PaddleObject player) {
		player.AddShield();
	}
	
	void SpawnPortals(PaddleObject player) {
		GameObject portal1 = GameObject.Instantiate(Resources.Load ("BluePortal")) as GameObject;
		GameObject portal2 = GameObject.Instantiate(Resources.Load ("OrangePortal")) as GameObject;

        portal1.transform.parent = ((ScreenShaker)FindObjectOfType(typeof(ScreenShaker))).transform;
        portal2.transform.parent = ((ScreenShaker)FindObjectOfType(typeof(ScreenShaker))).transform;

        portal1.GetComponent<Portal>().matchingPortal = portal2.gameObject;
		portal2.GetComponent<Portal>().matchingPortal = portal1.gameObject;

		var randY1 = Random.Range (-Constants.FIELD_HEIGHT_2 * 0.8f, Constants.FIELD_HEIGHT_2 * 0.8f);
		var randY2 = Random.Range (-Constants.FIELD_HEIGHT_2 * 0.8f, Constants.FIELD_HEIGHT_2 * 0.8f);

		var randX1 = Random.Range(Constants.FIELD_WIDTH * 0.35f, Constants.FIELD_WIDTH * 0.6f) - Constants.FIELD_WIDTH_2;
		var randX2 = Random.Range (Constants.FIELD_WIDTH * 0.1f, Constants.FIELD_WIDTH * 0.3f);

		var playerSide = -Mathf.Sign (player.transform.position.x);

		portal1.transform.position = new Vector3(randX1 * playerSide, randY1, 0);
		portal2.transform.position = new Vector3(randX1 + randX2 * playerSide, randY2, 0);
	}
}

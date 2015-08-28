using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupManager : MonoBehaviour {

	public PowerupUI player1ui;
	public PowerupUI player2ui;
	public BallScript ballHandle; // Ball fondlers

	public PaddleScript player1; // Paddle fondlers
	public PaddleScript player2;
	PaddleScript[] players = new PaddleScript[3];

	/** How much powerup progress does a paddle hit give? */
	float hitIncrement = 20f;

	List<Powerup> activePowerups = new List<Powerup>();

	void Start() {
		players[1] = player1;
		players[2] = player2;

		AddPowerup(1, PowerupType.Magnet);
//		AddPowerup(2, PowerupType.Magnet);
	}

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
	void StartPowerup(PowerupType powerup, PaddleScript player) {
		switch (powerup) {
		case PowerupType.Fireball:  // ACTIVATE FIREBALL
			ballHandle.flamin = true;
			break;
		case PowerupType.Magnet:
			player.magnetized = true;
			break;
		}
	}

	// Called when a powerup is over and done
	void EndPowerup(PowerupType powerup, PaddleScript player) {
		switch(powerup) {
		case PowerupType.Magnet:
			player.magnetized = false;
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
}

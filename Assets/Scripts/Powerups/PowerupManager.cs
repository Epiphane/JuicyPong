using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupManager : MonoBehaviour {

	public PowerupUI player1ui;
	public PowerupUI player2ui;
	public BallScript ballHandle; // Ball fondlers

	/** How much powerup progress does a paddle hit give? */
	float hitIncrement = 100f;

	List<Powerup> activePowerups = new List<Powerup>();

	public void PlayerPaddleHit(int which) {
		if (which == 1) {
			player1ui.AddPowerupProgress(hitIncrement);
		}
		else if (which == 2) {
			player2ui.AddPowerupProgress(hitIncrement);
		}
		
		foreach (var powerup in activePowerups) {
			if (which == powerup.whichPlayer) {
				print(powerup.type);
				DoFriendlyHit(powerup.type);
			}
			else {
				DoEnemyHit(powerup.type);
			}
		}
	}

	public void AddPowerup(int player, PowerupType powerup) {
		activePowerups.Add(new Powerup(powerup, player));
		StartPowerup(powerup);
	}

	void Update() {
		for (int i = activePowerups.Count - 1; i >= 0; i--) {
			var powerup = activePowerups[i];
			powerup.timeLeft -= Time.deltaTime;

			if (powerup.timeLeft <= 0) {
				activePowerups.RemoveAt(i);
			}
		}
	}

	// Called when a powerup was just added to a player.
	void StartPowerup(PowerupType powerup) {
		switch (powerup) {
		case PowerupType.Fireball:  // ACTIVATE FIREBALL
			ballHandle.flamin = true;
			break;
		case PowerupType.Iceball:  // nothin
			break;
		}
	}

	// Called when a player with a powerup just hit the ball
	void DoFriendlyHit(PowerupType powerup) {
		switch (powerup) {
		case PowerupType.Fireball:  // ACTIVATE FIREBALL
			ballHandle.flamin = true;
			break;
		case PowerupType.Iceball:  // nothin
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

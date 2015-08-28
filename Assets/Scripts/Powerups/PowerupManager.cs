using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupManager : MonoBehaviour {

	public PowerupUI player1ui;
	public PowerupUI player2ui;

	/** How much powerup progress does a paddle hit give? */
	float hitIncrement = 20f;

	List<Powerup> activePowerups = new List<Powerup>();

	public void PlayerPaddleHit(int which) {
		if (which == 1) {
			player1ui.AddPowerupProgress(hitIncrement);
		}
		else if (which == 2) {
			player2ui.AddPowerupProgress(hitIncrement);
		}
		
		foreach (var powerup in activePowerups) {
			if (player == powerup.whichPlayer) {
				DoFriendlyHit(powerup);
			}
			else {
				DoEnemyHit(powerup);
			}
		}
	}

	public void AddPowerup(int player, PowerupType powerup) {
		activePowerups.Add(new Powerup(powerup, player));
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



	void DoFriendlyHit(PowerupType powerup) {
		switch (powerup) {
		case PowerupType.Fireball:
			// Speed da ball up
			break;
		case PowerupType.Iceball:
			// Turn ball to normal color
			break;
		}
	}

	void DoEnemyHit(PowerupType powerup) {
		switch(powerup) {
		case PowerupType.Fireball:
			// Turn ball to normal color
			break;
		case PowerupType.Iceball:
			// Slow da ball down
			break;
		}
	}
}

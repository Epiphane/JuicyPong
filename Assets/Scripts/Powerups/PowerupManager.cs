using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupManager : MonoBehaviour {

	public PowerupUI player1ui;
	public PowerupUI player2ui;
	public BallScript ballHandle; // Ball fondlers

	/** How much powerup progress does a paddle hit give? */
	float hitIncrement = 20f;

	List<Powerup> activePowerups = new List<Powerup>();

	void Start() {
		AddPowerup(1, PowerupType.Fireball);
		AddPowerup(1, PowerupType.Iceball);
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
	}

	void Update() {
		for (int i = activePowerups.Count - 1; i >= 0; i--) {
			var powerup = activePowerups[i];
//			powerup.timeLeft -= Time.deltaTime;

			if (powerup.timeLeft <= 0) {
				activePowerups.RemoveAt(i);
			}
		}
	}



	void DoFriendlyHit(PowerupType powerup) {
		switch (powerup) {
		case PowerupType.Fireball:  // ACTIVATE FIREBALL
			ballHandle.flamin = true;
			break;
		case PowerupType.Iceball:  // DEACTIVATE ICE
			ballHandle.icy = false;
			break;
		}
	}

	void DoEnemyHit(PowerupType powerup) {
		switch(powerup) {
		case PowerupType.Fireball: // DEACTIVATE FIREBALL
			ballHandle.flamin = false;
			break;
		case PowerupType.Iceball: // ACTIVATE ICE
			ballHandle.icy = true;
			break;
		}
	}
}

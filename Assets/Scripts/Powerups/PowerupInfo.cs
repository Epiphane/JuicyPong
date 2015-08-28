using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PowerupType {
	Fireball,
	Iceball,
}

// PowerupInfo only has static stuff, and handles the names / icons of all the powerups.
public class PowerupInfo {

	public static PowerupType[] Choose2RandomPowerups() {
		// Look at how random this is
		return new PowerupType[] {PowerupType.Fireball, PowerupType.Iceball};
	}

	public string PowerupName(PowerupType type) {
		switch (type) {
		case PowerupType.Fireball:
			return "Fireball";
		case PowerupType.Iceball:
			return "Iceball";
		}
		return "";
	}

	public static float FIREBALL_SPEED_MULT = 2f;
	public static float ICEBALL_SPEED_MULT = 0.5f;
}

public class Powerup {
	public PowerupType type;
	public int whichPlayer;
	public float timeLeft = 7;

	public Powerup(PowerupType type, int player) {
		this.type = type;
		this.whichPlayer = player;
	}
}
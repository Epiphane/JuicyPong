using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum PowerupType {
	Fireball,
	Iceball,
	Magnet,
	Shield,
	Portals,
	Ghost,
}

// PowerupInfo only has static stuff, and handles the names / icons of all the powerups.
public class PowerupInfo {

	public static PowerupType[] Choose2RandomPowerups() {
		var powerups = System.Enum.GetValues(typeof(PowerupType));
		var firstNdx = UnityEngine.Random.Range(0, powerups.Length);
		var secondNdx = firstNdx + UnityEngine.Random.Range (0, powerups.Length - 2);
		secondNdx %= powerups.Length;

		return new PowerupType[] {(PowerupType) powerups.GetValue(firstNdx), (PowerupType) powerups.GetValue(secondNdx)};
	}

	// Human readable string of this powerup
	public static string Name(PowerupType type) {
		switch (type) {
		case PowerupType.Fireball:
			return "Fireball";
		case PowerupType.Iceball:
			return "Iceball";
		case PowerupType.Magnet:
			return "Magnet";
		case PowerupType.Shield:
			return "Shield";
		case PowerupType.Portals:
			return "Portals";
		case PowerupType.Ghost:
			return "Ghost";
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
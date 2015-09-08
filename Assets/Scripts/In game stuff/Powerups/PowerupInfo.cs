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

	public static float PowerupChance(PowerupType type, int playerNum) {
		switch (type) {
		case PowerupType.Fireball:
			return 1f;
		case PowerupType.Iceball:
			return 1f;
		case PowerupType.Magnet:
			return 1f * CharacterAbilityManager.moreMagnets[playerNum];
		case PowerupType.Shield:
			return 1f;
		case PowerupType.Portals:
			return 1f;
		case PowerupType.Ghost:
			return 1f;
		}

		return 1f;
	}

	public static PowerupType[] Choose2RandomPowerups(int playerNum) {
		var powerups = System.Enum.GetValues(typeof(PowerupType));

		var weightedSum = 0f;
		foreach (PowerupType powerup in powerups) {
			weightedSum += PowerupChance(powerup, playerNum);
		}

		var random = UnityEngine.Random.value * weightedSum;
		var sumSoFar = 0f;
		PowerupType firstType = PowerupType.Fireball;
		PowerupType secondType = PowerupType.Fireball;

		foreach (PowerupType powerup in powerups) {
			sumSoFar += PowerupChance(powerup, playerNum);
			if (sumSoFar > random) {
				firstType = powerup;
				break;
			}
		}

		do {
			random = UnityEngine.Random.value * weightedSum;
			sumSoFar = 0f;
			foreach (PowerupType powerup in powerups) {
				sumSoFar += PowerupChance(powerup, playerNum);
				if (sumSoFar > random) {
					secondType = powerup;
					break;
				}
			}
		} while (firstType == secondType);

		return new PowerupType[] {firstType, secondType};
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
	public float timeLeft;

	public Powerup(PowerupType type, int player) {
		this.type = type;
		this.whichPlayer = player;
		timeLeft = 7f * CharacterAbilityManager.powerupLengthMod[player];
	}
}
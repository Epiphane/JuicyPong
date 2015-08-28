using UnityEngine;
using System.Collections;

public class PowerupManager : MonoBehaviour {

	public PowerupUI player1ui;
	public PowerupUI player2ui;

	// How much powerup progress does a paddle hit give?
	float hitIncrement = 20f;

	void PlayerPaddleHit(int which) {
		if (which == 1) {
			player1ui.powerupProgress += hitIncrement;
		}
		else if (which == 2) {
			player2ui.powerupProgress += hitIncrement;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

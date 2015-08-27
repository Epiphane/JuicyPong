﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Powerup UI handles all the UI for choosing and displaying powerups.
///   It has a progress bar at the bottom and handles the 'choice' buttons when they pop up.
///   Each player needs to have their own lil' version of the Powerup UI, one on 
///   each side of the screen.
/// </summary>

public class PowerupUI : MonoBehaviour {

	public float powerupProgress = 0f;
	public Canvas myCanvas;

	// Lets us change the width of the Green 'progress' indicator
	RectTransform innerBarTransform;

	void Awake() {
		innerBarTransform = transform.Find("PowerupBar/Progress").GetComponent<RectTransform>();
	}
	
	// Update is called once per frame. YA DON'T SAY!!?
	void Update () {
		var totalWidth = Screen.width / 2;
		var progressFraction = Mathf.Min(1, powerupProgress / 100);
		var barWidth = totalWidth * progressFraction;
		var insetWidth = totalWidth - barWidth;

		innerBarTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, insetWidth, barWidth);
	}

	/// <summary>
	/// Adds to the current powerup progress.
	/// </summary>
	/// <param name="amount">How much? (you get a powerup at 100)</param>
	public void AddPowerupProgress(int amount) {

	}
}
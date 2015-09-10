using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using System;

public class ScreenGestureHandler : MonoBehaviour {
	public PaddleObject player;
	public PowerupUI powerupUI;
	
	/**** MOBILE, PAN GESTURE STUFF ***/
	
	// Register for pan gesture events
	private void OnEnable() {
		GetComponent<PanGesture>().PanStarted += PanBegan;
		GetComponent<PanGesture>().Panned += PanMoved;

		GetComponent<TapGesture>().Tapped += Tapped;
	}
	
	private void OnDisable() {
		GetComponent<PanGesture>().PanStarted -= PanBegan;
		GetComponent<PanGesture>().Panned -= PanMoved;
		
		GetComponent<TapGesture>().Tapped -= Tapped;
	}
	
	void PanBegan(object sender, EventArgs e) { // idk I might need this eventually
	}
	
	public void PanMoved(object sender, EventArgs e) {
		var panGesture = sender as PanGesture;
		var worldPoint = Camera.main.ScreenToWorldPoint(panGesture.ScreenPosition);
		
		player.transform.position = new Vector3(player.PADDLE_X, worldPoint.y, player.PADDLE_Z);
	}

	public void Tapped(object sender, EventArgs e) {
		var tapGesture = sender as TapGesture;
		var worldPoint = Camera.main.ScreenToWorldPoint(tapGesture.ScreenPosition);

		if (worldPoint.y > 0) {
			powerupUI.ChosePowerup1();
		}
		else {
			powerupUI.ChosePowerup2();
		}
	}
	/*** END MOBILE PAN GESTURE STUFF ***/
}

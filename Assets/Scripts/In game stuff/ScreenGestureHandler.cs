using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using System;

public class ScreenGestureHandler : MonoBehaviour {
	public PaddleObject player;
	public PowerupUI powerupUI;
    public float side; // Negative or positive to get me on the correct side of the screen

    // Set sprite to cover half the screen
    void Start() {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        float worldScreenHeight = Camera.main.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        transform.localScale = new Vector3(
            worldScreenWidth / sr.sprite.bounds.size.x / 2,
            worldScreenHeight / sr.sprite.bounds.size.y, 1);

        transform.position = new Vector3(worldScreenWidth / 4 * side, 0, 0);
    }
	
	/**** MOBILE, PAN GESTURE STUFF ***/
	
	// Register for pan gesture events
	private void OnEnable() {
		GetComponent<PanGesture>().PanStarted += PanBegan;
		GetComponent<PanGesture>().Panned += PanMoved;
		GetComponent<PanGesture>().PanCompleted += PanEnded;

        GetComponent<TapGesture>().Tapped += Tapped;
	}
	
	private void OnDisable() {
		GetComponent<PanGesture>().PanStarted -= PanBegan;
		GetComponent<PanGesture>().Panned -= PanMoved;
		GetComponent<PanGesture>().PanCompleted -= PanEnded;

        GetComponent<TapGesture>().Tapped -= Tapped;
	}
	
    // Finger down!  Tell the animation system to raise the paddle
	void PanBegan(object sender, EventArgs e) {
        player.GetComponent<Animator>().SetBool("FingerDown", true);
	}
	
    // Pan moved.  Tell the paddle to go to that place.
	public void PanMoved(object sender, EventArgs e) {
		var panGesture = sender as PanGesture;
		var worldPoint = Camera.main.ScreenToWorldPoint(panGesture.ScreenPosition);

        player.targetY = worldPoint.y;
	}

    // Pan ended.  Set the paddle down.
    void PanEnded(object sender, EventArgs e) {
        player.GetComponent<Animator>().SetBool("FingerDown", false);
    }

    public void Tapped(object sender, EventArgs e) {
		var tapGesture = sender as TapGesture;
		var worldPoint = Camera.main.ScreenToWorldPoint(tapGesture.ScreenPosition);

        if (powerupUI)
		if (worldPoint.y > 0) {
			powerupUI.ChosePowerup1();
		}
		else {
			powerupUI.ChosePowerup2();
		}
	}
	/*** END MOBILE PAN GESTURE STUFF ***/
}

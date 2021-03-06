﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;

public class PaddleObject : MonoBehaviour {

	public float PADDLE_X = 0;
	public float PADDLE_Z = -10;

	public int speed = 10;
	public int playerNum = 1;
	public float dy = 0;
	public float powerupProgress = 0f;
	public PowerupManager powerupManager;
	public PowerupUI powerupUI;
	public GameManager gameManager;

	public bool magnetized = false;
	public bool ghostly = false;
    public bool icy = false;

    public Vector3 baseScale = Vector3.one;
    public float scaleModifier = 1f; // Used to animate the scale relative to the paddle's normal scale
	
	public List<ShieldObject> activeShields = new List<ShieldObject>();

    public float targetY = 0; // The last point the user dragged to.  We'll smoothly glide there.
    
    bool BallIsComingAtMe() {
        return Mathf.Sign(gameManager.ballScript.direction.x) == Mathf.Sign(PADDLE_X);
    }

    void LateUpdate() {
        transform.localScale = baseScale * scaleModifier;

        var diffX = Mathf.Abs(transform.position.x - gameManager.ballScript.transform.position.x);
        var maxDiff = Constants.FIELD_WIDTH * 0.8f;

        var differenceFactor = diffX / maxDiff;
        differenceFactor = Mathf.Clamp(differenceFactor, 0, 1);

        if (icy && BallIsComingAtMe()) {
            gameManager.ballScript.iciness = Mathf.Max(gameManager.ballScript.iciness, 1 - differenceFactor);
        }

        if (ghostly && !BallIsComingAtMe()) {
            gameManager.ballScript.ghostliness = Mathf.Max(gameManager.ballScript.ghostliness, differenceFactor + 0.13f);
        }
    }

	void FixedUpdate () {
        //if (Application.isMobilePlatform) {
            var diff = targetY - transform.position.y;
            transform.Translate(new Vector3(0, diff / 4, 0));
        //}

        if (!GameManager.ShouldUpdate()) {
			return;
		}

		dy = Input.GetAxisRaw("P" + playerNum + " Vertical") * speed;
		transform.position += new Vector3(0, dy * Time.deltaTime, 0);

		if (transform.position.y < -Constants.FIELD_HEIGHT_2) {
			var pos = transform.position;
			pos.y = -Constants.FIELD_HEIGHT_2;
			transform.position = pos;
		}
		else if (transform.position.y > Constants.FIELD_HEIGHT_2) {
			var pos = transform.position;
			pos.y = Constants.FIELD_HEIGHT_2;
			transform.position = pos;
		}

		if (magnetized) {
            var diffY = transform.position.y - gameManager.ballScript.transform.position.y;
            var diffX = Mathf.Abs(transform.position.x - gameManager.ballScript.transform.position.x) * 0.8f;
            var influence = Mathf.Pow((1 - diffX / Constants.FIELD_WIDTH), 4f) * 0.1f;

            gameManager.ballScript.transform.Translate(new Vector3(0f, diffY * influence, 0f));
        }
	}



	public void AddShield() {
		var newShieldObject = GameObject.Instantiate(Resources.Load ("Shield")) as GameObject;
		var newShield = newShieldObject.GetComponent<ShieldObject>();
		activeShields.Add(newShield);

		newShield.baseX = Constants.FIELD_WIDTH_2 * 0.95f * Mathf.Sign(transform.position.x);
		newShield.owner = this;

		newShield.UpdatePosition();
	}

	public void TriggerShieldUpdate() {
		foreach (var shield in activeShields) {
			shield.UpdatePosition();
		}
	}

	// Collided with ball!  Bounce myself and alert the powerup manager.
	public void HitBall() {
		GetComponent<Animator>().SetTrigger("PaddleHit");
		powerupManager.PlayerPaddleHit(playerNum);
	}
}

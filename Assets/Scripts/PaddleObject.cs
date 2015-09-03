using UnityEngine;
using System.Collections;

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

	/**** MOBILE, PAN GESTURE STUFF ***/

	private int currentTouchID = -1; // Each touch corresponds to a unique ID.  Each paddle only cares about 1 touch at a time, defined by this variable.

	// Register for pan gesture events
	private void OnEnable() {
		MobileInputHandler.OnPanBegan += PanBegan;
		MobileInputHandler.OnPanHeld += PanMoved;
		MobileInputHandler.OnPanEnded+= PanEnded;
	}

	private void OnDisable() {
		MobileInputHandler.OnPanBegan -= PanBegan;
		MobileInputHandler.OnPanHeld -= PanMoved;
		MobileInputHandler.OnPanEnded -= PanEnded;
	}

	void PanBegan(Touch t) {
		// Check if the gesture was on our side
		var worldPoint = Camera.main.ScreenToWorldPoint(t.position);
		if (Mathf.Sign (worldPoint.x) == Mathf.Sign (transform.position.x) && currentTouchID == -1) {
			currentTouchID = t.fingerId;
		}
	}

	public void PanMoved(Touch t) {
		if (t.fingerId == currentTouchID) {
			var worldPoint = Camera.main.ScreenToWorldPoint(t.position);
			transform.position = new Vector3(PADDLE_X, worldPoint.y, PADDLE_Z);
		}
	}

	public void PanEnded(Touch t) {
		if (t.fingerId == currentTouchID) {
			currentTouchID = -1;
		}
	}
	/*** END MOBILE PAN GESTURE STUFF ***/
	
	void FixedUpdate () {
		if (!gameManager.ShouldUpdate()) {
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
			var influence = Mathf.Pow ((1 - diffX / Constants.FIELD_WIDTH), 4f) * 0.1f;

			gameManager.ballScript.transform.Translate(new Vector3(0f, diffY * influence, 0f));
		}

		if (ghostly) {
			if (Mathf.Sign (gameManager.ballScript.transform.position.x) != Mathf.Sign (transform.position.x)) {
				gameManager.ballScript.ghost = true;
			}
			else {
				gameManager.ballScript.ghost = false;
			}
		}
	}

	// Collided with ball!  Bounce myself and alert the powerup manager.
	public void HitBall() {
		GetComponent<Animator>().SetTrigger("PaddleHit");
		powerupManager.PlayerPaddleHit(playerNum);
	}
}

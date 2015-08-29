using UnityEngine;
using System.Collections;

public class PaddleScript : MonoBehaviour {

	public int speed = 10;
	public int player = 1;
	public float dy = 0;
	public PowerupManager powerupManager;
	public GameManager gameManager;

	public bool magnetized = false;
	public bool ghostly = false;
	
	void FixedUpdate () {
		if (!gameManager.ShouldUpdate()) {
			return;
		}

		dy = Input.GetAxisRaw("P" + player + " Vertical") * speed;
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

			gameManager.ballScript.transform.Translate(new Vector3(0, diffY * influence, 0));
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
		powerupManager.PlayerPaddleHit(player);
	}
}

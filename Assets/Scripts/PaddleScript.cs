using UnityEngine;
using System.Collections;

public class PaddleScript : MonoBehaviour {

	public int speed = 10;
	public int player = 1;
	public float dy = 0;
	public PowerupManager powerups;
	public GameManager manager;
	
	void FixedUpdate () {
		if (!manager.ShouldUpdate()) {
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
	}

	// Collided with ball!  Bounce myself and alert the powerup manager.
	public void HitBall() {
		GetComponent<Animator>().SetTrigger("PaddleHit");
		powerups.PlayerPaddleHit(player);
	}
}

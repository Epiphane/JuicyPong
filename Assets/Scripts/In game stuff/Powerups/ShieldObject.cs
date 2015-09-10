using UnityEngine;
using System.Collections;

public class ShieldObject : MonoBehaviour {
	public float baseX = 0;

	public float shieldMargin = 1f;

	public PaddleObject owner;

	/**
	 * Positions this shield to the correct X value
	 *  based on the baseX and shieldNdx
	 */
	public void UpdatePosition() {
		var numShields = owner.activeShields.Count;
		var shieldNdx = owner.activeShields.IndexOf(this);

		var shieldWidth = GetComponent<BoxCollider2D>().size.x;
		var totalWidth = numShields * shieldWidth + numShields * shieldWidth * shieldMargin;

		var newX = baseX - totalWidth/2 + shieldNdx/numShields * totalWidth;

		transform.position = new Vector3(newX, transform.position.y, transform.position.z);
	}

	public void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Ball") {
			owner.activeShields.Remove(this);
			owner.TriggerShieldUpdate();
			Destroy(gameObject);
		}
	}
}

using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

	public GameObject matchingPortal;
	float cooldown = 0;

	public float lifeLeft = 10;

	void Update() {
		cooldown -= Time.deltaTime;
		lifeLeft -= Time.deltaTime;

		if (lifeLeft <= 0) {
			Destroy(gameObject);
		}
	}
	
	public void OnTriggerEnter2D (Collider2D other) {

		var ball = other.gameObject.GetComponent<BallScript>();
		if (ball != null && cooldown <= 0) {
			var diff = matchingPortal.transform.position - transform.position;
			ball.transform.Translate(diff);

			cooldown = 0.8f;
			matchingPortal.GetComponent<Portal>().cooldown = 0.8f;

            GetComponent<Animator>().SetTrigger("PortalBounce");
        }
	}
}

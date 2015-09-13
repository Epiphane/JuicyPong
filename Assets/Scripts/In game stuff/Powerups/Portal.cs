using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

	public GameObject matchingPortal;
	float cooldown = 0;
    const float PORTAL_COOLDOWN = 0.5f;

	public float lifeLeft = 10;

	void Update() {
		cooldown -= Time.deltaTime;
        
        if (GameManager.ShouldUpdate()) {
            lifeLeft -= Time.deltaTime;
        }

        if (lifeLeft <= 0) {
			Destroy(gameObject);
		}
	}
	
	public void OnTriggerEnter2D (Collider2D other) {

		var ball = other.gameObject.GetComponent<BallScript>();
		if (ball != null && cooldown <= 0) {
			var diff = matchingPortal.transform.position - transform.position;
			ball.transform.Translate(diff);

            cooldown = PORTAL_COOLDOWN;
            matchingPortal.GetComponent<Portal>().cooldown = PORTAL_COOLDOWN;

            GetComponent<Animator>().SetTrigger("PortalBounce");
            matchingPortal.GetComponent<Animator>().SetTrigger("PortalBounceExit");

            var particleObject = Instantiate(Resources.Load("Portal Particles")) as GameObject;
            var particles = particleObject.GetComponent<ParticleSystem>();

            particleObject.transform.position = ball.transform;
        }
    }
}

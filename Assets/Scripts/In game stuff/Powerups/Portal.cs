using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

	public GameObject matchingPortal;
	float cooldown = 0;
    const float PORTAL_COOLDOWN = 0.5f;

	public float lifeLeft = 10;
    public Color particleColor;

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

            var particleObject = Instantiate(Resources.Load("PortalImpactParticles")) as GameObject;
            var particles = particleObject.GetComponent<ParticleSystem>();
            var angle = Mathf.Atan2(ball.direction.y, ball.direction.x);
            particleObject.transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * -angle, Vector3.forward);

            var newPos = matchingPortal.transform.position + new Vector3(0, 0, 0.2f);
            particleObject.transform.position = newPos;

            particles.startColor = matchingPortal.GetComponent<Portal>().particleColor;
        }
    }
}

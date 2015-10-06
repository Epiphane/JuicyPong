using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

	public GameObject matchingPortal;
	float cooldown = 0;
    const float PORTAL_COOLDOWN = 0.5f;

	public int warpsLeft = 4;
    public Color particleColor;

	void Update() {
		cooldown -= Time.deltaTime;
	}
	
	public void OnTriggerEnter2D (Collider2D other) {

		var ball = other.gameObject.GetComponent<BallScript>();
		if (ball != null && cooldown <= 0) {
            var otherPortal = matchingPortal.GetComponent<Portal>();

			var diff = matchingPortal.transform.position - transform.position;
			ball.transform.Translate(diff);

            cooldown = PORTAL_COOLDOWN;
            otherPortal.cooldown = PORTAL_COOLDOWN;

            GetComponent<Animator>().SetTrigger("PortalBounce");
            matchingPortal.GetComponent<Animator>().SetTrigger("PortalBounceExit");

            var particleObject = Instantiate(Resources.Load("PortalImpactParticles")) as GameObject;
            var particles = particleObject.GetComponent<ParticleSystem>();
            var angle = Mathf.Atan2(ball.direction.y, ball.direction.x);
            particleObject.transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * -angle, Vector3.forward);

            var newPos = matchingPortal.transform.position + new Vector3(0, 0, 0.2f);
            particleObject.transform.position = newPos;

            particles.startColor = otherPortal.particleColor;

            warpsLeft--;
            otherPortal.warpsLeft--;

            if (warpsLeft <= 0) {
                Destroy(gameObject);
                Destroy(matchingPortal);
            }
        }
    }
}

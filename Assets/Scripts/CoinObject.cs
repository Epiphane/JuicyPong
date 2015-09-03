using UnityEngine;
using System.Collections;

public class CoinObject : MonoBehaviour {

	public ParticleSystem coinParticles;

	public void OnTriggerEnter2D (Collider2D other) {
		var particles = Instantiate (coinParticles, transform.position, Quaternion.identity) as GameObject;

		Destroy(gameObject);
	}

}

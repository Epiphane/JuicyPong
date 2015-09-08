using UnityEngine;
using System.Collections;

public class CoinObject : MonoBehaviour {

	public ParticleSystem coinParticles;

	public void OnTriggerEnter2D (Collider2D other) {
		Instantiate (coinParticles, transform.position, Quaternion.identity);

		Destroy(gameObject);
	}

}

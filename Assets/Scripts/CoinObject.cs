using UnityEngine;
using System.Collections;

public class CoinObject : MonoBehaviour {

	public void OnTriggerEnter2D (Collider2D other) {
		Destroy(gameObject);
	}

}

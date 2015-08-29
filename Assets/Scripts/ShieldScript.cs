using UnityEngine;
using System.Collections;

public class ShieldScript : MonoBehaviour {
	public void OnTriggerEnter2D (Collider2D other) {
		Destroy(gameObject);
	}
}

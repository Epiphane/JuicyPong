using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CoinManager : MonoBehaviour {
	public List<GameObject> currentCoins = new List<GameObject>();

	private float timeTillNextCoin = 0;
	
	// Update is called once per frame
	void Update () {
		// Wipe out destroyed coins
		currentCoins = currentCoins.Where(item => item != null).ToList ();

		if (timeTillNextCoin <= 0 && currentCoins.Count <= 2) {
			timeTillNextCoin = Random.Range(1f, 1.5f);

			var newCoin = GameObject.Instantiate(Resources.Load ("Coin")) as GameObject;

			var newX = Random.Range (-Constants.FIELD_WIDTH_2 * 0.7f, Constants.FIELD_WIDTH_2 * 0.7f);
			var newY = Random.Range (-Constants.FIELD_HEIGHT_2 * 0.8f, Constants.FIELD_HEIGHT_2 * 0.8f);
			newCoin.transform.position = new Vector3(newX, newY, -10);

			currentCoins.Add(newCoin);
		}

		timeTillNextCoin -= Time.deltaTime;
	}
}

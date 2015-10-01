using UnityEngine;
using System.Collections;

public class ScreenShaker : MonoBehaviour {

    public static ScreenShaker me;

	// Use this for initialization
	void Awake() {
        me = this;
	}

    private float screenShakiness = 0f;

	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(Random.Range(-screenShakiness, screenShakiness), Random.Range(-screenShakiness, screenShakiness), 
            Random.Range(-screenShakiness, screenShakiness));
        screenShakiness *= 0.9f;
	}

    public void ShakeScreen(float shakeValue) {
        screenShakiness = shakeValue;
    }
}

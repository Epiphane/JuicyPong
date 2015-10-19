using UnityEngine;
using System.Collections;

public class Constants : MonoBehaviour {
	public static float FIELD_WIDTH;
	public static float FIELD_HEIGHT;
	
	public static float FIELD_WIDTH_2;
	public static float FIELD_HEIGHT_2;

	Bounds SCREEN_SIZE;

    public static bool singlePlayer;

	void Awake() {
		GameObject.DontDestroyOnLoad(this);

        singlePlayer = false;

		float screenAspect = (float)Screen.width / (float)Screen.height;
		float cameraHeight = Camera.main.orthographicSize * 2;
		SCREEN_SIZE = new Bounds(
			Camera.main.transform.position,
			new Vector3(cameraHeight * screenAspect, cameraHeight, 0));

		FIELD_WIDTH = SCREEN_SIZE.size.x;
		FIELD_HEIGHT = SCREEN_SIZE.size.y;

		FIELD_WIDTH_2 = FIELD_WIDTH / 2;
		FIELD_HEIGHT_2 = FIELD_HEIGHT / 2;
	}
}

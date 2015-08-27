using UnityEngine;
using System.Collections;

public class Constants : MonoBehaviour {
	public static float FIELD_WIDTH = 12f;
	public static float FIELD_HEIGHT = 5.3f;
	
	public static float FIELD_WIDTH_2 = FIELD_WIDTH / 2;
	public static float FIELD_HEIGHT_2 = FIELD_HEIGHT / 2;

	void Awake() {
		GameObject.DontDestroyOnLoad(this);
	}
}

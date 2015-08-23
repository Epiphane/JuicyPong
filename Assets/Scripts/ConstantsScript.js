#pragma strict

function Awake() {
	GameObject.DontDestroyOnLoad(this);
}

class Constants extends MonoBehaviour {
	public static var FIELD_WIDTH = 12;
	public static var FIELD_HEIGHT = 5.3;
	
	public static var FIELD_WIDTH_2 = FIELD_WIDTH / 2;
	public static var FIELD_HEIGHT_2 = FIELD_HEIGHT / 2;
}
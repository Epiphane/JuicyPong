using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinScreenUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Text>().text = "PLAYER " + ScoreManager.winner + " WINS AHHHHHH";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

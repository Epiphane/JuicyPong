#pragma strict

public var gameManager : GameManager;

public var P1GUI : GUIText;
public var P2GUI : GUIText;
public var scores : int[] = [0, 0];

function Start () {
	
}

function Update () {

}

function GetPoint(player : int) {
	if (player == 1) {
		scores[0] ++;
		P1GUI.text = "" + scores[0];
	}
	if (player == 2) {
		scores[1] ++;
		P2GUI.text = "" + scores[1];
	}
	
	gameManager.Reset();
}
using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {
	
	public GameManager gameManager;
	
	public GUIText P1GUI;
	public GUIText P2GUI;
	public int[] scores = {0, 0};

	public static int winner;

	public void GetPoint(int player) {
		if (player == 1) {
			scores[0] ++;
			P1GUI.text = "" + scores[0];

			if (scores[0] >= 11) {
				winner = 1;
				Application.LoadLevel ("WinScreen");
			}
		}
		if (player == 2) {
			scores[1] ++;
			P2GUI.text = "" + scores[1];

			if (scores[1] >= 11) {
				winner = 2;
				Application.LoadLevel ("WinScreen");
			}
		}
		
		gameManager.Reset();
	}
}

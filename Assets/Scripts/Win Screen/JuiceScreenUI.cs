using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class JuiceScreenUI : MonoBehaviour {

	public float amountJuiceLeft = 1f; // Animated property that makes the +XXX Juice meter go down
	public JuiceLabel earnedJuiceLabel;
	public JuiceLabel myJuiceLabel;

	// Figure out how much juice the player earned and addd iiiiit
	public void CalculateJuice() {
		// If local multiplayer...
		var gainedJuice = JuicePointManager.LocalMultiplayerJuiceReward();
		myJuiceLabel.InitJuiceAmounts(JuicePointManager.totalJuice, JuicePointManager.totalJuice + gainedJuice);
		earnedJuiceLabel.InitJuiceAmounts(gainedJuice, 0);
		
		JuicePointManager.GainJuice(gainedJuice);
	}

	// Play that sweet juice squeezin' animation
	public void SqueezeJuice() {
		earnedJuiceLabel.DoSqueeze();
		myJuiceLabel.DoSqueeze();
	}

	public void NicePressed() {
		Application.LoadLevel ("TitleScreen");
	}
}

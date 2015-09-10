using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterButton : MonoBehaviour {

	public void Awake() {
		GetComponent<Button>().onClick.AddListener(SelectButton);
	}

	public void ResetSelected() {
		GetComponent<Image>().color = Color.white;	
	}

	// User tapped this button.  Turn it red and let the character picker know what just happened
	public void SelectButton() {
		transform.parent.gameObject.BroadcastMessage("ResetSelected");
		GetComponent<Image>().color = Color.red;

		CharacterPicker characterPicker = Object.FindObjectOfType(typeof(CharacterPicker)) as CharacterPicker;

		string canvasName = transform.parent.parent.name ;
		int playerNum = int.Parse(canvasName.Substring(canvasName.Length - 1));

		int characterNum = int.Parse(this.name.Substring(this.name.Length - 1));

		characterPicker.PlayerChoseCharacter(playerNum, characterNum);
	}
}

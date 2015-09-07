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

	public void SelectButton() {
		transform.parent.gameObject.BroadcastMessage("ResetSelected");
		GetComponent<Image>().color = Color.red;

		Object.FindObjectOfType(typeof(CharacterPicker));
	}
}

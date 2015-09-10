using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class JuiceLabel : MonoBehaviour {
	public float animatibleJuice;
	public int startJuice, endJuice;
	public bool plusOrNah;

	public void InitJuiceAmounts(int startJuice, int endJuice) {
		this.startJuice = startJuice;
		this.endJuice = endJuice;
	}

	public void DoSqueeze() {
		GetComponent<Animator>().SetTrigger("SqueezeJuice");
	}

	public void Update() {
		int currJuice = startJuice + (int) ((endJuice - startJuice) * animatibleJuice);
		GetComponent<Text>().text = (plusOrNah ? "+" : "") + currJuice + " JUICE";
	}

}

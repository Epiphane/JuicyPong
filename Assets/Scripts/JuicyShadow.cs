using UnityEngine;
using System.Collections;

public class JuicyShadow : MonoBehaviour {

    private GameObject shadow;
    private Shader guiTextShader;

	// Use this for initialization
	void Start () {
        shadow = Instantiate(Resources.Load("ShadowPrefab")) as GameObject;
        var shadowRenderer = shadow.GetComponent<SpriteRenderer>();

        shadowRenderer.sprite = GetComponent<SpriteRenderer>().sprite;
        shadowRenderer.material.shader = Shader.Find("GUI/Text Shader"); 
        shadow.transform.parent = transform;
        shadow.transform.position = new Vector3(0.1f, 0.1f, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

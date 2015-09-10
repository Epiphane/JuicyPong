using UnityEngine;
using System.Collections;

public class JuicyShadow : MonoBehaviour {

    public float shadowElevation = 0.15f;

    private GameObject shadow;
    private Shader guiTextShader;

	// Use this for initialization
	void Start () {
        shadow = Instantiate(Resources.Load("ShadowPrefab")) as GameObject;
        var shadowRenderer = shadow.GetComponent<SpriteRenderer>();

        shadowRenderer.sprite = GetComponentsInChildren<SpriteRenderer>()[0].sprite;
        shadowRenderer.material.shader = Shader.Find("GUI/Text Shader");
        shadow.transform.parent = transform;
        shadow.transform.localScale = new Vector3(1f, 1f, 1f);

        var shadowXOffset = transform.position.x / Constants.FIELD_WIDTH_2;
        shadow.transform.localPosition = new Vector3(shadowElevation / transform.localScale.x,
                                                    -shadowElevation / transform.localScale.y, 0);
    }

    // Update is called once per frame
    void Update () {
        var shadowXOffset = transform.position.x / Constants.FIELD_WIDTH_2;
        shadow.transform.localPosition = new Vector3(shadowElevation / transform.localScale.x * shadowXOffset,
                                                    -shadowElevation / transform.localScale.y, 0);
    }
}

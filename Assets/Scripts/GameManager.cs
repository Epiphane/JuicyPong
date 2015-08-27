﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public Transform player1;
	public Transform player2;
	public BallScript ballScript;
	
	public GUIText CountdownGUI;
	public float timer = 3;
	private float lastTime;
	
	void Awake() {
		Time.timeScale = 0;
	}
	
	public void Reset() {
		timer = 3;
		Time.timeScale = 0;
		
		lastTime = Time.realtimeSinceStartup;
		
		player1.position = new Vector3(-5, 0, -10);
		player2.position = new Vector3(5, 0, -10);
		
		ballScript.Start();
	}
	
	public void Start () {
		Reset();
	}
	
	void Update () {
		if (timer > -1) {
			timer -= (Time.realtimeSinceStartup - lastTime);
			lastTime = Time.realtimeSinceStartup;
			
			if (timer > 0) {
				CountdownGUI.text = "" + Mathf.Ceil(timer);
			}
			else {
				Time.timeScale = 1;
				CountdownGUI.text = "GO!";
			}
		}
		else {
			CountdownGUI.text = "";
		}
	}
}

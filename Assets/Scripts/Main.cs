﻿using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	
	public int score;
	public GUIText scoreDisplay;
	public GUIText statusMessage;
	public GameObject[] enemies = new GameObject[2];
	// Use this for initialization
	void Start () {
		init ();
	}
	
	void init(){
		Random.seed = (int)System.DateTime.Now.Ticks;
		int f = Random.Range(1,4);
		for(int i=0;i< enemies.Length; i++){
			switch(f){
			case 1:
				createLinearEnemy(i);
				statusMessage.text = "Linear";
				break;
			case 2:
				createQuadraticEnemy(i);
				statusMessage.text = "Quadratic";
				break;
			case 3:
				createExponentialEnemy(i);
				statusMessage.text = "Exponential";
				break;
			case 4:
				createSinusoidEnemy(i);
				statusMessage.text = "Sinusoid";
				break;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		bool create = true;
		for(int i=0;i< enemies.Length; i++){
			if(enemies[i] != null){
				create = false;
			}
		}
		if(create){
			init ();
		}
	}
	
	public int UpdateScore(int i){
		score = score + i;
		scoreDisplay.text = ("Score: " + score);
		return score;
	}	
	
	void KillPlayer(){
		statusMessage.text = ("YOU HAVE DIED. YOU ARE DEAD.");
		return;
	}
	
	void createLinearEnemy(int i){
		Random.seed = (int)System.DateTime.Now.Ticks;
		int sign1 = Random.Range(0,100)>50?1:-1;
		int sign2 = Random.Range(0,100)>50?1:-1;
		int n1 = sign1 * Random.Range(3,10);
		int n2 = sign2 * Random.Range(3,10);
		Vector3 pos = new Vector3(n1, n2, 10);
		enemies[i] = (GameObject)Instantiate(Resources.Load("Enemy"));
		enemies[i].transform.position = pos;
		CurveMotion mEnemy = (CurveMotion)enemies[i].GetComponent(typeof(CurveMotion));
		mEnemy.functionType = "linear";
		mEnemy.moveDirection = -sign1;
		mEnemy.moveSpeed = 3f;
		mEnemy.setEquationFromStartPoint(pos);
	}
	
	void createQuadraticEnemy(int i){
		Random.seed = (int)System.DateTime.Now.Ticks;
		int sign1 = Random.Range(0,100)>50?1:-1;
		int sign2 = Random.Range(0,100)>50?1:-1;
		int n1 = sign1 * Random.Range(3,10);
		int n2 = sign2 * Random.Range(3,10);
		Vector3 pos = new Vector3(n1, n2, 10);
		enemies[i] = (GameObject)Instantiate(Resources.Load("Enemy"));
		enemies[i].transform.position = pos;
		CurveMotion mEnemy = (CurveMotion)enemies[i].GetComponent(typeof(CurveMotion));
		mEnemy.functionType = "quadratic";
		mEnemy.moveDirection = -sign1;
		mEnemy.moveSpeed = 1f;
		mEnemy.setEquationFromStartPoint(pos);
	}
	
	void createExponentialEnemy(int i){
		Random.seed = (int)System.DateTime.Now.Ticks;
		int sign1 = Random.Range(0,100)>50?1:-1;
		int sign2 = 1;//Y needs to be always positive
		int n1 = sign1 * Random.Range(3,10);
		int n2 = sign2 * Random.Range(3,10);
		Vector3 pos = new Vector3(n1, n2, 10);
		enemies[i] = (GameObject)Instantiate(Resources.Load("Enemy"));
		enemies[i].transform.position = pos;
		CurveMotion mEnemy = (CurveMotion)enemies[i].GetComponent(typeof(CurveMotion));
		mEnemy.functionType = "exponential";
		mEnemy.moveDirection = -sign1;
		mEnemy.moveSpeed = 1f;
		mEnemy.setEquationFromStartPoint(pos);
	}
	
	void createSinusoidEnemy(int i){
		Random.seed = (int)System.DateTime.Now.Ticks;
		int sign1 = Random.Range(0,100)>50?1:-1;
		int n1 = sign1 * 10;
		float freq = Random.Range(1,100)*0.1f;
		Vector3 pos = new Vector3(n1, 0, 10);
		enemies[i] = (GameObject)Instantiate(Resources.Load("Enemy"));
		enemies[i].transform.position = pos;
		CurveMotion mEnemy = (CurveMotion)enemies[i].GetComponent(typeof(CurveMotion));
		mEnemy.functionType = "sinusoidal";
		mEnemy.functionParameter = freq;
		mEnemy.moveDirection = -sign1;
		mEnemy.moveSpeed = 1f;
		mEnemy.setEquationFromStartPoint(pos);
	}
}

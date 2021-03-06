﻿using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	
	public int score = 0;
	public int hiScore = 0;
	public float speed = 0.3f;
	public GUIText scoreDisplay;
	public GUIText hiScoreDisplay;
	public GUIText statusMessage;
	public AudioClip enemyDieSound;
	public AudioClip playerDieSound;
	public static int numberOfEnemies = 4;
	public GameObject[] enemies = new GameObject[numberOfEnemies];
	public Material[] enemyMaterials = new Material[8];
	
	public Material[] tutorials = new Material[6];
	
	public static int nInputLevels = 6;  // Number of intensity levels
	public static float[] linearInputLevels = {-1.1781f, -0.7854f, -0.3927f, 0.3927f, 0.7854f, 1.1781f}; 
	public static float[] quadInputLevels = {-2.0f, -1.0f, -0.1f, 0.1f, 1.0f, 2.0f}; 
	public static float[] expInputLevels = {-2.0f, -1.0f, -0.5f, 0.5f, 1.0f, 2.0f}; 
	public static float[] sinInputLevels = {0.2f, 0.3f, 0.5f, 0.7f, 1f, 1.2f}; 	
	public static float[] sinAmpLevels = {2f, 6f, 7f, 8f, 9f, 10f};
	private bool start = false;
	// Use this for initialization
	void Start () {
		start = false;
		showIntro();
	}
	
	void init(){
		Random.seed = (int)System.DateTime.Now.Ticks;
		int f = Random.Range(1,5);
		float s = speed + (0.2f*(int)(score/10));
		for(int i=0;i< enemies.Length; i++){
			switch(f){
			case 1:
				s += 1;
				createNewEnemy(i,"linear",s);
				break;
			case 2:
				s -= 0.1f;
				createNewEnemy(i,"quadratic",s);
				break;
			case 3:
				s += 0.5f;
				createNewEnemy(i,"exponential",s);
				break;
			case 4:
				createNewEnemy(i,"sinusoidal",s);
				break;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if(start==false){
				showTutorial("intro");
				start = true;
			}else{
				hideTutorial();
				Main.ResumeAll();
			}
		}
		
		if (paused) {
			return;
		}
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
		if (score >= hiScore) {
			hiScore = score;
			hiScoreDisplay.text = ("HI-Score: " + hiScore);
		}

		return score;
	}	
	
	void KillPlayer(){
		audio.PlayOneShot(playerDieSound);
		StartCoroutine("PlayerDeathMessage");
		score = 0;
		scoreDisplay.text = "Score: " + score;
	}
	
	
	void createNewEnemy(int i, string type, float speed){
		Random.seed = (int)System.DateTime.Now.Ticks;
		
		int param = Random.Range(0,6);
		Vector3[] x = new Vector3[4];
		x[0] = new Vector3(9.9f,CurveMotion.getY (9.9f,type,param),10);
		x[1] = new Vector3(-9.9f,CurveMotion.getY (-9.9f,type,param),10);
		x[2] = new Vector3(CurveMotion.getX (9.9f,type,param),9.9f,10);
		x[3] = new Vector3(CurveMotion.getX (-9.9f,type,param),-9.9f,10);
		
		
		
		for(int j=0;j<x.Length;j++){
			if((!float.IsNaN(x[j].y)) && (!float.IsNaN(x[j].x)) &&
				Mathf.Abs(x[j].y) <= 11 && Mathf.Abs(x[j].x) <=11){
				enemies[i] = (GameObject)Instantiate(Resources.Load("Enemy"));
				enemies[i].transform.position = x[j];
				CurveMotion mEnemy = (CurveMotion)enemies[i].GetComponent(typeof(CurveMotion));
				mEnemy.functionType = type;
				mEnemy.functionIndex = param;
				mEnemy.moveDirection = x[j].x>0?-1:1;
				mEnemy.moveSpeed = speed;
				SetProjectileColor(mEnemy);
				return;	
			}
		}
	}
	
	void PlayEnemyDieSound() {
		audio.PlayOneShot(enemyDieSound);
	}
	
	void SetProjectileColor(CurveMotion proj) {
		Renderer projRend;
		TrailRenderer trailRend;
		Color theColor = Color.black;
		Material theMaterial = enemyMaterials[0];
		
		switch (proj.functionType) {
		case "linear":
			theColor = new Color(0.00f,0.80f,0.00f,1.00f);
			if (proj.transform.position.x > 0) {
				theMaterial = enemyMaterials[0];
				}
				else {
						theMaterial = enemyMaterials[4];
					}
//			if (proj.transform.position.x < 0) {
//				scale = proj.transform.localScale;
//				scale.x = -scale.x;
//				proj.transform.localScale = scale;
//			}
			break;
		case "quadratic":
			theColor = new Color(0.00f,0.00f,0.90f,1.00f);
			if (proj.transform.position.x > 0) {
				theMaterial = enemyMaterials[1];
				}
				else {
						theMaterial = enemyMaterials[5];
					}
			break;
		/*case "hyperbolic":
			functionParameter = start.x * start.y;
			break;*/
		case "exponential":
			theColor = new Color(0.90f,0.00f,0.00f,1.00f);
			if (proj.transform.position.x > 0) {
				theMaterial = enemyMaterials[2];
				}
				else {
						theMaterial = enemyMaterials[6];
					}
			break;
		case "sinusoidal":
			theColor = new Color(0.90f,0.00f,0.90f,1.00f);
			if (proj.transform.position.x > 0) {
				theMaterial = enemyMaterials[3];
				}
				else {
						theMaterial = enemyMaterials[7];
					}
			break;
		}		

		projRend = (Renderer)proj.GetComponent("Renderer");
		projRend.material = theMaterial;
		

		trailRend = (TrailRenderer)proj.GetComponent("TrailRenderer");
		trailRend.material.SetColor("_Color",theColor);
	}
	
	public static float GetParameterValue(int index, string functionType) {

		float param;
		
		switch (functionType) {
		case "linear":
			param = linearInputLevels[index];
			break;
		case "quadratic":
			param = quadInputLevels[index];
			break;
		/*case "hyperbolic":
			functionParameter = start.x * start.y;
			break;*/
		case "exponential":
			param = expInputLevels[index];
			break;
		case "sinusoidal":
			param = sinInputLevels[index];
			break;
		default:
			param = 0.0f;
			break;
		}		

		return param;
	}
	
	void showTutorial(string type){
		GameObject tutorial = GameObject.Find("tutorial");
		if(tutorial == null){
			tutorial = GameObject.CreatePrimitive(PrimitiveType.Quad);
			tutorial.name = "tutorial";
		}
		tutorial.transform.position = new Vector3(0,0,2);
		tutorial.transform.localEulerAngles = new Vector3(0,0,0);
		tutorial.transform.localScale = new Vector3(36f,20f,2f);
		tutorial.renderer.enabled = true;
		
		switch(type){
		case "linear":
			tutorial.renderer.material = tutorials[0];
			break;
		case "quadratic":
			tutorial.renderer.material = tutorials[1];
			break;
		case "exponential":
			tutorial.renderer.material = tutorials[2];
			break;
		case "sinusoidal":
			tutorial.renderer.material = tutorials[3];
			break;
		case "logo":
			tutorial.renderer.material = tutorials[5];
			break;
		case "intro":
			tutorial.renderer.material = tutorials[4];
			break;
		}
		
		
	}
	
	
	void showIntro (){
		PauseAll();
		showTutorial("logo");
	}
	
	void hideTutorial(){
		GameObject tutorial = GameObject.Find("tutorial");
		if(tutorial != null){
			Destroy(tutorial);
		}
	}
	
	
	public static void PauseAll(){
		Object[] objects = FindObjectsOfType (typeof(GameObject));
		foreach (GameObject go in objects) {
			go.SendMessage ("OnPauseGame", SendMessageOptions.DontRequireReceiver);
		}	
	}
	
	public static void ResumeAll(){
		Object[] objects = FindObjectsOfType (typeof(GameObject));
		foreach (GameObject go in objects) {
			go.SendMessage ("OnResumeGame", SendMessageOptions.DontRequireReceiver);
		}	
	}
	
	protected bool paused;
	 
	void OnPauseGame (){
		scoreDisplay.text = ("");
		hiScoreDisplay.text = ("");
		statusMessage.text = ("");
		paused = true;
	}
	 
	void OnResumeGame (){
		scoreDisplay.text = ("Score: " + score);
		hiScoreDisplay.text = ("Hi-Score: " + hiScore);
		paused = false;
	}
	
	
	IEnumerator PlayerDeathMessage()
	{
		statusMessage.text = ("YOU GOT ROBO-SMACKED!  SCORE RESET.");
		yield return new WaitForSeconds(3);
		statusMessage.text = ("");
	}
	
}

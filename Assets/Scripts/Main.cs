using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	
	public int score;
	public float speed = 0.5f;
	public GUIText scoreDisplay;
	public GUIText statusMessage;
	public AudioClip enemyDieSound;
	public AudioClip playerDieSound;
	public static int numberOfEnemies = 4;
	public GameObject[] enemies = new GameObject[numberOfEnemies];
	public Material[] enemyMaterials = new Material[numberOfEnemies];
	
	// Use this for initialization
	void Start () {
		init ();
	}
	
	void init(){
		Random.seed = (int)System.DateTime.Now.Ticks;
		int f = Random.Range(1,5);
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
	
	void OnGUI () {
		GUI.color = Color.red;
		if (GUI.Button (new Rect (10,10,150,50), "1: Linear")) {
			GameObject obj = GameObject.Find("Player");
			obj.SendMessage("changeFunction","linear");		
		}
		GUI.color = Color.blue;
		if (GUI.Button (new Rect (10,70,150,50), "2: Quadratic")) {
			GameObject obj = GameObject.Find("Player");
			obj.SendMessage("changeFunction","quadratic");
		}
		GUI.color = Color.green;
		if (GUI.Button (new Rect (10,130,150,50), "3: Exponential")) {
			GameObject obj = GameObject.Find("Player");
			obj.SendMessage("changeFunction","exponential");
		}
		GUI.color = Color.magenta;
		if (GUI.Button (new Rect (10,190,150,50), "4: Sinusoid")) {
			GameObject obj = GameObject.Find("Player");
			obj.SendMessage("changeFunction","sinusoid");
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
		audio.PlayOneShot(playerDieSound);
		statusMessage.text = ("YOU HAVE DIED. YOU ARE DEAD.");
		score = 0;
		scoreDisplay.text = "Score: " + score;
	}
	
	void createLinearEnemy(int i){
		Random.seed = (int)System.DateTime.Now.Ticks;
		int sign1 = Random.Range(0,100)>50?1:-1;
		int sign2 = Random.Range(0,100)>50?1:-1;
		int n1 = sign1 * 10;//Random.Range(3,10);
		int n2 = sign2 * 10;//Random.Range(3,10);
		Vector3 pos = new Vector3(n1, n2, 10);
		enemies[i] = (GameObject)Instantiate(Resources.Load("Enemy"));
		enemies[i].transform.position = pos;
		CurveMotion mEnemy = (CurveMotion)enemies[i].GetComponent(typeof(CurveMotion));
		mEnemy.functionType = "linear";
		mEnemy.moveDirection = -sign1;
		mEnemy.moveSpeed = 2 + speed + (0.2f*(int)(score/10));
		mEnemy.setEquationFromStartPoint(pos);
		
		SetProjectileColor(mEnemy);
	}
	
	void createQuadraticEnemy(int i){
		Random.seed = (int)System.DateTime.Now.Ticks;
		int sign1 = Random.Range(0,100)>50?1:-1;
		int sign2 = Random.Range(0,100)>50?1:-1;
		int n1 = sign1 * 10;//Random.Range(3,10);
		int n2 = sign2 * 10;//Random.Range(3,10);
		Vector3 pos = new Vector3(n1, n2, 10);
		enemies[i] = (GameObject)Instantiate(Resources.Load("Enemy"));
		enemies[i].transform.position = pos;
		CurveMotion mEnemy = (CurveMotion)enemies[i].GetComponent(typeof(CurveMotion));
		mEnemy.functionType = "quadratic";
		mEnemy.moveDirection = -sign1;
		mEnemy.moveSpeed = speed + (0.2f*(int)(score/10));
		mEnemy.setEquationFromStartPoint(pos);

		SetProjectileColor(mEnemy);		
	}
	
	void createExponentialEnemy(int i){
		Random.seed = (int)System.DateTime.Now.Ticks;
		int sign1 = Random.Range(0,100)>50?1:-1;
		int sign2 = 1;//Y needs to be always positive
		int n1 = sign1 * 10;//Random.Range(3,10);
		int n2 = sign2 * 10;//Random.Range(3,10);
		Vector3 pos = new Vector3(n1, n2, 10);
		enemies[i] = (GameObject)Instantiate(Resources.Load("Enemy"));
		enemies[i].transform.position = pos;
		CurveMotion mEnemy = (CurveMotion)enemies[i].GetComponent(typeof(CurveMotion));
		mEnemy.functionType = "exponential";
		mEnemy.moveDirection = -sign1;
		mEnemy.moveSpeed = speed + (0.2f*(int)(score/10));
		mEnemy.setEquationFromStartPoint(pos);

		SetProjectileColor(mEnemy);	
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
		mEnemy.moveSpeed = speed + (0.2f*(int)(score/10));
		mEnemy.setEquationFromStartPoint(pos);

		SetProjectileColor(mEnemy);	
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
			theColor = Color.red;
			theMaterial = enemyMaterials[0];
			break;
		case "quadratic":
			theColor = Color.blue;
			theMaterial = enemyMaterials[1];
			break;
		/*case "hyperbolic":
			functionParameter = start.x * start.y;
			break;*/
		case "exponential":
			theColor = Color.green;
			theMaterial = enemyMaterials[2];
			break;
		case "sinusoidal":
			theColor = Color.magenta;
			theMaterial = enemyMaterials[3];
			break;
		}		

		projRend = (Renderer)proj.GetComponent("Renderer");
		projRend.material = theMaterial;
		

		trailRend = (TrailRenderer)proj.GetComponent("TrailRenderer");
		trailRend.material.SetColor("_Color",theColor);
	}
}

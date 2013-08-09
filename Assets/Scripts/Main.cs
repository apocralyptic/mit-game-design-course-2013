using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	
	public int score = 0;
	public int hiScore = 0;
	public float speed = 0.5f;
	public GUIText scoreDisplay;
	public GUIText hiScoreDisplay;
	public GUIText statusMessage;
	public AudioClip enemyDieSound;
	public AudioClip playerDieSound;
	public static int numberOfEnemies = 4;
	public GameObject[] enemies = new GameObject[numberOfEnemies];
	public Material[] enemyMaterials = new Material[numberOfEnemies];
	
	public Material[] tutorials = new Material[6];
	
	public static int nInputLevels = 6;  // Number of intensity levels
	public static float[] linearInputLevels = {-1.1781f, -0.7854f, -0.3927f, 0.3927f, 0.7854f, 1.1781f}; 
	public static float[] quadInputLevels = {-2.0f, -1.0f, -0.1f, 0.1f, 1.0f, 2.0f}; 
	public static float[] expInputLevels = {-2.0f, -1.0f, -0.5f, 0.5f, 1.0f, 2.0f}; 
	public static float[] sinInputLevels = {0.383f, 1.0f, 1.5f, 2.0f, 2.5f, 3.1416f}; 	
	
	private bool start = false;
	// Use this for initialization
	void Start () {
		//init ();
		start = false;
		showIntro();
	}
	
	void init(){
		Random.seed = (int)System.DateTime.Now.Ticks;
		int f = Random.Range(1,5);
		for(int i=0;i< enemies.Length; i++){
			switch(f){
			case 1:
				createLinearEnemy(i);
				//statusMessage.text = "Linear";
				break;
			case 2:
				createQuadraticEnemy(i);
				//statusMessage.text = "Quadratic";
				break;
			case 3:
				createExponentialEnemy(i);
				//statusMessage.text = "Exponential";
				break;
			case 4:
				createSinusoidEnemy(i);
				//statusMessage.text = "Sinusoid";
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
			theColor = new Color(0.00f,0.80f,0.00f,1.00f);
			theMaterial = enemyMaterials[0];
			break;
		case "quadratic":
			theColor = new Color(0.00f,0.00f,0.90f,1.00f);
			theMaterial = enemyMaterials[1];
			break;
		/*case "hyperbolic":
			functionParameter = start.x * start.y;
			break;*/
		case "exponential":
			theColor = new Color(0.90f,0.00f,0.00f,1.00f);
			theMaterial = enemyMaterials[2];
			break;
		case "sinusoidal":
			theColor = new Color(0.90f,0.00f,0.90f,1.00f);
			theMaterial = enemyMaterials[3];
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
		tutorial.transform.localScale = new Vector3(35f,19f,2f);
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

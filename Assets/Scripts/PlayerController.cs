using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	
	public string currentFunction = "linear";

	public float changeRate = 100.0f;
	public float shootDelay = 0.25f;  // Delay between shots
	//	public float changeRate = 100.0f;
	
	float currentParameter;	
	int currentInputLevel = 3;
	public AudioClip shootSound;
	bool canShoot = true;

	public GameObject userButton;
	Vector3 buttonDiff;
	
	GameObject[] capsule = new GameObject[4];
	
	public Texture[] targetTexture = new Texture[4];
	public Texture lightOff;
	public Texture lightOn;
	public GameObject[] lights = new GameObject[4];
	//float[] buttonPositions = new float[Main.nInputLevels];
	
	// Use this for initialization
	void Start ()
	{
		for(int i = 0;i<capsule.Length;i++){
			capsule[i] = GameObject.CreatePrimitive(PrimitiveType.Plane);
			capsule[i].name = "t"+i;
			capsule[i].transform.position = new Vector3(0,0,0);
			capsule[i].transform.localEulerAngles = new Vector3(270,0,0);
			capsule[i].transform.localScale = new Vector3(0.1f,0.1f,0.1f);
			capsule[i].renderer.material = (Material)Instantiate (Resources.Load ("target"));
		}
		// Read button positions for GUI management
		buttonDiff = new Vector3(0, GameObject.Find("Meter Hash 1").transform.position.y - GameObject.Find("Meter Hash 0").transform.position.y,0);
		currentParameter = Main.GetParameterValue(currentInputLevel,currentFunction); 
		
		GALevel level = new GALevel ();
		// Add the level to the save queue
		GoogleAnalytics.instance.Add (level);
		// Upload ALL the items in the save queue to Google
		GoogleAnalytics.instance.Dispatch ();
	}
	public void changeFunction(string f){
		this.currentFunction = f;
	}
	// Update is called once per frame
	void LateUpdate(){
		showTarget();
	}
	void Update ()
	{
		if (paused) {
			return;
		}
		if (Input.GetKeyDown ("space")) {
			if (canShoot) {
				StartCoroutine ("PlayerShoot");
			}
		}
			
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			if (currentInputLevel < Main.nInputLevels-1) {
				currentInputLevel++;
				currentParameter = Main.GetParameterValue(currentInputLevel,currentFunction);
				userButton.transform.Translate(buttonDiff,Space.World);
			}	
		}
			
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			if (currentInputLevel > 0) {
				currentInputLevel--;
				currentParameter = Main.GetParameterValue(currentInputLevel,currentFunction); 
				userButton.transform.Translate(-buttonDiff,Space.World);
			}	
		}
		
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			currentFunction = "linear";
			setLightOn(0);
			currentParameter = Main.GetParameterValue(currentInputLevel,currentFunction);
			GAEvent myEvent = new GAEvent ("GameAction", "FireWeapon", "LinearWeapon");
		    GoogleAnalytics.instance.Add (myEvent);
		    GoogleAnalytics.instance.Dispatch ();
		}

		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			currentFunction = "quadratic";
			setLightOn(1);
			currentParameter = Main.GetParameterValue(currentInputLevel,currentFunction);
			GAEvent myEvent = new GAEvent ("GameAction", "FireWeapon", "QuadraticWeapon");
		    GoogleAnalytics.instance.Add (myEvent);
		    GoogleAnalytics.instance.Dispatch ();			
		}

		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			currentFunction = "exponential";
			setLightOn(2);
			currentParameter = Main.GetParameterValue(currentInputLevel,currentFunction);
			GAEvent myEvent = new GAEvent ("GameAction", "FireWeapon", "ExponentialWeapon");
		    GoogleAnalytics.instance.Add (myEvent);
		    GoogleAnalytics.instance.Dispatch ();			
		}

		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			currentFunction = "sinusoidal";
			setLightOn(3);
			currentParameter = Main.GetParameterValue(currentInputLevel,currentFunction);
			GAEvent myEvent = new GAEvent ("GameAction", "FireWeapon", "SinusoidalWeapon");
		    GoogleAnalytics.instance.Add (myEvent);
		    GoogleAnalytics.instance.Dispatch ();			
		}
		/*if (Input.GetKeyDown (KeyCode.Alpha5)) {
			currentFunction = "hyperbolic";
		}*/
	}
	
	void turnOffLights(){
		foreach(GameObject g in lights){
			g.renderer.material.mainTexture = lightOff;	
		}
	}
	
	void setLightOn(int i){
		turnOffLights();
		lights[i].renderer.material.mainTexture = lightOn;
	}
	
	IEnumerator PlayerShoot ()
	{
		canShoot = false;
		audio.PlayOneShot(shootSound);

		createProjectile(new Vector3(0,0,10),1,"Projectile");
		createProjectile(new Vector3(0,0,10),-1,"Projectile");

		GAEvent myEvent = new GAEvent ("GameAction", "FireWeapon");
		GoogleAnalytics.instance.Add (myEvent);
		GoogleAnalytics.instance.Dispatch ();
		
		yield return new WaitForSeconds(shootDelay);
		canShoot = true;
	}
	
	void createProjectile(Vector3 pos, int direction,string type){
		GameObject p1 = (GameObject)Instantiate (Resources.Load (type));
		p1.transform.position = pos;
		CurveMotion mP1 = (CurveMotion)p1.GetComponent (typeof(CurveMotion));
		mP1.functionType = currentFunction;
		mP1.functionParameter = currentParameter;			
		mP1.moveDirection = direction;		

		GameObject obj = GameObject.Find("Main Camera");
		if(type=="Projectile"){
			obj.SendMessage("SetProjectileColor",mP1);
		}
	}
	
	void showTarget(){
		Vector3[] x = new Vector3[4];
		x[0] = new Vector3(9.9f,CurveMotion.getY (9.9f,currentFunction,currentParameter),4);
		x[1] = new Vector3(-9.9f,CurveMotion.getY (-9.9f,currentFunction,currentParameter),4);
		x[2] = new Vector3(CurveMotion.getX (9.9f,currentFunction,currentParameter),9.9f,4);
		x[3] = new Vector3(CurveMotion.getX (-9.9f,currentFunction,currentParameter),-9.9f,4);
		
		int textureIndex = 0;
	    switch(currentFunction){
	    case "linear":
	      textureIndex = 0;
	      break;
	    case "quadratic":
	      textureIndex = 1;
	      break;
	    case "exponential":
	      textureIndex = 2;
	      break;
	    case "sinusoidal":
	      textureIndex = 3;
	      break;
	    }
		for(int i=0;i<x.Length;i++){
			GameObject capsule = GameObject.Find("t"+i);
			if((!float.IsNaN(x[i].y)) && (!float.IsNaN(x[i].x)) &&
				Mathf.Abs(x[i].y) <= 11 && Mathf.Abs(x[i].x) <=11){
					capsule.transform.position = x[i];
					capsule.renderer.enabled = true;
				capsule.renderer.material.mainTexture = targetTexture[textureIndex];
			}else{
				capsule.renderer.enabled = false;
			}
			
		}
		if(currentFunction=="quadratic"){
			if((!float.IsNaN(x[2].y)) && (!float.IsNaN(x[2].x)) &&
				Mathf.Abs(x[2].y) <= 11 && Mathf.Abs(x[2].x) <=11){
				GameObject capsule = GameObject.Find("t"+3);
				capsule.transform.position = new Vector3(-x[2].x,x[2].y,x[2].z);
				capsule.renderer.enabled = true;
				capsule.renderer.material.mainTexture = targetTexture[textureIndex];
			}else{
				GameObject capsule = GameObject.Find("t"+2);
				capsule.transform.position = new Vector3(-x[3].x,x[3].y,x[3].z);
				capsule.renderer.enabled = true;
				capsule.renderer.material.mainTexture = targetTexture[textureIndex];
			}
		}
	}
	
	
	protected bool paused;
	 
	void OnPauseGame (){
		paused = true;
	}
	 
	void OnResumeGame (){
		paused = false;
	}
}
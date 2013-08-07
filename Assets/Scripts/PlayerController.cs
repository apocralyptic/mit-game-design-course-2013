using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	
	public float currentParameter = 1.0f;
	public string currentFunction = "linear";
	public float changeRate = 100.0f;
	public float shootDelay = 0.25f;  // Delay between shots
	
	public AudioClip shootSound;
	bool canShoot = true;
	
	// Use this for initialization
	void Start ()
	{
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
	void Update ()
	{
		if (Input.GetKeyDown ("space")) {
			if (canShoot) {
				StartCoroutine ("PlayerShoot");
			}
		}
			
		if (Input.GetKey (KeyCode.UpArrow)) {
			currentParameter += Time.deltaTime * changeRate;
	
		}
			
		if (Input.GetKey (KeyCode.DownArrow)) {
			currentParameter -= Time.deltaTime * changeRate;
		}
		
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			currentFunction = "linear";
			GAEvent myEvent = new GAEvent ("GameAction", "FireWeapon", "LinearWeapon");
		    GoogleAnalytics.instance.Add (myEvent);
		    GoogleAnalytics.instance.Dispatch ();
		}

		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			currentFunction = "quadratic";
			GAEvent myEvent = new GAEvent ("GameAction", "FireWeapon", "QuadraticWeapon");
		    GoogleAnalytics.instance.Add (myEvent);
		    GoogleAnalytics.instance.Dispatch ();			
		}

		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			currentFunction = "exponential";
			GAEvent myEvent = new GAEvent ("GameAction", "FireWeapon", "ExponentialWeapon");
		    GoogleAnalytics.instance.Add (myEvent);
		    GoogleAnalytics.instance.Dispatch ();			
		}

		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			currentFunction = "sinusoidal";
			GAEvent myEvent = new GAEvent ("GameAction", "FireWeapon", "SinusoidalWeapon");
		    GoogleAnalytics.instance.Add (myEvent);
		    GoogleAnalytics.instance.Dispatch ();			
		}
		
		/*if (Input.GetKeyDown (KeyCode.Alpha5)) {
			currentFunction = "hyperbolic";
		}*/
	}

	IEnumerator PlayerShoot ()
	{
		canShoot = false;
		audio.PlayOneShot(shootSound);
		
		if(currentFunction=="hyperbolic"){
			createProjectile(new Vector3(1,currentParameter,10),1);
			createProjectile(new Vector3(1,currentParameter,10),-1);
			
			createProjectile(new Vector3(-1,-currentParameter,10),1);
			createProjectile(new Vector3(-1,-currentParameter,10),-1);
			
		}else{
		
			createProjectile(new Vector3(0,0,10),1);
			createProjectile(new Vector3(0,0,10),-1);
			
		}
		GAEvent myEvent = new GAEvent ("GameAction", "FireWeapon");
		GoogleAnalytics.instance.Add (myEvent);
		GoogleAnalytics.instance.Dispatch ();
		
		yield return new WaitForSeconds(shootDelay);
		canShoot = true;
	}
	
	void createProjectile(Vector3 pos, int direction){
		GameObject p1 = (GameObject)Instantiate (Resources.Load ("Projectile"));
		p1.transform.position = pos;
		CurveMotion mP1 = (CurveMotion)p1.GetComponent (typeof(CurveMotion));
		mP1.functionType = currentFunction;
		mP1.functionParameter = currentParameter;			
		mP1.moveDirection = direction;		

		GameObject obj = GameObject.Find("Main Camera");
		obj.SendMessage("SetProjectileColor",mP1);		
	}
}
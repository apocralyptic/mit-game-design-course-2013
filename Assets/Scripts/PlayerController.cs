using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float currentParameter = 1.0f;
	public string currentFunction = "linear";
	public float changeRate = 100.0f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space")){
<<<<<<< HEAD
			GameObject p1 = (GameObject)Instantiate(Resources.Load("Projectile"));
			GameObject p2 = (GameObject)Instantiate(Resources.Load("Projectile"));

			p1.transform.position = new Vector3(0,0,10);
			p2.transform.position = new Vector3(0,0,10);

			CurveMotion mP1 = (CurveMotion)p1.GetComponent(typeof(CurveMotion));
			mP1.functionType = currentFunction;
			mP1.functionParameter = currentParameter;			
			mP1.moveDirection = 1;			

			CurveMotion mP2 = (CurveMotion)p2.GetComponent(typeof(CurveMotion));
			mP2.functionType = currentFunction;
			mP2.functionParameter = currentParameter;			
			mP2.moveDirection = -1;			
=======
			GameObject p = (GameObject)Instantiate(Resources.Load("Projectile"));
			p.transform.position = new Vector3(0,0,10);
			CurveMotion mP = (CurveMotion)p.GetComponent(typeof(CurveMotion));
			mP.functionType = "linear";//"quadratic";
			mP.functionParameter = currentParameter;
			mP.moveDirection = 1;			
>>>>>>> 166ab0814248cdf2f2796498ce97c46b844459e1
		}
			
		if (Input.GetKey(KeyCode.UpArrow)) {
			currentParameter += Time.deltaTime*changeRate;
	
		}
			
		if (Input.GetKey(KeyCode.DownArrow)) {
			currentParameter -= Time.deltaTime*changeRate;
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			currentFunction = "linear";
		}

			if (Input.GetKeyDown(KeyCode.Alpha2)) {
			currentFunction = "quadratic";
		}

			if (Input.GetKeyDown(KeyCode.Alpha3)) {
			currentFunction = "hyperbolic";
		}

			if (Input.GetKeyDown(KeyCode.Alpha4)) {
			currentFunction = "exponential";
		}

			if (Input.GetKeyDown(KeyCode.Alpha5)) {
			currentFunction = "sinusoidal";
		}
}
}

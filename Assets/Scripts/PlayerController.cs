﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	
	public float currentParameter = 1.0f;
	public string currentFunction = "linear";
	public float changeRate = 100.0f;
	public float shootDelay = 0.25f;  // Delay between shots
	
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
		}

		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			currentFunction = "quadratic";
		}

		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			currentFunction = "hyperbolic";
		}

		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			currentFunction = "exponential";
		}

		if (Input.GetKeyDown (KeyCode.Alpha5)) {
			currentFunction = "sinusoidal";
		}
	}

	IEnumerator PlayerShoot ()
	{
		canShoot = false;
		//audio.PlayOneShot(hitSound);
		//targetRoot.Play("down"); 

		GameObject p1 = (GameObject)Instantiate (Resources.Load ("Projectile"));
		GameObject p2 = (GameObject)Instantiate (Resources.Load ("Projectile"));

		p1.transform.position = new Vector3 (0, 0, 10);
		p2.transform.position = new Vector3 (0, 0, 10);

		CurveMotion mP1 = (CurveMotion)p1.GetComponent (typeof(CurveMotion));
		mP1.functionType = currentFunction;
		mP1.functionParameter = currentParameter;			
		mP1.moveDirection = 1;			

		CurveMotion mP2 = (CurveMotion)p2.GetComponent (typeof(CurveMotion));
		mP2.functionType = currentFunction;
		mP2.functionParameter = currentParameter;			
		mP2.moveDirection = -1;	
			
		GAEvent myEvent = new GAEvent ("GameAction", "FireWeapon");
		GoogleAnalytics.instance.Add (myEvent);
		GoogleAnalytics.instance.Dispatch ();
		
		yield return new WaitForSeconds(shootDelay);
		canShoot = true;
	}
}
﻿using UnityEngine;
using System.Collections;

public class CurveMotion : MonoBehaviour {
	
	public float moveSpeed;
	public float moveDirection;
	public string functionType;
	public float functionParameter;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float delta = Time.deltaTime*moveSpeed*moveDirection;
		transform.position = MoveAlongCurve(transform.position,delta,functionType,functionParameter);
	}
	
	// 
	Vector3 MoveAlongCurve(Vector3 currentPosition, float delta, string type, float param) {
		
		Vector3 newPosition = currentPosition;		
		newPosition.x = currentPosition.x + delta;  // Update x
		
		switch(functionType)
		{
			case "linear":
				newPosition.y = param*newPosition.x;			
				break;

		    case "quadratic":
				newPosition.y = param*newPosition.x*newPosition.x;
				break;
			
			case "hyperbolic":
				break;
			
			case "exponential":
				break;

			case "sinusoidal":
				newPosition.y = param * Mathf.Sin(2*Mathf.PI*0.2f*newPosition.x);
				break;
		}		
		
		if(newPosition.x > 20 || newPosition.y > 20){
			Destroy(this.gameObject);
		}
		return newPosition;
	}
	
	void OnTriggerEnter(Collider col) {
		// Debug-draw all contact points and normals
			/*if(typeof(contact.otherCollider.gameObject)==Enemy){
				this.gameObject.renderer.material.color = new Color(255f,0f,0f,1f);
			}*/
			Debug.Log ("Collision");
		}

}
	
	
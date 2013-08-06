using UnityEngine;
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
		if(transform.position.x > 10 || transform.position.y > 10 || 
			transform.position.x < -10 || transform.position.y < -10){
			Destroy(this.gameObject);
		}
	}
	
	// 
	Vector3 MoveAlongCurve(Vector3 currentPosition, float delta, string type, float param) {
		
		Vector3 newPosition = currentPosition;		
		
		switch(functionType) {

			case "linear":
 			    newPosition.x = currentPosition.x + delta*Mathf.Cos(param);
				newPosition.y = currentPosition.y + delta*Mathf.Sin(param);			
				break;

		    case "quadratic":
				newPosition.x = currentPosition.x + delta;
				newPosition.y = newPosition.x*newPosition.x;
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
		//Debug.Log (this.tag + " : " + col.tag);
		if(this.tag == "enemy" && col.tag == "projectile"){
			Debug.Log ("Kill enemy");
			((Main)GameObject.FindWithTag("MainCamera").camera.GetComponent(typeof(Main))).UpdateScore(1);
			Destroy(this.gameObject);
		}else if(this.tag == "enemy" && col.tag == "enemy"){
			Debug.Log ("Kill explosion");
		}else if(this.tag == "enemy" && col.tag == "Player"){
			Debug.Log("Kill player");
		}
	}

}
	
	
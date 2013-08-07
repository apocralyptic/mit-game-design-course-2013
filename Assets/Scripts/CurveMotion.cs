using UnityEngine;
using System.Collections;

public class CurveMotion : MonoBehaviour {
	
	public float moveSpeed;
	public float moveDirection;
	public string functionType;
	public float functionParameter = 0.0f;
	
	float frequencyScaling = 0.1f;
	float exponentialScaling = 0.5f;
	
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
	
	public void setEquationFromStartPoint(Vector3 start){
		switch(functionType){
		case "linear":
			//one point must always be (0,0,0) and the other one is the "start"
			functionParameter = Mathf.Atan(start.y/start.x);
			break;
		case "quadratic":
			functionParameter = start.y / (start.x * start.x);
			break;
		/*case "hyperbolic":
			functionParameter = start.x * start.y;
			break;*/
		case "exponential":
			functionParameter = Mathf.Log(start.y)/(start.x * exponentialScaling);
			break;
		case "sinusoidal":
			
			float y = 5*Mathf.Sin(2*Mathf.PI*frequencyScaling*functionParameter*start.x);
			Vector3 n = new Vector3(start.x,y,10);
			transform.position = n;
			break;
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
				newPosition.y = param*newPosition.x*newPosition.x;
				break;
			
			/*case "hyperbolic":
				newPosition.x = currentPosition.x + delta;
				newPosition.y = param/newPosition.x;
				break;*/
			
			case "exponential":
				newPosition.x = currentPosition.x + delta;
				newPosition.y = Mathf.Exp(exponentialScaling*param*newPosition.x) - 1;
				break;

			case "sinusoidal":
				newPosition.x = currentPosition.x + delta;
				newPosition.y = 5*Mathf.Sin(2*Mathf.PI*frequencyScaling*param*newPosition.x);
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
			GameObject obj = GameObject.Find("Main Camera");
			obj.SendMessage("KillPlayer");
		}
	}

}
	
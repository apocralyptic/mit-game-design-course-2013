using UnityEngine;
using System.Collections;

public class CurveMotion : MonoBehaviour {
	
	public float moveSpeed;
	public float moveDirection;
	public string functionType;
	//public float functionParameter = 0.0f;
	public int functionIndex = 0;
	
	public static float frequencyScaling = 0.1f;
	public static float exponentialScaling = 0.5f;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (paused) {
			return;
		}
		float delta = Time.deltaTime*moveSpeed*moveDirection;
		transform.position = MoveAlongCurve(transform.position,delta,functionType,functionIndex);
		if(Mathf.Abs(transform.position.x) > 10 || Mathf.Abs(transform.position.y) > 10){
			Destroy(this.gameObject);
		}
	}
	
	public static Vector3 MoveAlongCurve(Vector3 currentPosition, float delta, string type,int paramIndex) {
		
		Vector3 newPosition = currentPosition;		
		float param = Main.GetParameterValue(paramIndex,type);
		switch(type) {

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
				newPosition.y = Mathf.Exp(CurveMotion.exponentialScaling*param*newPosition.x) - 1;
				break;

			case "sinusoidal":
				newPosition.x = currentPosition.x + delta;
				newPosition.y = Main.sinAmpLevels[paramIndex]*Mathf.Sin(2*Mathf.PI*CurveMotion.frequencyScaling*param*newPosition.x);
				break;
		}		
		
		return newPosition;
	}
	
	public static float getY(float x, string type, int paramIndex){
		float y = 0.0f;
		float param = Main.GetParameterValue(paramIndex,type);
		switch(type) {
		case "linear":
 			y = Mathf.Tan(param) * x;			
			break;
		case "quadratic":
			y = param*x*x;
			break;
		case "exponential":
			y = Mathf.Exp(CurveMotion.exponentialScaling*param*x) - 1;
			break;
		case "sinusoidal":
			y = Main.sinAmpLevels[paramIndex]*Mathf.Sin(2*Mathf.PI*CurveMotion.frequencyScaling*param*x);
			break;
		}
		return y;
	}
	public static float getX(float y, string type, int paramIndex){
		float x = 0.0f;
		float param = Main.GetParameterValue(paramIndex,type);
		switch(type) {
		case "linear":
 			x = y/Mathf.Tan(param);
			break;
		case "quadratic":
			x = Mathf.Sqrt(y/param);
			break;
		case "exponential":
			x = Mathf.Log(y + 1)/(CurveMotion.exponentialScaling*param);
			break;
		case "sinusoidal":
			x = Mathf.Asin(y/Main.sinAmpLevels[paramIndex])/(2*Mathf.PI*CurveMotion.frequencyScaling*param);
			break;
		}
		return x;
	}
	
	void OnTriggerEnter(Collider col) {
		//Debug.Log (this.tag + " : " + col.tag);
		if(this.tag == "enemy" && col.tag == "projectile"){
			if(((CurveMotion)col.GetComponent(typeof(CurveMotion))).functionType == functionType){
				GameObject obj = GameObject.Find("Main Camera");
				obj.SendMessage("UpdateScore",1);
				obj.SendMessage("PlayEnemyDieSound");
				Destroy(this.gameObject);
			}
		}else if(this.tag == "enemy" && col.tag == "enemy"){
			//Debug.Log ("Kill explosion");
		}else if(this.tag == "enemy" && col.tag == "Player"){
			GameObject obj = GameObject.Find("Main Camera");
			obj.SendMessage("KillPlayer");
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
	
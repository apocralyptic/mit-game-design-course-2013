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
	}
	
	// 
	Vector3 MoveAlongCurve(Vector3 currentPosition, float delta, string type, float param) {
		
		Vector3 newPosition = currentPosition;
		
		switch(functionType)
		{
			case "linear":
				newPosition.x = newPosition.x + delta;			
				newPosition.y = newPosition.y + param*delta;			
				break;

		    case "parabola":
				break;
			
			case "hyperbola":
				break;
			
			case "exponential":
				break;

			case "sinusoid":
				break;
		}		
		
		return newPosition;
	}
}
	
	
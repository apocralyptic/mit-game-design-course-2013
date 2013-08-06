using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float currentParameter = 0.0f;
	public float changeRate = 10.0f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space")){
			GameObject p = (GameObject)Instantiate(Resources.Load("Projectile"));
			p.transform.position = new Vector3(0,0,10);
			CurveMotion mP = (CurveMotion)p.GetComponent(typeof(CurveMotion));
			mP.functionType = "linear";//"quadratic";
			mP.functionParameter = currentParameter;			
			mP.moveDirection = 1;
			
			GameObject p2 = (GameObject)Instantiate(Resources.Load("Projectile"));
			p2.transform.position = new Vector3(0,0,10);
			CurveMotion mP2 = (CurveMotion)p.GetComponent(typeof(CurveMotion));
			mP2.functionType = "linear";//"quadratic";
			mP2.functionParameter = currentParameter;			
			mP2.moveDirection = -1;
			
		}
			
		if (Input.GetKey(KeyCode.UpArrow)) {
			currentParameter += Time.deltaTime*changeRate;
	
		}
			
		if (Input.GetKey(KeyCode.DownArrow)) {
			currentParameter -= Time.deltaTime*changeRate;
		}
	}
}

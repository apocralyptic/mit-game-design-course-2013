using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject gEnemy = (GameObject)Instantiate(Resources.Load("Enemy"));
		gEnemy.transform.position = new Vector3(10,10,10);
		CurveMotion mEnemy = (CurveMotion)gEnemy.GetComponent(typeof(CurveMotion));
		mEnemy.functionType = "linear";//"quadratic";
		mEnemy.moveDirection = -1;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("space")){
			GameObject p = (GameObject)Instantiate(Resources.Load("Projectile"));
			p.transform.position = new Vector3(0,0,10);
			CurveMotion mP = (CurveMotion)p.GetComponent(typeof(CurveMotion));
			mP.functionType = "linear";//"quadratic";
			mP.moveDirection = 1;
		}
	
	}
}

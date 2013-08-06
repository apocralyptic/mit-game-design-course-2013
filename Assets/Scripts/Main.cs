using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject gEnemy = (GameObject)Instantiate(Resources.Load("Enemy"));
		gEnemy.rigidbody.position = new Vector3(1,2,3);
		CurveMotion mEnemy = (CurveMotion)gEnemy.GetComponent(typeof(CurveMotion));
		mEnemy.functionType = "sinusoidal";//"quadratic";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

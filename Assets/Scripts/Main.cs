using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	
	public int score;
	
	// Use this for initialization
	void Start () {
		GameObject gEnemy = (GameObject)Instantiate(Resources.Load("Enemy"));
		gEnemy.transform.position = new Vector3(10,10,10);
		CurveMotion mEnemy = (CurveMotion)gEnemy.GetComponent(typeof(CurveMotion));
		mEnemy.functionType = "linear";//"quadratic";
		mEnemy.moveDirection = -1;
		mEnemy.functionParameter = Mathf.PI/4;
	}
	
	// Update is called once per frame
	void Update () {	
	}
	
	public int UpdateScore(int i)
	{
		score = score + i;
		Debug.Log("Score! " + score);
		return score;
	}	
}

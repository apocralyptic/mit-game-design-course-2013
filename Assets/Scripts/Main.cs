using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	
	public int score;
	
	// Use this for initialization
	void Start () {
		Vector3 pos = new Vector3(7,10,10);
		GameObject gEnemy = (GameObject)Instantiate(Resources.Load("Enemy"));
		gEnemy.transform.position = pos;
		CurveMotion mEnemy = (CurveMotion)gEnemy.GetComponent(typeof(CurveMotion));
		mEnemy.functionType = "linear";//"quadratic";
		mEnemy.moveDirection = -1;
		mEnemy.setEquationFromStartPoint(pos);
		//mEnemy.functionParameter = Mathf.PI/4;
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

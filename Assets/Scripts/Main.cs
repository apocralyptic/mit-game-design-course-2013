using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	
	public int score;
	public GameObject[] enemies = new GameObject[2];
	// Use this for initialization
	void Start () {
		for(int i=0;i< enemies.Length; i++){
			createEnemy(i);
		}
		//mEnemy.functionParameter = Mathf.PI/4;
	}
	void createEnemy(int i){
		Random.seed = (int)System.DateTime.Now.Ticks;
			int sign1 = Random.Range(0,100)>50?1:-1;
			int sign2 = Random.Range(0,100)>50?1:-1;
			int n1 = sign1 * Random.Range(3,10);
			int n2 = sign2 * Random.Range(3,10);
			Vector3 pos = new Vector3(n1, n2, 10);
			Debug.Log(sign1+" "+sign2+" " + n1+ " " +n2);
			enemies[i] = (GameObject)Instantiate(Resources.Load("Enemy"));
			enemies[i].transform.position = pos;
			CurveMotion mEnemy = (CurveMotion)enemies[i].GetComponent(typeof(CurveMotion));
			mEnemy.functionType = "quadratic";//"quadratic";
			mEnemy.moveDirection = -sign1;
			mEnemy.moveSpeed = 1f;
			mEnemy.setEquationFromStartPoint(pos);
	}
	// Update is called once per frame
	void Update () {
		for(int i=0;i< enemies.Length; i++){
			if(enemies[i] == null){
				createEnemy (i);
			}
		}
	}
	
	public int UpdateScore(int i)
	{
		score = score + i;
		Debug.Log("Score! " + score);
		return score;
	}	
}

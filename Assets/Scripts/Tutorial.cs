using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseDown(){
		Main.PauseAll();
		GameObject obj = GameObject.Find("Main Camera");
		obj.SendMessage("showTutorial",this.tag);
	}
	
	
}

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void menu1(){
		Application.LoadLevel (1);
	}
	public void menu2(){
		Application.LoadLevel (2);
	}
	public void menu3(){
		Application.LoadLevel (3);
	}
	public void loadmenu(){
		Application.LoadLevel (0);
	}

}

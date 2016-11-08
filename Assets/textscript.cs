using UnityEngine;
using System.Collections;

public class textscript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<TextMesh>().text= this.transform.parent.gameObject.GetComponent<dataframe> ().packetNum.ToString() + " : " +this.transform.parent.gameObject.GetComponent<dataframe> ().data.ToString() ;
	}
}

using UnityEngine;
using System.Collections;

public class Reciever : MonoBehaviour {


	public GameObject ack;
	public char[] recievedparcel = new char[20];
	int num;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision data)
	{
		if (data.gameObject.name == "Packet") {
			//Debug.Log (data.gameObject.GetComponent<dataframe>().data);
			num = data.gameObject.GetComponent<dataframe> ().packetNum;
			recievedparcel[num]= data.gameObject.GetComponent<dataframe> ().data;
			Destroy (data.gameObject);
			SendACK (num);
		}
	}

	void SendACK(int packetnum){
		GameObject shoot;
		shoot = Instantiate (ack, transform.position+new Vector3(-1,1,0),ack.transform.rotation) as GameObject;
		shoot.name = "ack";
		shoot.GetComponent<dataframe> ().data = 'r';
		shoot.GetComponent<dataframe> ().packetNum = packetnum;
		shoot.GetComponent<Rigidbody>().AddForce(new Vector3(-5,0,0),ForceMode.VelocityChange);


	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Reciever : MonoBehaviour {


	public GameObject ack;	//prefab
	public char[] recievedparcel = new char[20];	//recieved data
	int num;	//packet number
	public Text RecievedData;	//billboard
//	int numcheck;	//to check for billboard, replace with to string later if necessary


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

			//update decieved data text
			string parcelstring = "";

			for(int i =0; i<20; i++)
			{
				parcelstring += recievedparcel[i];
			}
			RecievedData.text = "Received Data:"+parcelstring;
		}
	}

	void SendACK(int packetnum){
		GameObject shoot;
		shoot = Instantiate (ack, transform.position+new Vector3(-1,0.5f,0),ack.transform.rotation) as GameObject;
		shoot.name = "ack";
		shoot.GetComponent<Renderer> ().material.color = Color.yellow;
		shoot.GetComponent<dataframe> ().data = 'r';
		shoot.GetComponent<dataframe> ().packetNum = packetnum;
		shoot.GetComponent<Rigidbody>().AddForce(new Vector3(-5,0,0),ForceMode.VelocityChange);

	}
}

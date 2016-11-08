using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RecieverGoBackN : MonoBehaviour {

	public int windowSize = 4;
	public GameObject ack;	//prefab
	public char[] recievedparcel = new char[20];	//recieved data
	int num;	//packet number
	public Text RecievedData;	//billboard
	bool[] window = new bool[20]; //recieved list
	bool firstwindow = true;
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
			window [num] = true;
			Destroy (data.gameObject);
			bool test = true;
			for (int i = 0; i < num; i++) {
				if (window [i] == false)
					test = false;
			}

			int numfix =0;
			int numfix2 = 1;
			if (firstwindow == true) {
				numfix = 1;
				numfix2 = 0;
			}
			if (test && (((num + numfix2) % (windowSize - numfix)) == 0) && num > 0)
			{
				SendACK (num);
				firstwindow = false;
			}

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

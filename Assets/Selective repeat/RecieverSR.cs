using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class RecieverSR : MonoBehaviour {

	public int windowSize = 4;
	public GameObject ack;	//prefab
	public char[] recievedparcel = new char[20];	//recieved data
	int num;	//packet number
	public Text RecievedData;	//billboard
	bool[] window = new bool[20]; //recieved list
	bool firstwindow = true;
	int gotupto =0;
	int previousNACK;
	bool timeoutbool;
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
			gotupto = Math.Max(num,gotupto);
			bool test = true;
			for (int i = 0; i < gotupto; i++) {
				if (window [i] == false) {
					test = false;
					StartCoroutine( sendNACK (i));
				}
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

			else if (test && (((gotupto + numfix2) % (windowSize - numfix)) == 0) && gotupto > 0)
			{
				SendACK (gotupto);
				firstwindow = false;
			}

			//update decieved data text
			string parcelstring = "";

			for(int i =0; i<20; i++)
			{
				parcelstring += recievedparcel[i];
			}
			RecievedData.text = "Received Data:"+parcelstring;

			timeoutbool = true;
		}
	}

	IEnumerator sendNACK(int packetnum){
		if (packetnum != previousNACK) {
			yield return new WaitForSeconds (0.5f);
			GameObject shoot;
			shoot = Instantiate (ack, transform.position + new Vector3 (-1, 0.5f, 0), ack.transform.rotation) as GameObject;
			shoot.name = "nack";
			shoot.GetComponent<Renderer> ().material.color = Color.red;
			shoot.GetComponent<dataframe> ().data = 'n';
			shoot.GetComponent<dataframe> ().packetNum = packetnum;
			shoot.GetComponent<Rigidbody> ().AddForce (new Vector3 (-5, 0, 0), ForceMode.VelocityChange);
			previousNACK = packetnum;
		}
	}

	IEnumerator TimeOut(){
		yield return new WaitForSeconds (5);
		if (timeoutbool) {
			timeoutbool = false;
		} else if (timeoutbool) {
			sendNACK (gotupto);
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

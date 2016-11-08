using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SenderSR: MonoBehaviour {


	public GameObject message;


	public int windowSize = 4;
	public static string Parcel = "hello world!";	//string to be sent
	int ParcelPointer = -1;	//pointer to current packet
	char[] ParcelArr = Parcel.ToCharArray();	//list of packets
	public int packetnumber = -1;	//packet identification number
	bool started =false;	//game initializer
	bool acknowledged = false;	//timeout trigger
	public Text TimeoutDisplay;
	int timeouts =0;	//number of timeouts
//	int stopper = 0;	

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)&&!started) {
			StartCoroutine( sendWindow ());
			started = true;
		}
	}

	IEnumerator sendWindow(){
		for (int i = 0; i < windowSize; i++) {
			if (ParcelPointer < ParcelArr.GetLength (0) - 1) {
				++packetnumber;
				++ParcelPointer;
				SendMessage ();
				yield return new WaitForSeconds (1f);
			
			
				//	yield return new WaitForSeconds (10);

			}
		}
	//	if (ParcelPointer < ParcelArr.GetLength (0) - 1)
		StartCoroutine (timeout ());
	}

	void SendMessage()
	{
		GameObject shoot;
		shoot = Instantiate (message, transform.position+new Vector3(1,-0.5f,0),message.transform.rotation) as GameObject;
		shoot.name = "Packet";
		shoot.GetComponent<Renderer> ().material.color = Color.cyan;
		shoot.GetComponent<dataframe> ().data = this.ParcelArr [this.ParcelPointer];
		shoot.GetComponent<dataframe> ().packetNum = packetnumber;
		shoot.GetComponent<Rigidbody>().AddForce(new Vector3(5,0,0),ForceMode.VelocityChange);
		acknowledged = false;
	//	StartCoroutine( timeout ());
	}

	IEnumerator timeout(){
		Debug.Log ("stARTed");
		yield return new WaitForSeconds (10);

		if (!acknowledged) {
			Debug.Log ("triggered");
			packetnumber -= windowSize;
			ParcelPointer -= windowSize;
			StartCoroutine (sendWindow());
			TimeoutDisplay.text = "Windows Resend: "+ ++timeouts;
		}
	}
	void OnCollisionEnter(Collision ack){
		if (ack.gameObject.name == "ack") {
			int num;
			num = ack.gameObject.GetComponent<dataframe> ().packetNum;

			if (num == packetnumber) {
				acknowledged = true;
				Debug.Log("ack recieved");

				StopAllCoroutines ();	//to stop timeout
				Debug.Log("ack recieved");

				if (ParcelPointer < ParcelArr.GetLength(0)-1) {
					StartCoroutine(sendWindow()) ;
					//		stopper++;
				}
				Debug.Log ("stopped");

			}
			Destroy (ack.gameObject);
		}
		if (ack.gameObject.name == "nack") {
			int num;
			num = ack.gameObject.GetComponent<dataframe> ().packetNum;
			Sendagain (num);
			Destroy (ack.gameObject);
			}
			
		}



	void Sendagain(int id)
	{
		GameObject shoot;
		shoot = Instantiate (message, transform.position+new Vector3(1,-0.5f,0),message.transform.rotation) as GameObject;
		shoot.name = "Packet";
		shoot.GetComponent<Renderer> ().material.color = Color.blue;
		shoot.GetComponent<dataframe> ().data = this.ParcelArr [id];
		shoot.GetComponent<dataframe> ().packetNum = id;
		shoot.GetComponent<Rigidbody>().AddForce(new Vector3(5,0,0),ForceMode.VelocityChange);
		acknowledged = false;
		//	StartCoroutine( timeout ());
	}


}

using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;

public class client :
MonoBehaviour
{
	bool socketReady = false;
	float x = 0;
	int y = 0;
	float z = 0;
	float k1=0.8f;
	float k2=1.0f;
	float xref=0;
	float zref=0;
	int r=1;
	String x1 = "";
	String y1 = "";
	String z1 = "";
	int[,] array = new int[5,3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
	Vector3 dir = new Vector3 (0, 0, 0);
	Vector3 lineOrigin = new Vector3(0.0f, 0.0f, 0.0f);
	TcpClient mySocket;
	NetworkStream theStream;
	StreamWriter theWriter;
	StreamReader theReader;
	String Host = "192.168.43.123";
	Int32 Port = 8080;
	private LineRenderer laserLine;	
	private bool dirRight = true;
	//public float speed = 2.0f;

	// Use this for initialization
	void Start()
	{
		laserLine = GetComponent<LineRenderer>();
		laserLine.enabled = true;
		setupSocket();
	}

	// Update is called once per frame
	void Update()
	{String a=readSocket();
		if (!a.Equals(""))
		{
			x1 = "";
			y1 = "";
			z1 = "";
			int o = 0;
			for (int i = 0; i < a.Length; i++)
			{
				if (o == 0)
				{
					if (a[i] == ' ')
					{
						o = 1;
						continue;
					}
					x1 = x1 + a[i];

				}
				if (o == 1)
				{
					if (a[i] == ' ')
					{
						o = 2;
						continue;
					}
					z1 = z1 + a[i];

				}
				if (o == 2)
				{
					y1 = y1 + a[i];

				}
			}
			print(a);

			x=float.Parse(x1);
			Int32.TryParse(y1, out y);
			z=float.Parse(z1);

			if (r == 1) {
				xref = x;
				zref = z;
				r = 2;
			}
			x = (float)(x -xref);
			z = (float)(z -zref);
			transform.position = new Vector3((float)(k1*x+3.92), (float)(k2*z+16.5),51.48f );

			dir.x =(float) (k1*x+3.92);
			dir.y = (float)(k2 * z+16.5);
			dir.z = 51.48f;
			// Draw a line in the Scene View  from the point lineOrigin in the direction of fpsCam.transform.forward * weaponRange, using the color green
			//Debug.DrawRay(lineOrigin, dir , Color.green);
			laserLine.SetPosition (0, lineOrigin);
			laserLine.SetPosition (1,dir);


		}
		maintainConnection();
//		if (y==1 && Time.time > nextFire)
//		{
//			// Update the time when our player can fire next
//			nextFire = Time.time + fireRate;
//
//			// Start our ShotEffect coroutine to turn our laser line on and off
//			//StartCoroutine(ShotEffect());
//
//			// Create a vector at the center of our camera's viewport
//			//Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f));
//			//Vector3 a = new Vector3 (0f, 0f, 1f);
//
//			// Declare a raycast hit to store information about what our raycast has hit
//			RaycastHit hit;
//
//			// Set the start position for our visual effect for our laser to the position of gunEnd
//			//laserLine.SetPosition(0, dir);
//
//			// Check if our raycast has hit anything
//			if (Physics.Raycast(lineOrigin, dir, out hit))
//			{
//				// Set the end position for our laser line 
//				//laserLine.SetPosition(1, hit.point);
//
//				// Get a reference to a health script attached to the collider we hit
//				ShootableBox health = hit.collider.GetComponent<ShootableBox>();
//
//				// If there was a health script attached
//				if (health != null)
//				{
//					// Call the damage function of that script, passing in our gunDamage variable
//					health.Damage(gunDamage);
//				}
//
//			}
//		
//		}	
	}

	public void setupSocket()
	{
		try
		{
			mySocket = new TcpClient(Host, Port);
			theStream = mySocket.GetStream();
			theWriter = new StreamWriter(theStream);
			theReader = new StreamReader(theStream);
			socketReady = true;
			print("oyu");
		}
		catch (Exception e)
		{
			Debug.Log("Socket error:" + e);
		}
	}

	public void writeSocket(string theLine)
	{
		if (!socketReady)
			return;
		String tmpString = theLine + "\r\n";
		theWriter.Write(tmpString);
		theWriter.Flush();
	}

	public String readSocket()
	{
		print("dekha");
		if (!socketReady)
			return "";
		//print("FO");
		if (theStream.DataAvailable)
		{
			print("LEo");
			return theReader.ReadLine();
		}
		return "";
	}

	public void closeSocket()
	{
		if (!socketReady)
			return;
		theWriter.Close();
		theReader.Close();
		mySocket.Close();
		socketReady = false;
	}

	public void maintainConnection()
	{
		if (!theStream.CanRead)
		{
			setupSocket();
		}
	}

} // end class s_TCP
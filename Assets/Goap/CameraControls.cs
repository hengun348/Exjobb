using UnityEngine;
using System.Collections;
 
[AddComponentMenu("Camera/CameraControls")]
public class CameraControls : MonoBehaviour {

  	float speed = 25.0f;
	SmoothFollow follower;
	
	void Start()
	{
		follower = (SmoothFollow)Camera.main.gameObject.GetComponent("SmoothFollow");
	}
    
	void Update() {
		if((Input.GetAxis("Horizontal") != 0) || (Input.GetAxis("Vertical") != 0))
		{	
			if(follower.target == null )
			{
				//just move the camera
				float x = Input.GetAxis("Horizontal") * speed;
				float z = Input.GetAxis("Vertical") * speed;
				x *= Time.deltaTime;
				z *= Time.deltaTime;
				transform.position += new Vector3(x, 0, z);
			}
			else if(!follower.target.Equals(GameObject.Find("Ground").transform))
			{
				follower.height = 20;
				follower.distance = 20;
				follower.target = GameObject.Find("Ground").transform;
			}
			else if (follower.target.Equals(GameObject.Find("Ground").transform))
			{
				if(follower.wantedHeight - follower.currentHeight < 1)
				{
					follower.target = null;
				}
			}
		}
		
		if(Input.GetAxis("Mouse ScrollWheel") != 0)
		{
			if(follower.target != null )
			{
				follower.distance -= Input.GetAxis("Mouse ScrollWheel") * speed;
			}
			else
			{
				float y = Input.GetAxis("Mouse ScrollWheel") * speed;
				y *= Time.deltaTime;
				transform.position += new Vector3(0, -y, 0);
			}
		}
    }
}
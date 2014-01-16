using UnityEngine;
using System.Collections;

public class PlanGUI : MonoBehaviour {
	Color color; 

	public float duration = 40.0f; 
	
	// Use this for initialization
	void Start () {
		color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		gameObject.renderer.material.color = color;
		

 
		gameObject.renderer.material.shader = Shader.Find("Transparent/Diffuse");
	}
	
	void Update() { 
		transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);

		if(Input.GetKeyUp(KeyCode.T))
	    {
	        StartCoroutine(FadeTo(0.0f, 1.0f));
	    }
	    if(Input.GetKeyUp(KeyCode.F))
	    {
	        StartCoroutine(FadeTo(1.0f, 1.0f));
	    }
		
	} 
	
 
	//IEnumerator FadeTo(float aValue, float aTime)
	public IEnumerator FadeTo(float aValue, float aTime)
	{
	    float alpha = transform.renderer.material.color.a;
		
		transform.position = gameObject.transform.parent.transform.position + new Vector3(0, 1.0f, 0);
		
	    for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
	    {
	        Color newColor = new Color(color.r, color.g, color.b, Mathf.Lerp(alpha,aValue,t));
	        transform.renderer.material.color = newColor;
			if(aValue == 0.0f)
				transform.position += new Vector3(Time.deltaTime*-1, 0, 0);
	        yield return null;
	    }
	}
}

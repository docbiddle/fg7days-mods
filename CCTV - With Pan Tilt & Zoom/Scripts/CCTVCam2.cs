using UnityEngine;
using System.Collections;

public class CCTVCam2 : MonoBehaviour
{
	
	public Camera renderCam2;
	public GameObject renderCam2OBJ;
	public GameObject CameraModel1;
	public bool isInputAllowed = false;
	public float TurnSpeed;
	public float StartAngle;//Pan Left
	public float EndAngle;//Pan Right
	private float currentAngle;
	
	//Tilt
	public GameObject CameraModel2;//Tilt
	public float tiltStartAngle = -15;
	public float tiltEndAngle = 20;
	public float tiltAngle;
	public float TiltSpeed;
	
	//Zoom
	public float minFov;
	public float maxFov = 29.5f;
	public float ZoomLevelFromXML;
	public float zoomSpeedFromXML;
	
	//Light
	public Light light;
	public GameObject spotlightOBJ;
	public bool canUseLights;
	
	public bool isInterfaceDisabled;
	
	private static void DisplayChatAreaText(string str)
    {
		EntityAlive entity = default(EntityAlive);//Say as server.
		//Say as Local player//EntityAlive entity = GameManager.Instance.World.GetLocalPlayer();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameMessage(EnumGameMessages.Chat, str, entity);
        }
    }
	
	void Start() 
	{
		minFov = ZoomLevelFromXML;
		renderCam2OBJ = this.transform.Find("CameraMount2/CameraSwivel2/CameraBodyOB/CameraBody/RenderCamera").gameObject;
		renderCam2 = renderCam2OBJ.GetComponent<Camera>();
		renderCam2.enabled = false;
		CameraModel1 = this.transform.Find("CameraMount2/CameraSwivel2").gameObject;
		CameraModel2 = this.transform.Find("CameraMount2/CameraSwivel2/CameraBodyOB").gameObject;
		spotlightOBJ = this.transform.Find("CameraMount2/CameraSwivel2/CameraBodyOB/CameraBody/Spotlight").gameObject;
		light = spotlightOBJ.GetComponent<Light>();
		light.enabled = false;
	}
	
	void Update() 
	{
		if (Input.GetKeyUp(KeyCode.Keypad2))
		{
			isInputAllowed = true;
			renderCam2.enabled = true;
		}
		
		
		if (Input.GetKeyUp(KeyCode.Keypad1))
		{
			isInputAllowed = false;
			renderCam2.enabled = false;
			light.enabled = false;
		}
		
		if (Input.GetKeyUp(KeyCode.Keypad3))
		{
			isInputAllowed = false;
			renderCam2.enabled = false;
			light.enabled = false;
		}
		
		if (Input.GetKeyUp(KeyCode.Keypad4))
		{
			isInputAllowed = false;
			renderCam2.enabled = false;
			light.enabled = false;
		}
		
		if (Input.GetKeyUp(KeyCode.E))
		{
			isInputAllowed = false;
			renderCam2.enabled = false;
			light.enabled = false;
		}
		
		if (isInterfaceDisabled)
		{
			isInputAllowed = false;
		}
		
		if (isInputAllowed)
		{
			MoveCamera2();
		}
	}
	
	void MoveCamera2()
	{
		if (!isInputAllowed)
		{
			return;
		}
		if (isInputAllowed)
		{
			if (!renderCam2.enabled)
			{
				renderCam2.enabled = true;
			}
			
			//Pan the camera left or right.
			if(Input.GetKey(KeyCode.LeftArrow))
			{
				if (Mathf.Abs(StartAngle - currentAngle) > 10)
				{
					currentAngle = Mathf.LerpAngle(currentAngle, StartAngle, TurnSpeed * Time.deltaTime);
				}
			}
			if(Input.GetKey(KeyCode.RightArrow))
			{
				
				if (Mathf.Abs(EndAngle - CameraModel1.transform.rotation.eulerAngles.y) > 10)
				{
					currentAngle = Mathf.LerpAngle(currentAngle, EndAngle, TurnSpeed * Time.deltaTime);
				}
			}
			
			//Tilt the camera up or down.
			if(Input.GetKey(KeyCode.UpArrow))
			{
				if (Mathf.Abs(tiltStartAngle - tiltAngle) > 0.1)
				{
					tiltAngle = Mathf.LerpAngle(tiltAngle, tiltStartAngle, TiltSpeed * Time.deltaTime);
				}
			}
			if(Input.GetKey(KeyCode.DownArrow))
			{
				
				if (Mathf.Abs(tiltEndAngle - CameraModel2.transform.rotation.eulerAngles.x) > 0.1)
				{
					tiltAngle = Mathf.LerpAngle(tiltAngle, tiltEndAngle, TiltSpeed * Time.deltaTime);
				}
			}
			
			//Zoom
			if(Input.GetKey(KeyCode.KeypadMinus))
			{
				float Zoom = renderCam2.fieldOfView;
				Zoom = Mathf.Clamp(Zoom, minFov, maxFov);
				Zoom += zoomSpeedFromXML;
				renderCam2.fieldOfView = Zoom;
			}
			
			if(Input.GetKey(KeyCode.KeypadPlus))
			{
				float Zoom = renderCam2.fieldOfView;
				Zoom = Mathf.Clamp(Zoom, minFov, maxFov);
				Zoom -= zoomSpeedFromXML;
				renderCam2.fieldOfView = Zoom;
			}
			
			//Light
			if (Input.GetKeyUp(KeyCode.L))
			{
				if (canUseLights)
				{
					light.enabled = !light.enabled;
				}
			}
		}
		
		CameraModel2.transform.localEulerAngles = new Vector3(tiltAngle, currentAngle, CameraModel1.transform.rotation.eulerAngles.z);
	}
	
	
}
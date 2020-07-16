using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float moveSpeed;
	public GameObject image;
	public GameObject sword;
	public float distance;
	public float activeSwingDirection;
	
	private Vector3 currentPos;
	private Vector3 bodyOffset;
	private Vector3 bodyNewPos;
	private float mouseX;
	private float mouseY;
	private float stabilizeZ;
	private Vector3 camDirection;
	private Vector3 restrictedCamDirection;
	private RectTransform rt;
	private Transform swordTrans;
	private float swingDirection;
	private float timer;
	private float timeLimit;
	private bool swordSwinging;
	private bool swingSetup;
	private Vector3 holdingAdjustment;
	private bool holdingSword;
	private float cleanTimer;
	private float cleanTimeLimit;
    // Start is called before the first frame update
    void Start()
    {
		cleanTimeLimit = 30.0f;
		cleanTimer = cleanTimeLimit;
		timeLimit = 0.9f;
		timer = timeLimit;
		holdingSword = false;
		swordSwinging = false;
		swingSetup = false;
		currentPos = this.transform.position;
		swordTrans = sword.GetComponent(typeof(Transform)) as Transform;
		rt = image.GetComponent(typeof(RectTransform)) as RectTransform;
		Cursor.lockState = CursorLockMode.Locked;
		holdingAdjustment = new Vector3(0.0f,-0.30f,0.0f);
		swordTrans.position = new Vector3(0.0f,-5.0f,0.0f);
    }

    // Update is called once per frame
    void Update()
    {
		holdingSword = this.GetComponent<InteractionController>().swordState;
		if(swordSwinging == true)
		{
			timer = timer - Time.deltaTime;
		}
		if(timer <= 0.0f)
		{
			swordSwinging = false;
			timer = timeLimit;
		}
		if(cleanTimer <= 0.0f)
		{
			GameObject[] cutPieces = GameObject.FindGameObjectsWithTag("CutPiece");
			for(int i = 0;i < cutPieces.Length;i++)
			{
				Destroy(cutPieces[i]);
			}
			cleanTimer = cleanTimeLimit;
		}
		cleanTimer = cleanTimer - Time.deltaTime;
		CameraMovement();
		PlayerMovement();
		SwingDisplay();
		if(holdingSword)
		{
			SwingControl();
		}
    }
	
	void CameraMovement()
	{
		camDirection = this.transform.forward;
		mouseX = Input.GetAxis("Mouse X");
		mouseY = Input.GetAxis("Mouse Y");
		this.transform.Rotate(-mouseY, mouseX, 0.0f);
		stabilizeZ = -(this.transform.eulerAngles.z);
		this.transform.Rotate(0.0f,0.0f,stabilizeZ);
	}
	
	void PlayerMovement()
	{
		if(Input.GetKey("w"))
		{
			restrictedCamDirection = new Vector3(camDirection.x,0.0f,camDirection.z);
			this.transform.position = this.transform.position + (restrictedCamDirection * moveSpeed);
		}
		if(Input.GetKey("s"))
		{
			restrictedCamDirection = new Vector3(camDirection.x,0.0f,camDirection.z);
			this.transform.position = this.transform.position - (restrictedCamDirection * moveSpeed);
		}
	}
	
	void SwingDisplay()
	{
		if(mouseX != 0.0f && mouseY != 0.0f && swordSwinging == false)
		{
			if(mouseY > 0.0f && mouseX < 0.0f)
			{
				swingDirection = 2.0f;
				rt.localRotation = Quaternion.Euler(0.0f,0.0f,45.0f);
			} else if(mouseY > 0.0f && mouseX > 0.0f)
			{
				swingDirection = 3.0f;
				rt.localRotation = Quaternion.Euler(0.0f,0.0f,-45.0f);
			} else if(mouseX < 0.0f)
			{
				swingDirection = 1.0f;
				rt.localRotation = Quaternion.Euler(0.0f,0.0f,90.0f);
			} else if(mouseX > 0.0f)
			{
				swingDirection = 4.0f;
				rt.localRotation = Quaternion.Euler(0.0f,0.0f,-90.0f);
			}
		}
	}
	void SwordAnimation(float requiredDirection, float degrees)
	{
		if(swordSwinging == true && activeSwingDirection == requiredDirection)
		{
			if(swingSetup == true)
			{
				swordTrans.rotation = Quaternion.Euler(swordTrans.transform.eulerAngles.x,swordTrans.transform.eulerAngles.y,swordTrans.transform.eulerAngles.z + degrees);
				swingSetup = false;
			}
			swordTrans.position = this.transform.position + (this.transform.forward) * distance + holdingAdjustment;
			swordTrans.Rotate(1.6f,0.0f,0.0f);
		} else if (swordSwinging == false)
		{
			swordTrans.position = this.transform.position + (this.transform.forward) * distance + holdingAdjustment;
			swordTrans.rotation = this.transform.rotation;
			swordTrans.Rotate(0.0f,-20.0f,-45.0f);
		}
	}
	void SwingControl()
	{
		if(Input.GetMouseButtonDown(0) && swordSwinging == false)
		{
			swordSwinging = true;
			swingSetup = true;
			activeSwingDirection = swingDirection;
		}
		SwordAnimation(1.0f, 135.0f);
		SwordAnimation(2.0f, 90.0f);
		if(swordSwinging == true && activeSwingDirection == 3.0f)
		{
			swordTrans.position = this.transform.position + (this.transform.forward) * distance + holdingAdjustment;
			swordTrans.Rotate(1.6f,0.0f,0.0f);
		} else if (swordSwinging == false)
		{
			swordTrans.position = this.transform.position + (this.transform.forward) * distance + holdingAdjustment;
			swordTrans.rotation = this.transform.rotation;
			swordTrans.Rotate(0.0f,-20.0f,-45.0f);
		}
		SwordAnimation(4.0f, -45.0f);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
	public GameObject storagePanel;
	public GameObject storageButton;
	public GameObject increaseButton;
	public GameObject decreaseButton;
	public GameObject spawnButton;
	public GameObject moveButton;
	public GameObject sword;
	public bool swordState;
	public GameObject dummyNumber;
	public GameObject dummyPrefab;
	
	private GameObject hitObject;
	private RaycastHit hit;
	private bool doorOpen;
	private bool doorOpening;
	private bool doorClosing;
	private int selectedNum;
	private TextMesh text;
	private GameObject[] dummyHolder;
	private bool dummyMoving;
	private bool movingForwards;
	private bool movingBackwards;
	private float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
		movingForwards = true;
		movingBackwards = false;
		dummyMoving = false;
        doorOpen = false;
		doorOpening = false;
		doorClosing = false;
		text = dummyNumber.GetComponent<TextMesh>();
		selectedNum = 0;
		dummyHolder = new GameObject[6];
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(this.transform.position, this.transform.forward);
		if(Physics.Raycast(ray, out hit))
		{
			Debug.Log(hit.collider.gameObject.name);
			hitObject = hit.collider.gameObject;
			if((hitObject.name == "Storage Button Panel" || hitObject.name == "Storage Button") && !(doorOpen) && Input.GetKeyDown(KeyCode.E))
			{
				doorOpening = true;
			}
			if((hitObject.name == "Storage Button Panel" || hitObject.name == "Storage Button") && doorOpen && Input.GetKeyDown(KeyCode.E))
			{
				doorClosing = true;
			}
			buttonAnimator(storageButton, "Storage Button", hitObject);
			if((hitObject.name == "Blade" || hitObject.name == "Guard" || hitObject.name == "Modified Sword") && Input.GetKeyDown(KeyCode.E))
			{
				swordState = true;
				Destroy(sword);
			}
			if((hitObject.name == "Increase Button Panel" || hitObject.name == "Increase Button") && Input.GetKeyDown(KeyCode.E) && selectedNum < 4)
			{
				selectedNum = selectedNum + 1;
				text.text = "Current\nDummy\nNumber\n\n  - "+ selectedNum + "  +";
			}
			buttonAnimator(increaseButton, "Increase Button", hitObject);
			if((hitObject.name == "Decrease Button Panel" || hitObject.name == "Decrease Button") && Input.GetKeyDown(KeyCode.E) && selectedNum > 0)
			{
				selectedNum = selectedNum - 1;
				text.text = "Current\nDummy\nNumber\n\n  - "+ selectedNum + "  +";
			}
			buttonAnimator(decreaseButton, "Decrease Button", hitObject);
			if((hitObject.name == "Spawn Button Panel" || hitObject.name == "Spawn Button") && Input.GetKeyDown(KeyCode.E) && selectedNum >= 0 && selectedNum <= 4)
			{
				DummySpawn();
			}
			buttonAnimator(spawnButton, "Spawn Button", hitObject);
			if((hitObject.name == "Move Button Panel" || hitObject.name == "Move Button") && Input.GetKeyDown(KeyCode.E) && !dummyMoving)
			{
				dummyMoving = true;
			} else if((hitObject.name == "Move Button Panel" || hitObject.name == "Move Button") && Input.GetKeyDown(KeyCode.E) && dummyMoving)
			{
				dummyMoving = false;
			}
			buttonAnimator(moveButton, "Move Button", hitObject);
		}
		StorageMove();
		DummyAnimator();
    }
	void buttonAnimator(GameObject button, string buttonName, GameObject ho)
	{
		if((ho.name == buttonName + " Panel" || ho.name == buttonName) && Input.GetKey(KeyCode.E))
		{
			button.transform.localScale = new Vector3(button.transform.localScale.x,0.0f,button.transform.localScale.z);
		} else {
			button.transform.localScale = new Vector3(button.transform.localScale.x,0.05f,button.transform.localScale.z);
		}
	}		
	void StorageMove()
	{
		Transform panelT = storagePanel.transform;
		if(doorOpening && panelT.localScale.y >= 0.0f)
		{
			panelT.position = new Vector3(panelT.position.x,(panelT.position.y + 0.01f),panelT.position.z);
			panelT.localScale = new Vector3(panelT.localScale.x,(panelT.localScale.y - 0.02f),panelT.localScale.z);
		}
		if(doorOpening && panelT.localScale.y <= 0.0f)
		{
			doorOpening = false;
			doorOpen = true;
		}
		if(doorClosing && panelT.localScale.y <= 2.6f)
		{
			panelT.position = new Vector3(panelT.position.x,(panelT.position.y - 0.01f),panelT.position.z);
			panelT.localScale = new Vector3(panelT.localScale.x,(panelT.localScale.y + 0.02f),panelT.localScale.z);
		}
		if(doorClosing && panelT.localScale.y >= 2.6f)
		{
			doorClosing = false;
			doorOpen = false;
		}
	}
	void DummySpawn()
	{
		for(int i = 0;i < dummyHolder.Length;i++)
		{
			Destroy(dummyHolder[i]);
			if(i < selectedNum)
			{
				dummyHolder[i] = Instantiate(dummyPrefab, new Vector3(10.8f - (i * 3.0f),0.7f,8.2f), Quaternion.identity);
			}
		}
	}
	void DummyAnimator()
	{
		for(int i = 0;i < dummyHolder.Length;i++)
		{
			if(dummyHolder[i] != null && dummyMoving)
			{
				moveSpeed = Random.Range(0.02f,0.03f);
				if(movingForwards && dummyHolder[i].transform.position.z >= 5.1f)
				{
					dummyHolder[i].transform.position = new Vector3(dummyHolder[i].transform.position.x,dummyHolder[i].transform.position.y,(dummyHolder[i].transform.position.z - moveSpeed));
				} else {
					movingForwards = false;
					movingBackwards = true;
				}
				if(movingBackwards && dummyHolder[i].transform.position.z <= 8.2f)
				{
					dummyHolder[i].transform.position = new Vector3(dummyHolder[i].transform.position.x,dummyHolder[i].transform.position.y,(dummyHolder[i].transform.position.z + moveSpeed));
				} else {
					movingForwards = true;
					movingBackwards = false;
				}
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceController : MonoBehaviour
{
	public GameObject guard;
	public GameObject grip;
	public Material botMat;
	public GameObject camera;
	public GameObject contactedPart;
	
	private Plane swordPlane;
	private Collider swordCollider;
	private Collider d1Collider;
	private Collider d2Collider;
	private Mesh contactedMesh;
	private Vector3[] meshVertices;
	private Vector3[] posVertices;
	private Vector3[] negVertices;
	private Vector3[] meshVerticesToWorld;
	private Bounds swordBounds;
	private Mesh negCopy;
	private GameObject negPart;
	private MeshRenderer mr;
	private MeshFilter mf;
	private Rigidbody rb;
	private MeshCollider mc;
	private FixedJoint connectedJoint;
	private float activeSwingDirection;
	
    // Start is called before the first frame update
    void Start()
    {
        swordCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        activeSwingDirection = camera.GetComponent<PlayerController>().activeSwingDirection;
    }
	
	void OnTriggerEnter(Collider other)
	{
		negCopy = new Mesh();
		contactedPart = other.gameObject;
		Debug.Log(contactedPart.name);
		contactedMesh = contactedPart.GetComponent<MeshFilter>().mesh;
		negCopy.vertices = contactedMesh.vertices;
		negCopy.triangles = contactedMesh.triangles;
		negCopy.uv = contactedMesh.uv;
		negCopy.normals = contactedMesh.normals;
		negCopy.colors = contactedMesh.colors;
		negCopy.tangents = contactedMesh.tangents;
		meshVertices = contactedMesh.vertices;
		posVertices = new Vector3[meshVertices.Length];
		negVertices = new Vector3[meshVertices.Length];
		meshVerticesToWorld = new Vector3[meshVertices.Length];
	}
	void OnTriggerExit(Collider other)
	{
		verticesLocator();
		ItemBuild();
	}
	void verticesLocator()
	{
		swordPlane = new Plane(this.transform.TransformPoint(this.transform.position),grip.transform.TransformPoint(grip.transform.position),guard.transform.TransformPoint(guard.transform.position));
		if(activeSwingDirection == 1 || activeSwingDirection == 2)
		{
			Debug.Log("Sword flipped");
			swordPlane.Flip();
		}
		for(int i = 0;i < meshVertices.Length;i++)
		{
			meshVerticesToWorld[i] = contactedPart.transform.TransformPoint(meshVertices[i]);
		}
		for(int i = 0;i < meshVerticesToWorld.Length;i++)
		{
			if(swordPlane.GetSide(meshVerticesToWorld[i]))
			{
				Debug.Log("Pos ran");
				posVertices[i] = meshVertices[i];
			} else if(!(swordPlane.GetSide(meshVerticesToWorld[i])))
			{
				Debug.Log("Neg ran");
				negVertices[i] = meshVertices[i];
			}
		}
	}
	void ItemBuild()
	{
		contactedMesh.vertices = posVertices;
		negCopy.vertices = negVertices;
		connectedJoint = contactedPart.GetComponent<FixedJoint>();
		negPart = new GameObject();
		negPart.tag = "CutPiece";
		mr = negPart.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
		mf = negPart.AddComponent(typeof(MeshFilter)) as MeshFilter;
		rb = negPart.AddComponent(typeof(Rigidbody)) as Rigidbody;
		mc = negPart.AddComponent(typeof(MeshCollider)) as MeshCollider;		
		mc.sharedMesh = negCopy;
		mc.convex = true;
		mf.mesh = negCopy;
		negPart.transform.localScale = contactedPart.transform.localScale;
		negPart.transform.position = contactedPart.transform.position;
		rb.AddForce(new Vector3(1.0f,0.0f,0.0f));
		contactedPart.GetComponent<MeshCollider>().sharedMesh = null;
		contactedPart.GetComponent<MeshCollider>().sharedMesh = contactedMesh;
		mr.material = botMat;
		contactedPart.GetComponent<MeshCollider>().isTrigger = false;
		Destroy(connectedJoint);
	}
}

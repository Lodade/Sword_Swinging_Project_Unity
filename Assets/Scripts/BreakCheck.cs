using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakCheck : MonoBehaviour
{
	private FixedJoint fj;
	private Rigidbody rb;
	private GameObject under;
	private MeshCollider mc;
	private MeshCollider currentMC;
	private bool breaking;
	private float timeLimit;
	private float timer;
	private bool switched;
    // Start is called before the first frame update
    void Start()
    {
		timeLimit = 2.0f;
		timer = timeLimit;
		breaking = false;
		switched = false;
        fj = GetComponent(typeof(FixedJoint)) as FixedJoint;
		rb = fj.connectedBody;
		under = rb.gameObject;
		mc = under.GetComponent(typeof(MeshCollider)) as MeshCollider;
		currentMC = this.GetComponent(typeof(MeshCollider)) as MeshCollider;
    }

    // Update is called once per frame
    void Update()
    {
		if(mc.isTrigger == false)
		{
			currentMC.isTrigger = false;
			breaking = true;
		}
		if(breaking == true)
		{
			if(mc.isTrigger == true)
			{
				mc.isTrigger = false;
				switched = true;
			}
			timer = timer - Time.deltaTime;
		}
		if(timer <= 0.0f)
		{
			breaking = false;
			if(switched == true)
			{
				mc.isTrigger = true;
			}
			switched = false;
			timer = timeLimit;
		}
    }
}

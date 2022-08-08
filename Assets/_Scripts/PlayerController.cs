//Shady
using UnityEngine;
using TouchControlsKit;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TCKAxisType = TouchControlsKit.EAxisType;

[HideMonoScript]
public class PlayerController : MonoBehaviour
{
    [Title("PLAYER CONTROLLER", null, titleAlignment: TitleAlignments.Centered, false, true)]
	[SerializeField] bool  CanRun         = false;
	[SerializeField] int   Penalty        = 3;
	[SerializeField] float ForwardSpeed   = 1.0f;
	[SerializeField] float MovementSpeed  = 1.0f;
	[SerializeField] float ClampX         = 4.0f;
	[SerializeField] float BreakForce     = 100f;
	//[SerializeField] Transform CamParent  = null;
	[SerializeField] List<Part> Parts     = new List<Part>();
	[SerializeField] List<Part> Breakable = new List<Part>();

	private Transform Self = null;
	private Rigidbody RB   = null;
	private Collider  CD   = null;
	private float InputX   = 0.0f;
	// private Vector3 Pos    = Vector3.zero;
	private Vector3 Forward = Vector3.zero;
	private Vector3 Side    = Vector3.zero;

	public void Run()
	{
		CanRun = true;
	}//Run() end

	void Start()
	{
		CanRun = false;
		Self   = transform;
		RB     = GetComponent<Rigidbody>();
		RB.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX;
		CD     = GetComponent<Collider>();
		foreach(MeshRenderer M in GetComponentsInChildren<MeshRenderer>())
        {
            Part part = new Part(M.transform);
            Parts.Add(part);
			part.SetBreakForce(BreakForce);
			if(part.Self.localPosition.y > 0)
			{
				Breakable.Add(part);
			}//if end
        }//loop end
		RB.isKinematic = false;
		CD.enabled     = true;
	}//Start() end

	public void SetClampX(float Value)
	{	
		ClampX = Value;
	}//SetClampX() end

	void Update()
	{
		if(CanRun)
		{
			CheckInput();
		}//if end
		// else
		// 	CamParent.rotation = Quaternion.RotateTowards(CamParent.rotation, Quaternion.Euler(0.0f, 0.0f, 0.0f), Time.deltaTime * 10f);
	}//Update() end

	void CheckInput()
	{
		if(SH_GameController.Instance.Controls.Equals(SH_GameController.Control.Mobile))
		{
			if(TCKInput.CheckController("Touchpad"))
			{
				InputX = Mathf.Clamp(TCKInput.GetAxis("Touchpad", TCKAxisType.Horizontal), -1f, 1f);
			}//if end
		}//if end	
		else
		{
			InputX = Input.GetAxis("Horizontal");
		}//else end
		//CamParent.rotation = Quaternion.RotateTowards(CamParent.rotation, Quaternion.Euler(0.0f, 0.0f, 10.0f * InputX), Time.deltaTime * 10f);
	}//CheckInput() end

	void FixedUpdate ()
	{
		if(CanRun)
		{
			//Rigidbody Movement was removed because of left right movement
			// Pos   = Self.position + new Vector3(0.0f, 0.0f, ForwardSpeed * Time.deltaTime);
			// // Pos   = Self.position + new Vector3(InputX * MovementSpeed, 0.0f, ForwardSpeed * Time.deltaTime);
			// //Pos.x = Mathf.Clamp(Pos.x, -ClampX, ClampX);
			// RB.MovePosition(Pos);

			//Forward Movement
			ForwardSpeed += Time.deltaTime * 0.5f;
			//Forward   = Vector3.zero;
			Forward.z = ForwardSpeed * Time.deltaTime;
			Self.position += Forward;
			//Sideways Movement
			Side = Self.position;
			Side.x = Side.x + InputX * MovementSpeed * Time.deltaTime;
			Side.x = Mathf.Clamp(Side.x, -ClampX, ClampX);
			Self.position = Side;
		}//if end
		
	}//FixedUpdate() end

	void OnCollisionEnter(Collision col)
    {
        if(col.transform.CompareTag("Cube") && CanRun)
        {
			col.transform.GetComponent<CubeController>().Break();
            return;
        }//if end
		else if(col.transform.CompareTag("Enemy") && CanRun)
		{
			col.transform.GetComponent<EnemyController>().Break();
			CheckPenalty();
			return;
		}//else if end
		else if(col.transform.CompareTag("EndPoint") && CanRun)
		{
			//col.transform.GetComponent<EndPoint>().Break();
			Break();
			SH_GameController.Instance.GameComplete();
			return;
		}//else if end
    }//OnCollisionEnter() end

	void CheckPenalty()
	{
		Penalty--;
		if(Penalty > 0)
		{
			BreakPart();
		}//if end
		else
		{
			Break();
			SH_GameController.Instance.GameLose();
		}//else end
	}//CheckPenalty() end

	void BreakPart()
	{
		int toBreak    = Random.Range(0, Breakable.Count);
		Part breakPart = Breakable[toBreak];
		Breakable.Remove(breakPart);
		Parts.Remove(breakPart);
		breakPart.Break();
		breakPart.Self.SetParent(null);
		Destroy(breakPart.Self.gameObject, 5.0f);
		breakPart = null;
	}//BreakPart() end

	void Break()
	{
		CanRun = false;
		RB.isKinematic = true;
        CD.enabled     = false;
        foreach(Part P in Parts)
        {
            P.Break();
        }//loop end
	}//Break() end

}//class end
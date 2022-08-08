//Shady
using UnityEngine;
using Sirenix.OdinInspector;

[HideMonoScript]
public class CameraFollow : MonoBehaviour
{
	[Title("CAMERA FOLLOW", null, titleAlignment: TitleAlignments.Centered, false, true)]
	private enum UpdateMethod{Update, FixedUpdate, LateUpdate}
	[SerializeField] UpdateMethod updateMethod;
	[SerializeField] Transform Target = null;
	[Range(1.0f, 10.0f)]
	[SerializeField] float FollowSpeed = 5.0f;
	[SerializeField] Vector3 Offset   = Vector3.zero;

	private Transform Self = null;
	private Vector3   Pos  = Vector3.zero;

	void Start()
	{
		Self = transform;
	}//Start() end

	void Update()
	{
		if(Target && updateMethod.Equals(UpdateMethod.Update))
		{
			Pos = new Vector3(0.0f, Target.position.y, Target.position.z);
			Self.position = Vector3.Lerp(Self.position, Pos + Offset, FollowSpeed * Time.deltaTime);
		}//if end
	}//Update() end
	
	void FixedUpdate()
	{
		if(Target && updateMethod.Equals(UpdateMethod.FixedUpdate))
		{
			Pos = new Vector3(0.0f, Target.position.y, Target.position.z);
			Self.position = Vector3.Lerp(Self.position, Pos + Offset, FollowSpeed * Time.fixedDeltaTime);
		}//if end
	}//FixedUpdate() end

	void LateUpdate()
	{
		if(Target && updateMethod.Equals(UpdateMethod.LateUpdate))
		{
			Pos = new Vector3(0.0f, Target.position.y, Target.position.z);
			Self.position = Vector3.Lerp(Self.position, Pos + Offset, FollowSpeed * Time.deltaTime);
		}//if end
	}//LateUpdate() end

}//class end
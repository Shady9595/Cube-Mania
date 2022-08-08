//Shady
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[HideMonoScript]
public class CubeController : MonoBehaviour
{
    [Title("CUBE CONTROLLER", null, titleAlignment: TitleAlignments.Centered, false, true)]
    [SerializeField] float BreakForce    = 100f;
    [SerializeField] List<Part> Parts    = new List<Part>();

	private Transform Self = null;
	private Collider  CD   = null;

    void Start()
	{
        this.tag = "Cube";
		Self   = transform;
		CD     = GetComponent<Collider>();
		foreach(MeshRenderer M in GetComponentsInChildren<MeshRenderer>())
        {
            Part part = new Part(M.transform);
            part.SetBreakForce(BreakForce);
            if(M != Self)
                Parts.Add(part);
        }//loop end
		CD.enabled     = true;
	}//Start() end

    public void Break()
	{
        if(!CD.enabled)
            return;
        CD.enabled     = false;
        foreach(Part P in Parts)
        {
            P.Break();
        }//loop end
        SH_GameController.Instance.CubeObtained();
	}//Break() end

}//class end
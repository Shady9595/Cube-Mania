//Shady
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[HideMonoScript]
public class EndPoint : MonoBehaviour
{
    [Title("END POINT", null, titleAlignment: TitleAlignments.Centered, false, true)]
    [SerializeField] float BreakForce    = 100f;
    [SerializeField] List<Part> Parts    = new List<Part>();

	private Transform Self = null;
	private Collider  CD   = null;

    void Start()
    {
        Self   = transform;
        this.tag = "EndPoint";
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
        CD.enabled     = false;
        foreach(Part P in Parts)
        {
            P.Break();
        }//loop end
	}//Break() end

}//class end
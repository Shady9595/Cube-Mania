//Shady
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[HideMonoScript]
public class EnemyController : MonoBehaviour
{
    [Title("ENEMY CONTROLLER", null, titleAlignment: TitleAlignments.Centered, false, true)]
    [SerializeField] bool  CanRun        = false;
    [SerializeField] float DetectionDist = 30.0f;
    [SerializeField] float BreakForce    = 100f;
    [SerializeField] Transform Player    = null;
    [SerializeField] List<Part> Parts    = new List<Part>();

	private Transform Self = null;
	private Collider  CD   = null;

    void Start()
	{
		CanRun   = true;
        this.tag = "Enemy";
		Self     = transform;
        Player   = SH_GameController.Instance.GetPlayer();
		CD       = GetComponent<Collider>();
		foreach(MeshRenderer M in GetComponentsInChildren<MeshRenderer>())
        {
            Part part = new Part(M.transform);
            part.SetBreakForce(BreakForce);
            if(M != Self)
                Parts.Add(part);
        }//loop end
		CD.enabled     = true;
	}//Start() end

    void Update()
	{
        if(CanRun)
        {
            if(Vector3.Distance(Self.position, Player.position) <= DetectionDist)
            {
                CanRun = false;
                Self.DOMoveX(Player.position.x, 0.5f, false);
                this.enabled = false;
            }//if end
        }//if end	
	}//Update() end

    public void Break()
	{
        if(!CD.enabled)
            return;
        CD.enabled = false;
        SH_GameController.Instance.EnemyImpact();
        foreach(Part P in Parts)
        {
            P.Break();
        }//loop end
	}//Break() end

}//class end
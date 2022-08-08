//Shady
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[HideMonoScript]
public class CuboidGenerator : MonoBehaviour
{
    [Title("CUBOID GENERATOR", null, titleAlignment: TitleAlignments.Centered, false, true)]

    [SerializeField] GameObject TargetCube      = null;
    [SerializeField] Material   SubCubeMaterial = null;
    [SerializeField] Vector3    SectionCount    = Vector3.zero;
    
    private GameObject SubCube            = null;
    private Vector3    SizeOfOriginalCube = Vector3.zero;
    private Vector3    SectionSize        = Vector3.zero;
    private Vector3    FillStartPosition  = Vector3.zero;
    private List<GameObject> CubesMade = new List<GameObject>();
    
    [Button("Make Cuboids")]
    void MakeCuboids()
    {
        if(!TargetCube)
        {
            Debug.LogError("Assign Target Cube !");
            return;
        }//if end
        if(!SubCubeMaterial)
        {
            if(TargetCube.GetComponent<MeshRenderer>())
            {
                SubCubeMaterial = TargetCube.GetComponent<MeshRenderer>().sharedMaterial;
            }//if end
            else
            {
                Debug.LogError("Assign SubCube Material !");
                return;
            }//else end
        }//if end
        if(SectionCount.Equals(Vector3.zero))
        {
            Debug.LogError("Assign Section Count !");
            return;
        }//if end
        
        CubesMade          = new List<GameObject>();
        SizeOfOriginalCube = TargetCube.transform.lossyScale;
        SectionSize = new Vector3(SizeOfOriginalCube.x / SectionCount.x, SizeOfOriginalCube.y / SectionCount.y, SizeOfOriginalCube.z / SectionCount.z);
        FillStartPosition = TargetCube.transform.TransformPoint(new Vector3(-0.5f, 0.5f, -0.5f)) + TargetCube.transform.TransformDirection( new Vector3(SectionSize.x,-SectionSize.y,SectionSize.z)/2.0f);
        
        MeshRenderer MR = TargetCube.GetComponent<MeshRenderer>();
        MeshFilter   MF = TargetCube.GetComponent<MeshFilter>();
        DestroyImmediate(MR);
        DestroyImmediate(MF);

        for(int i=0 ; i<SectionCount.x ; i++)
        {
            for(int j=0 ; j<SectionCount.y ; j++)
            {
                for(int k=0; k<SectionCount.z ; k++)
                { 
                    SubCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    SubCube.transform.localScale = SectionSize;
                    SubCube.transform.position   = FillStartPosition + TargetCube.transform.TransformDirection( new Vector3((SectionSize.x) * i , -(SectionSize.y) * j,(SectionSize.z) * k));
                    SubCube.transform.rotation   = TargetCube.transform.rotation;
                    SubCube.GetComponent<MeshRenderer>().material = SubCubeMaterial;
                    SubCube.GetComponent<Collider>().enabled = false;
                    SubCube.AddComponent<Rigidbody>();
                    SubCube.GetComponent<Rigidbody>().isKinematic = true;
                    SubCube.transform.SetParent(TargetCube.transform);
                    CubesMade.Add(SubCube);
                }//loop end
            }//loop end
        }//loop end

        //This is done to make the scale of parent one so it does not effect the child scale when rigidbody is activated
        GameObject Default = new GameObject(TargetCube.name);
        Default.transform.SetParent(TargetCube.transform);
        Default.transform.localPosition = Vector3.zero;
        Default.transform.localRotation = Quaternion.identity;
        Default.transform.localScale    = Vector3.one;
        Default.transform.SetParent(null);
        Default.transform.position = TargetCube.transform.position;
        Default.transform.localScale    = Vector3.one;
        Default.AddComponent<BoxCollider>();
        Default.GetComponent<BoxCollider>().size = TargetCube.transform.localScale;
        foreach(GameObject Cube in CubesMade)
        {
            Cube.transform.SetParent(Default.transform);
        }//loop end
        DestroyImmediate(TargetCube);
        Debug.Log(CubesMade.Count + " Cubes Generated");
        CubesMade = null;
    }//MakeCuboids() end

    [Button("Reset")]
    void Reset()
    {
        TargetCube      = null;
        SubCubeMaterial = null;
        SectionCount    = Vector3.zero;
    }//Reset() end

}//class end
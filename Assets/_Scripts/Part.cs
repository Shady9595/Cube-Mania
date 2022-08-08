//Shady
using UnityEngine;

[System.Serializable]
public class Part
{
    public Transform Self;
    [HideInInspector]
    public Rigidbody  RB;
    [HideInInspector]
    public Collider   CD;
    [HideInInspector]
    public Vector3    StartPos;
    [HideInInspector]
    public Quaternion StartRot;

    private float BreakForce = 100f;
    private Transform Parent;

    public Part(Transform T)
    {
        Self           = T;
        Parent         = Self.parent;
        RB             = T.GetComponent<Rigidbody>();
        RB.isKinematic = true;
        CD             = T.GetComponent<Collider>();
        CD.enabled     = false;
        StartPos       = T.localPosition;
        StartRot       = T.rotation;
    }//Constructor() end

    public void SetBreakForce(float breakForce)
    {
        BreakForce = breakForce;
    }//SetBreakForce() end

    public void Break()
    {
        if(!Self)
            return;
        // Self.SetParent(null);
        RB.isKinematic = false;
        CD.enabled     = true;
        // Vector3 Force = new Vector3(Random.Range(50f, BreakForce), Random.Range(50f, BreakForce), Random.Range(50f, BreakForce));
        // RB.AddForce(Force, ForceMode.Impulse);
        RB.AddExplosionForce(BreakForce, Self.position, 100.0f, 0.01f, ForceMode.Impulse);
        Vector3 torque = new Vector3(Random.Range(50.0f, 100.0f), Random.Range(50.0f, 100.0f), Random.Range(50.0f, 100.0f));
        RB.AddRelativeTorque(torque, ForceMode.Impulse);
    }//Break() end

    public void Reset()
    {
        if(!Self)
            return;
        Self.SetParent(Parent);
        RB.isKinematic     = true;
        CD.enabled         = false;
        Self.localPosition = StartPos;
        Self.rotation      = StartRot;
    }//Reset() end
}//class end
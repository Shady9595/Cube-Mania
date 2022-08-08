//Shady
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }//Start() end

    private void OnTriggerEnter(Collider other)
    {
        if(other)
        {
            other.gameObject.SetActive(false);
        }//if end 
    }////OnTriggerEnter() end
}//class end
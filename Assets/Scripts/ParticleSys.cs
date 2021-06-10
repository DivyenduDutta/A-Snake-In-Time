using UnityEngine;

public class ParticleSys : MonoBehaviour
{
    void Start()
    {
        //destroy particles after some time
        Destroy(this.gameObject, 8f);
    }

}

using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class GlobalLight : MonoBehaviour
{

    private float dimLightTimer;
    private float dimLightTimerMax;

    private bool isLightDim;
    private void Awake()
    {
        dimLightTimer = 0.0f;
        dimLightTimerMax = 10.0f;
        isLightDim = false;
        InvokeRepeating("DimLight", 30, 90);
    }

    private void FixedUpdate()
    {
        if (isLightDim) {
            if (dimLightTimer < dimLightTimerMax)
            {
                dimLightTimer += (Time.deltaTime / Time.timeScale);
            }
            else
            {
                dimLightTimer = 0.0f;
                this.gameObject.GetComponent<Light2D>().intensity = 0.2f;
                isLightDim = false;
            }
        }
        
    }

    private void DimLight()
    {
        if (!isLightDim)
        {
            //dims light
            this.gameObject.GetComponent<Light2D>().intensity = 0.0f;
            isLightDim = true;
        }
    }
}

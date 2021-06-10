using UnityEngine;

public class Instructions : MonoBehaviour
{
    public static Instructions instance;

    public static Instructions GetInstance()
    {
        return instance;
    }

    public GameObject timeSlowInstr;
    public GameObject movementSlowInstr;

    private void Awake()
    {
        instance = this;    
    }

    public void ShowTimeSlowInstruction()
    {
        timeSlowInstr.SetActive(true);
    }
    public void HideTimeSlowInstruction()
    {
        timeSlowInstr.SetActive(false);
    }
    public void ShowMovementInstruction()
    {
        movementSlowInstr.SetActive(true);
    }
    public void HideMovementInstruction()
    {
        movementSlowInstr.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlowSlots : MonoBehaviour
{
    public Transform timeSlowSlotUI; //maybe change to a different sprite later?
    private const int MAX_TIMESLOW = 3;

    private List<Transform> timeSlowUIList;

    private void Awake(){
        timeSlowUIList = new List<Transform>();
        float initialX = -19.0f;
        float initialY = 11.0f;
        for (int i = 0; i < MAX_TIMESLOW; ++i){
            Transform timeSlowUI = Instantiate(timeSlowSlotUI, new Vector2(initialX, initialY), Quaternion.identity);
            timeSlowUI.gameObject.SetActive(false); //initally all hidden
            timeSlowUIList.Add(timeSlowUI);
            initialX += 1;
        }
    }

    private void Update(){
        for (int i=0; i<MAX_TIMESLOW; ++i){
            if (i < Snake.GetInstance().numberOfTimeSlowAvailable){
                timeSlowUIList[i].gameObject.SetActive(true);
            }
            else{
                timeSlowUIList[i].gameObject.SetActive(false);
            }
            
        }
    }
}

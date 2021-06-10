using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlowSpawnItem : Spawnable
{
    private int x;
    private int y;
    private Transform position;

    public TimeSlowSpawnItem()
    {
        x = (int)Random.Range((int)GameObject.FindGameObjectWithTag("WallLeft").transform.position.x+1,
                                        (int)GameObject.FindGameObjectWithTag("WallRight").transform.position.x);
        y = (int)Random.Range((int)GameObject.FindGameObjectWithTag("WallBottom").transform.position.y+1,
                                    (int)GameObject.FindGameObjectWithTag("WallTop").transform.position.y);
    }

    public void SpawnItem()
    {
        Vector2 timeSlowPoint = new Vector2(x, y);
        position = GameObject.Instantiate(GameAssets.GetInstance().timeSlow, timeSlowPoint, Quaternion.identity);
    }

    public void DeSpawnItem()
    {
        Animator pointAnimator = position.GetComponent<Animator>();
        pointAnimator.SetBool("Despawn", true);
    }

    public int GetPositionX()
    {
        return x;
    }
    /*public void SetPositionX(int x){
        this.x = x;
    }*/

    public int GetPositionY()
    {
        return y;
    }
    /*public void SetPositionY(int y){
        this.y = y;
    }*/

    public Transform GetPosition()
    {
        return position;
    }
    /*public void SetPosition(Transform position){
        this.position = position;
    }*/
}

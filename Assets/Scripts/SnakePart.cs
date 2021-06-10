using System;
using UnityEngine;
using CustomEvent;

public class SnakePart : MonoBehaviour
{

    public static event EventHandler OnGetPoint1;
    public static event EventHandler OnGetPoint2;
    public static event EventHandler OnGetTimeSlow;
    public static event EventHandler OnDeathCollide;

    private void OnTriggerEnter2D(Collider2D collider) {
        /*if (collider.gameObject.name.Equals("SnakeHead")) {
            //Debug.Log("Collided with myself");
            //OnDeathCollide?.Invoke(this, System.EventArgs.Empty);
        }*/
        if (collider.gameObject.name.StartsWith("Wall")){
            //Debug.Log("Collided with wall");
            OnDeathCollide?.Invoke(this, System.EventArgs.Empty);
        }
        if (this.gameObject.name.Equals("SnakeHead")) {
            if (collider.gameObject.name.Contains("Point1")){
                ObtainPointsEvent e = new ObtainPointsEvent();
                e.PointX = collider.gameObject.transform.position.x;
                e.PointY = collider.gameObject.transform.position.y;
                //collider.gameObject.GetComponent<Animator>().enabled = false;
                OnGetPoint1?.Invoke(this, e);
                //Debug.Log("Collided with point1");
            }
            if (collider.gameObject.name.Contains("Point2")){
                ObtainPointsEvent e = new ObtainPointsEvent();
                e.PointX = collider.gameObject.transform.position.x;
                e.PointY = collider.gameObject.transform.position.y;
                //collider.gameObject.GetComponent<Animator>().enabled = false;
                OnGetPoint2?.Invoke(this, e);
                //Debug.Log("Collided with point2");
            }
            if (collider.gameObject.name.Contains("Timeslow")) {
                ObtainPointsEvent e = new ObtainPointsEvent();
                e.PointX = collider.gameObject.transform.position.x;
                e.PointY = collider.gameObject.transform.position.y;
                //collider.gameObject.GetComponent<Animator>().enabled = false;
                OnGetTimeSlow?.Invoke(this, e);
                //Debug.Log("Collided with point1");
            }
        }
    }
}

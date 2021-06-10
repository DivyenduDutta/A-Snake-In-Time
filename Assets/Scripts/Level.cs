using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEvent;

public class Level : MonoBehaviour
{
    private const int MAX_NUMBER_OF_POINT1 = 8;
    //private List<Transform> point1List;
    private List<Spawnable> point1List;

    private const int MAX_NUMBER_OF_POINT2 = 5;
    //private List<Transform> point2List;
    private List<Spawnable> point2List;

    private const int MAX_TIMESLOW = 3;
    //private List<Transform> timeSlowList;
    private List<Spawnable> timeSlowList;

    public static int score;
    public static int scoreCopy;
    public static int finalScore;

    private const int POINT1_VALUE = 2;
    private const int POINT2_VALUE = 4;

    private void Awake(){
        point1List = new List<Spawnable>();
        point2List = new List<Spawnable>();
        timeSlowList = new List<Spawnable>();
        

        score = 0;
        scoreCopy = 0;
        finalScore = 0;

        InvokeRepeating("SpawnPoint1", 2, 4); //every 4 seconds
        InvokeRepeating("SpawnPoint2", 6, 8); //every 8 seconds

        InvokeRepeating("DeSpawnPoint1", 10, 8); //every 12 seconds
        InvokeRepeating("DeSpawnPoint2", 22, 16); //every 20 seconds

        InvokeRepeating("SpawnTimeSlow", 0, 60);
        InvokeRepeating("DeSpawnTimeSlow", 70, 60);
    }

    private void Start(){
        SnakePart.OnGetPoint1 += SnakePart_OnGetPoint1;
        SnakePart.OnGetPoint2 += SnakePart_OnGetPoint2;
        SnakePart.OnGetTimeSlow += SnakePart_OnGetTimeSlow;
    }

    private void OnDestroy(){
        SnakePart.OnGetPoint1 -= SnakePart_OnGetPoint1;
        SnakePart.OnGetPoint2 -= SnakePart_OnGetPoint2;
        SnakePart.OnGetTimeSlow -= SnakePart_OnGetTimeSlow;
    }

    /*private void SpawnPoint1(){
        if (point1List.Count < MAX_NUMBER_OF_POINT1) {
            int x = (int)Random.Range((int)GameObject.FindGameObjectWithTag("WallLeft").transform.position.x,
                                        (int)GameObject.FindGameObjectWithTag("WallRight").transform.position.x);
            int y = (int)Random.Range((int)GameObject.FindGameObjectWithTag("WallBottom").transform.position.y,
                                        (int)GameObject.FindGameObjectWithTag("WallTop").transform.position.y);
            if (IsPointSpawnPositionOk(x, y) && IsTimeSlowSpawnPositionOk(x, y) && Snake.GetInstance().IsPointOnSnakeBody(x, y)) {
                Vector2 point1Pos = new Vector2(x, y);
                Transform point1 = Instantiate(GameAssets.GetInstance().point1, point1Pos, Quaternion.identity);
                point1List.Add(point1);
            }
        }
    }*/

    private void SpawnPoint1(){
        if (point1List.Count < MAX_NUMBER_OF_POINT1){
            Spawnable item = new PointOneSpawnItem();
            if (IsItemSpawnPositionOk(item.GetPositionX(), item.GetPositionY()) 
                && Snake.GetInstance().IsPointOnSnakeBody(item.GetPositionX(), item.GetPositionY()))
            {
                item.SpawnItem();
                //item.GetPosition().gameObject.GetComponentInChildren<ParticleSystem>().Play();
                point1List.Add(item);
            }
        }
    }


    /*private void SpawnPoint2(){
        if (point2List.Count < MAX_NUMBER_OF_POINT2){
            int x = (int)Random.Range((int)GameObject.FindGameObjectWithTag("WallLeft").transform.position.x,
                                    (int)GameObject.FindGameObjectWithTag("WallRight").transform.position.x);
            int y = (int)Random.Range((int)GameObject.FindGameObjectWithTag("WallBottom").transform.position.y,
                                        (int)GameObject.FindGameObjectWithTag("WallTop").transform.position.y);
            if (IsPointSpawnPositionOk(x, y) && IsTimeSlowSpawnPositionOk(x, y) && Snake.GetInstance().IsPointOnSnakeBody(x, y)){
                Vector2 point2Pos = new Vector2(x, y);
                Transform point2 = Instantiate(GameAssets.GetInstance().point2, point2Pos, Quaternion.identity);
                point2List.Add(point2);
            }
        }
    }*/

    private void SpawnPoint2(){
        if (point2List.Count < MAX_NUMBER_OF_POINT2){
            Spawnable item = new PointTwoSpawnItem();
            if (IsItemSpawnPositionOk(item.GetPositionX(), item.GetPositionY())
                && Snake.GetInstance().IsPointOnSnakeBody(item.GetPositionX(), item.GetPositionY())){
                item.SpawnItem();
                point2List.Add(item);
            }
        }
    }

    /* private void SpawnTimeSlow(){
         if (timeSlowList.Count < MAX_TIMESLOW 
                 && Snake.GetInstance().numberOfTimeSlowAvailable < MAX_TIMESLOW){
             int x = (int)Random.Range((int)GameObject.FindGameObjectWithTag("WallLeft").transform.position.x,
                                         (int)GameObject.FindGameObjectWithTag("WallRight").transform.position.x);
             int y = (int)Random.Range((int)GameObject.FindGameObjectWithTag("WallBottom").transform.position.y,
                                         (int)GameObject.FindGameObjectWithTag("WallTop").transform.position.y);
             if (IsPointSpawnPositionOk(x, y) && IsTimeSlowSpawnPositionOk(x, y) && Snake.GetInstance().IsPointOnSnakeBody(x, y)){
                 Transform timeSlow = Instantiate(GameAssets.GetInstance().timeSlow, new Vector2(x, y), Quaternion.identity);
                 timeSlowList.Add(timeSlow);
             }
         }
     }*/

    private void SpawnTimeSlow(){
        if (timeSlowList.Count < MAX_TIMESLOW
                && Snake.GetInstance().numberOfTimeSlowAvailable < MAX_TIMESLOW){
            Spawnable item = new TimeSlowSpawnItem();
            if (IsItemSpawnPositionOk(item.GetPositionX(), item.GetPositionY())
                 && Snake.GetInstance().IsPointOnSnakeBody(item.GetPositionX(), item.GetPositionY())){
                item.SpawnItem();
                timeSlowList.Add(item);
            }
        }
    }

    private void DeSpawnPoint1() {
        if (point1List.Count > 1) {
            Spawnable pointToDespawn = point1List[0];
            pointToDespawn.DeSpawnItem();
            point1List.RemoveAt(0);
        }
    }

    private void DeSpawnPoint2(){
        if (point2List.Count > 3){
            Spawnable pointToDespawn = point2List[0];
            pointToDespawn.DeSpawnItem();
            point2List.RemoveAt(0);
        }
    }

    private void DeSpawnTimeSlow(){
        if (timeSlowList.Count > 0){
            Spawnable timeSlowToDespawn = timeSlowList[0];
            timeSlowToDespawn.DeSpawnItem();
            timeSlowList.RemoveAt(0);
        }
    }

    private void SnakePart_OnGetPoint1(object sender, System.EventArgs e) {
        ObtainPointsEvent point1Arg = (ObtainPointsEvent)e;

        //add to score
        score += POINT1_VALUE;
        scoreCopy = score;
        finalScore++;

        //remove coresponding point1 from the list
        for (int i=0; i<point1List.Count; ++i){
            Transform point1 = point1List[i].GetPosition();
            if (Vector2.Distance(new Vector2(point1Arg.PointX, point1Arg.PointY), 
                                 new Vector2(point1.position.x, point1.position.y)) < 0.001f){
                Instantiate(GameAssets.GetInstance().pointDestroyParticleSys,
                            point1.transform.position, Quaternion.identity);
                Instantiate(GameAssets.GetInstance().plusTwoParticleSys,
                            point1.transform.position, Quaternion.identity);
                AudioHandler.PlayAudio(AudioHandler.Sound.PointOne, false);
                point1List.RemoveAt(i);
                Destroy(point1.gameObject);
                break;
            }
        }
    }

    private void SnakePart_OnGetPoint2(object sender, System.EventArgs e){
        ObtainPointsEvent point2Arg = (ObtainPointsEvent)e;

        //add to score
        score += POINT2_VALUE;
        scoreCopy = score;
        finalScore++;

        //remove coresponding point2 from the list
        for (int i = 0; i < point2List.Count; ++i) {
            Transform point2 = point2List[i].GetPosition();
            if (Vector2.Distance(new Vector2(point2Arg.PointX, point2Arg.PointY),
                                 new Vector2(point2.position.x, point2.position.y)) < 0.001f){
                Instantiate(GameAssets.GetInstance().pointDestroyParticleSys,
                            point2.transform.position, Quaternion.identity);
                Instantiate(GameAssets.GetInstance().plusFourParticleSys,
                            point2.transform.position, Quaternion.identity);
                AudioHandler.PlayAudio(AudioHandler.Sound.PointTwo, false);
                point2List.RemoveAt(i);
                Destroy(point2.gameObject);
                break;
            }
        }
    }

    private void SnakePart_OnGetTimeSlow(object sender, System.EventArgs e){
        ObtainPointsEvent timeSlowArg = (ObtainPointsEvent)e;

        //remove coresponding timeslow from the list
        for (int i = 0; i < timeSlowList.Count; ++i){
            Transform timeSlow = timeSlowList[i].GetPosition();
            if (Vector2.Distance(new Vector2(timeSlowArg.PointX, timeSlowArg.PointY),
                                 new Vector2(timeSlow.position.x, timeSlow.position.y)) < 0.001f){
                Instantiate(GameAssets.GetInstance().pointDestroyParticleSys,
                            timeSlow.transform.position, Quaternion.identity);
                Instantiate(GameAssets.GetInstance().timeSlowParticleSys,
                            timeSlow.transform.position, Quaternion.identity);
                AudioHandler.PlayAudio(AudioHandler.Sound.TimeSlowPoint, false);
                timeSlowList.RemoveAt(i);
                Destroy(timeSlow.gameObject);
                break;
            }
        }
    }

    private bool IsItemSpawnPositionOk(int x, int y){
        for (int i=0; i<point1List.Count; ++i){
            if (Vector2.Distance(point1List[i].GetPosition().position, new Vector2(x, y)) < 0.01f){
                //Debug.Log("Not spawning point cause its too close to another point1");
                return false;
            }
        }

        for (int i = 0; i < point2List.Count; ++i){
            if (Vector2.Distance(point2List[i].GetPosition().position, new Vector2(x, y)) < 0.01f){
                //Debug.Log("Not spawning point cause its too close to another point2");
                return false;
            }
        }

        for (int i = 0; i < timeSlowList.Count; ++i){
            if (Vector2.Distance(timeSlowList[i].GetPosition().position, new Vector2(x, y)) < 0.01f){
                //Debug.Log("Not spawning point cause its too close to another point2");
                return false;
            }
        }

        return true;
    }
}

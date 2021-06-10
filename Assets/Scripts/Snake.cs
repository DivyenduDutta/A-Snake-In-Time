using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class Snake : MonoBehaviour{

    private int SNAKE_PARTS = 8;
    //private const float SNAKE_SPEED = 3f;
    private const float CHANGE_DIR_PRECISION = 0.01f;
    private float MIN_SCORE_NEEDED_TO_REWIND = 2.0f;
    private const float TIMESCALE_AT_REWIND = 0.8f;
    private const float TIMESCALE_INCREMENT = 0.15f;

    private float currentTimescale;
    private float currentTimescaleBeforeRewind;

    private List<ASnakePart> snakeBody;

    private List<DirectionChangePoint> directionChangePoints;

    public Animator gameOverAnimator;

    public int numberOfTimeSlowAvailable;

    private bool isRewind;
    private bool isSlow;

    public bool gameOver = false;

    public GameObject rewindWindow;
    public Slider rewindProgress;
    public GameObject timeSlowImage;


    private enum MOVE_DIR{
        UP,
        DOWN,
        LEFT,
        RIGHT,
        NONE
    }

    private Dictionary<MOVE_DIR, Vector3> movementVectors;

    public static Snake instance;

    public static Snake GetInstance(){
        return instance;
    }

    private float snakeIncreaseSpeedTimer;
    private float snakeIncreaseSpeedTimerMax = 15.0f; //increase speed after every 15 secs

    private float slowSnakeTimer;
    private float slowSnakeTimerMax = 5.0f; //snake slows for 5 secs

    private float slowTimeInstrTimer;
    private float slowTimeInstrTimerMax = 2.0f; //display instructions for 2 secs
    private bool isFirstTimeSlow;
    private bool showTimeSLowInstruction;

    private void Awake(){
        instance = this;

        //initialize the movement dictionary
        movementVectors = new Dictionary<MOVE_DIR, Vector3>();
        movementVectors.Add(MOVE_DIR.UP, Vector3.up);
        movementVectors.Add(MOVE_DIR.DOWN, Vector3.down);
        movementVectors.Add(MOVE_DIR.LEFT, Vector3.left);
        movementVectors.Add(MOVE_DIR.RIGHT, Vector3.right);

        directionChangePoints = new List<DirectionChangePoint>();

        isRewind = false;
        isSlow = false;

        numberOfTimeSlowAvailable = 0;

        isFirstTimeSlow = false;
        showTimeSLowInstruction = true;

        HandleInitialSnakeSpawn();
    }

    private void Start(){
        SnakePart.OnGetPoint1 += SnakePart_OnGetPoint1;
        SnakePart.OnGetPoint2 += SnakePart_OnGetPoint2;
        SnakePart.OnDeathCollide += SnakePart_OnDeathCollide;
        SnakePart.OnGetTimeSlow += SnakePart_OnGetTimeSlow;
    }

    void OnDestroy(){
        SnakePart.OnGetPoint1 -= SnakePart_OnGetPoint1;
        SnakePart.OnGetPoint2 -= SnakePart_OnGetPoint2;
        SnakePart.OnDeathCollide -= SnakePart_OnDeathCollide;
        SnakePart.OnGetTimeSlow -= SnakePart_OnGetTimeSlow;
    }

    /*private void Start(){
        InvokeRepeating("Move", 2f, 0.25f);
    }*/

    private void Update(){
        /*if (!isRewind && Input.GetKeyDown(KeyCode.T)){
            RewindSnakeDirection();
            isRewind = true;
        }
        if (isRewind && Input.GetKeyDown(KeyCode.Q)){
            RewindSnakeDirection();
            isRewind = false;
        }*/
        if (!isSlow && !gameOver && !isRewind && numberOfTimeSlowAvailable>0 && Input.GetKeyDown(KeyCode.LeftShift)){
            currentTimescale = Time.timeScale;
            Time.timeScale = TIMESCALE_AT_REWIND;
            isSlow = true;
            timeSlowImage.SetActive(isSlow);
            slowSnakeTimer = 0.0f;
            numberOfTimeSlowAvailable--;
        }
        if (isRewind && Input.GetKeyDown(KeyCode.Space)){
            //spacebar to stop rewinding
            HandleStopRewind();
        }

        //take player movement input
        if (!isRewind && !gameOver){
            if ((snakeBody[0].GetDirection() != MOVE_DIR.LEFT && snakeBody[0].GetDirection() != MOVE_DIR.RIGHT) 
                        && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))){
                directionChangePoints.Insert(0, new DirectionChangePoint(snakeBody[0].GetSnakePartTransform().position,
                                                MOVE_DIR.LEFT, GetOppositeDirection(snakeBody[0].GetDirection())));
                //snakeBody[0].GetSnakePartTransform().position += Vector3.left * SNAKE_SPEED ;
                //snakeBody[0].SetDirection(MOVE_DIR.LEFT);
            }
            if ((snakeBody[0].GetDirection() != MOVE_DIR.RIGHT && snakeBody[0].GetDirection() != MOVE_DIR.LEFT) 
                        && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))){
                directionChangePoints.Insert(0, new DirectionChangePoint(snakeBody[0].GetSnakePartTransform().position,
                                                MOVE_DIR.RIGHT, GetOppositeDirection(snakeBody[0].GetDirection())));
                //snakeBody[0].GetSnakePartTransform().position += Vector3.right * SNAKE_SPEED ;
                //snakeBody[0].SetDirection(MOVE_DIR.RIGHT);
            }
            if ((snakeBody[0].GetDirection() != MOVE_DIR.UP && snakeBody[0].GetDirection() != MOVE_DIR.DOWN) 
                        && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))){
                directionChangePoints.Insert(0, new DirectionChangePoint(snakeBody[0].GetSnakePartTransform().position,
                                                MOVE_DIR.UP, GetOppositeDirection(snakeBody[0].GetDirection())));
                //snakeBody[0].GetSnakePartTransform().position += Vector3.up * SNAKE_SPEED ;
                //snakeBody[0].SetDirection(MOVE_DIR.UP);
            }
            if ((snakeBody[0].GetDirection() != MOVE_DIR.DOWN && snakeBody[0].GetDirection() != MOVE_DIR.UP) 
                        && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))){
                directionChangePoints.Insert(0, new DirectionChangePoint(snakeBody[0].GetSnakePartTransform().position,
                                                MOVE_DIR.DOWN, GetOppositeDirection(snakeBody[0].GetDirection())));
                //snakeBody[0].GetSnakePartTransform().position += Vector3.down * SNAKE_SPEED ;
                //snakeBody[0].SetDirection(MOVE_DIR.DOWN);
            }

            //HandleMovement();
        }

        
    }

    private void FixedUpdate() {
        //Debug.Log(gameOver);
        //Debug.Log(Time.timeScale);
        //Debug.Log(MIN_SCORE_NEEDED_TO_REWIND);
        if (!gameOver) {
            Move();
            if (isFirstTimeSlow && showTimeSLowInstruction)
            {
                DisableSlowSpeedInstruction();
            }
            
            if (!isSlow) {
                IncreaseSnakeSpeed();
            }
            else{
                HandleSlowSnakeSpeed();
            }
            
        }
    }

    private void DisableSlowSpeedInstruction()
    {
        slowTimeInstrTimer += Time.unscaledDeltaTime;
        if (slowTimeInstrTimer > slowTimeInstrTimerMax)
        {
            slowTimeInstrTimer -= slowTimeInstrTimerMax;
            Instructions.GetInstance().HideTimeSlowInstruction();
            showTimeSLowInstruction = false;
        }
    }

    private void IncreaseSnakeSpeed(){
        snakeIncreaseSpeedTimer -= Time.deltaTime;
        if (snakeIncreaseSpeedTimer < 0){
            snakeIncreaseSpeedTimer += snakeIncreaseSpeedTimerMax;
            Time.timeScale += TIMESCALE_INCREMENT;
        }
    }
    private void HandleSlowSnakeSpeed() {
        if (slowSnakeTimer <= slowSnakeTimerMax){
            slowSnakeTimer += Time.deltaTime;
        }
        else{
            isSlow = false;
            timeSlowImage.SetActive(isSlow);
            Time.timeScale = currentTimescale;
        }
    }

    private void HandleStopRewind() {
        Time.timeScale = Mathf.Max(1.0f, currentTimescaleBeforeRewind-0.4f); //gives the player some time to breathe
        currentTimescaleBeforeRewind = 0.0f;
        rewindProgress.value = 0;
        rewindWindow.SetActive(false);
        RewindSnakeDirection();
        isRewind = false;
        Level.scoreCopy = Level.score;
    }

    private MOVE_DIR GetOppositeDirection(MOVE_DIR direction){
        MOVE_DIR oppositeDirection = MOVE_DIR.NONE;
        switch (direction){
            case MOVE_DIR.UP: oppositeDirection = MOVE_DIR.DOWN;
                break;
            case MOVE_DIR.DOWN: oppositeDirection = MOVE_DIR.UP;
                break;
            case MOVE_DIR.LEFT: oppositeDirection = MOVE_DIR.RIGHT;
                break;
            case MOVE_DIR.RIGHT: oppositeDirection = MOVE_DIR.LEFT;
                break;
        }
        return oppositeDirection;
    }

    private void HandleInitialSnakeSpawn(){
        snakeBody = new List<ASnakePart>();

        Vector3 initialSpawnPosition = new Vector3(0, 0, 0); //initial spawn position of the snake --> randomize later
        for (int i = 0; i < SNAKE_PARTS; ++i){ 
            Transform aSnakePart = Instantiate(GameAssets.GetInstance().snakeSprite,
                                        initialSpawnPosition, Quaternion.identity);
            //ASnakePart.parent = this.gameObject.transform;

            //snake moving up by default --> randomize later
            snakeBody.Add(new ASnakePart(aSnakePart, (i==0) ? true : false, MOVE_DIR.UP));
            if(i == 0){
                //name the head for id purposes
                aSnakePart.gameObject.name = "SnakeHead";

                //add the light to snake head
                Transform snakeHeadLight = Instantiate(GameAssets.GetInstance().snakeHeadLight, 
                                            initialSpawnPosition,
                                            Quaternion.identity);
                snakeHeadLight.parent = aSnakePart.transform;
            }
            initialSpawnPosition.y -= 1f;                     //add other snake parts below
            
        }
    }

    private void Move(){
        if (!isRewind){
            HandleMovement();
        }
        else{
            HandleRewindMovement();
        }
    }

    private void HandleMovement(){
        for (int i = 0; i < SNAKE_PARTS; ++i){
            foreach (DirectionChangePoint changePoint in directionChangePoints) {
                if (changePoint.isActive()
                    && Vector3.Distance(snakeBody[i].GetSnakePartTransform().position, 
                                        changePoint.GetDirectionChangeCoords()) < CHANGE_DIR_PRECISION){
                    //this is a corner point
                    //change that snake parts direction
                    snakeBody[i].SetDirection(changePoint.GetDirectionTurned());
                    //snakeBody[i].SetSnakePartTransform(changePoint.GetDirectionChangeCoords());
                    if (i == SNAKE_PARTS - 1){
                        //Debug.Log("Deactivating ");
                        //Debug.Log(changePoint.GetDirectionChangeCoords());
                        changePoint.SetActive(false);
                    }
                    break;
                }
                
            }
            /*snakeBody[i].GetSnakePartTransform().position +=
                movementVectors[snakeBody[i].GetDirection()] * SNAKE_SPEED * Time.deltaTime;*/
            if (!DetectSnakeHeadCollisionWithItself(i)){
                //Above method used in condition is jank code because the snake head moves by 1 unit full so by the time collision
                //detection occurs in unity, the snake head is already in the snake body --> that causes issues
                //the above condition fixes that but I aint proud of it :(
                snakeBody[i].GetSnakePartTransform().Translate(movementVectors[snakeBody[i].GetDirection()]);
            }
            else{
                break;
            }
        }


    }

    private bool DetectSnakeHeadCollisionWithItself(int part){
        //WARNING: Not good way to solve the snake head collison with snake body problem
        Vector3 snakeHeadCurPos = new Vector3(snakeBody[0].GetSnakePartTransform().position.x,
                                               snakeBody[0].GetSnakePartTransform().position.y,
                                               snakeBody[0].GetSnakePartTransform().position.z);
        Vector3 snakeHeadNextPos = snakeHeadCurPos + movementVectors[snakeBody[0].GetDirection()];
        if (part == 0){
            for (int i = 1; i < SNAKE_PARTS; ++i)
            {
                if (snakeBody[i].GetSnakePartTransform().position == snakeHeadNextPos)
                {
                    //snake do be colliding with itself next
                    Rewind();
                    return true;
                }
            }
        }
        return false;
    }

    private void HandleRewindMovement(){
        for (int i = 0; i < SNAKE_PARTS-1; ++i){ //every part other than last
            DirectionChangePoint dcpToRemove = null;
            foreach (DirectionChangePoint changePoint in directionChangePoints){
                /*bool toConsiderDirChange = false;
                if((i != (SNAKE_PARTS - 1) && changePoint.isActive()))
                {
                    toConsiderDirChange = true;
                }else if (i == (SNAKE_PARTS - 1))
                {
                    toConsiderDirChange = true;
                }*/
                if (changePoint.isActive() && Vector3.Distance(snakeBody[i].GetSnakePartTransform().position, 
                                        changePoint.GetDirectionChangeCoords()) < CHANGE_DIR_PRECISION){
                    //this is a corner point
                    //change that snake parts direction
                    snakeBody[i].SetDirection(changePoint.GetRewindDirection());
                    //snakeBody[i].SetSnakePartTransform(changePoint.GetDirectionChangeCoords());
                    if (i == 0){
                        //Debug.Log("Removing ");
                        //Debug.Log(changePoint.GetDirectionChangeCoords());
                        dcpToRemove = changePoint;
                    }
                    /*if ((i == SNAKE_PARTS-1) && !changePoint.isActive()){
                        //Debug.Log("Activating");
                        //Debug.Log(changePoint.GetDirectionChangeCoords());
                        changePoint.SetActive(true);
                    }*/
                    break;
                }
                
            }

            //removing dcp not needed (if any)
            if(dcpToRemove != null){
                //Debug.Log("Removed");
                directionChangePoints.RemoveAll(item => item == dcpToRemove);
            }
            /*snakeBody[i].GetSnakePartTransform().position +=
                movementVectors[snakeBody[i].GetDirection()] * SNAKE_SPEED * Time.deltaTime ;*/
            snakeBody[i].GetSnakePartTransform().Translate(movementVectors[snakeBody[i].GetDirection()]);
        }

        //for last part ---> i == SNAKE_PARTS-1
        DirectionChangePoint dcpForLastSnakePart = GetLatestNonActiveDCP();
        if (dcpForLastSnakePart != null 
           && Vector3.Distance(snakeBody[SNAKE_PARTS-1].GetSnakePartTransform().position,
                                        dcpForLastSnakePart.GetDirectionChangeCoords()) < CHANGE_DIR_PRECISION) {
            snakeBody[SNAKE_PARTS-1].SetDirection(dcpForLastSnakePart.GetRewindDirection()); 
            dcpForLastSnakePart.SetActive(true);
        }
        snakeBody[SNAKE_PARTS - 1].GetSnakePartTransform().Translate(movementVectors[snakeBody[SNAKE_PARTS - 1].GetDirection()]);

        //handle the rewind bar
        Level.score -= (int)MIN_SCORE_NEEDED_TO_REWIND;
        rewindProgress.value = ((Level.scoreCopy/MIN_SCORE_NEEDED_TO_REWIND) - (Level.score/MIN_SCORE_NEEDED_TO_REWIND)) / (Level.scoreCopy/MIN_SCORE_NEEDED_TO_REWIND);
        if (Level.score <= 0){
            GameOverHandler();
        }
    }

    private DirectionChangePoint GetLatestNonActiveDCP(){
        foreach (DirectionChangePoint dcp in directionChangePoints){
            if (!dcp.isActive()){
                return dcp;
            }
        }
        return null;
    }

    private void RewindSnakeDirection(){
        for (int i = 0; i < SNAKE_PARTS; ++i){
            snakeBody[i].SetDirection(GetOppositeDirection(snakeBody[i].GetDirection()));
        }
    }

    private void OnGetPoint(){
        ASnakePart lastSnakePart = snakeBody[snakeBody.Count - 1];
        Vector3 pointSnakePartPosition = new Vector3();
        pointSnakePartPosition.z = lastSnakePart.GetSnakePartTransform().position.z; //z doesnt matter
        switch (lastSnakePart.GetDirection())
        {
            case MOVE_DIR.UP:
                pointSnakePartPosition.x = lastSnakePart.GetSnakePartTransform().position.x;
                pointSnakePartPosition.y = lastSnakePart.GetSnakePartTransform().position.y - 1;
                break;
            case MOVE_DIR.DOWN:
                pointSnakePartPosition.x = lastSnakePart.GetSnakePartTransform().position.x;
                pointSnakePartPosition.y = lastSnakePart.GetSnakePartTransform().position.y + 1;
                break;
            case MOVE_DIR.LEFT:
                pointSnakePartPosition.y = lastSnakePart.GetSnakePartTransform().position.y;
                pointSnakePartPosition.x = lastSnakePart.GetSnakePartTransform().position.x + 1;
                break;
            case MOVE_DIR.RIGHT:
                pointSnakePartPosition.y = lastSnakePart.GetSnakePartTransform().position.y;
                pointSnakePartPosition.x = lastSnakePart.GetSnakePartTransform().position.x - 1;
                break;
        }

        Transform pointSnakePart = Instantiate(GameAssets.GetInstance().snakeSprite,
                                        pointSnakePartPosition, Quaternion.identity);
        snakeBody.Add(new ASnakePart(pointSnakePart, false, lastSnakePart.GetDirection()));
        SNAKE_PARTS++;
    }

    private void SnakePart_OnGetPoint1(object sender, System.EventArgs e){
        OnGetPoint();
    }

    private void SnakePart_OnGetPoint2(object sender, System.EventArgs e){
        OnGetPoint();
    }

    private void SnakePart_OnDeathCollide(object sender, System.EventArgs e){
        Rewind();
    }

    private void SnakePart_OnGetTimeSlow(object sender, System.EventArgs e){
        if (!isFirstTimeSlow)
        {
            Instructions.GetInstance().ShowTimeSlowInstruction();
            isFirstTimeSlow = true;
        }
        numberOfTimeSlowAvailable++;
    }

    private bool isRewindPossible(){
        return Level.score >= MIN_SCORE_NEEDED_TO_REWIND;
    }

    private void Rewind(){
        if (isRewindPossible()){
            AudioHandler.PlayAudio(AudioHandler.Sound.RewindSound, false);
            //AudioHandler.PlayAudio(AudioHandler.Sound.Death, false);
            MIN_SCORE_NEEDED_TO_REWIND += 2.0f;
            isSlow = false;
            timeSlowImage.SetActive(isSlow);
            currentTimescaleBeforeRewind = Time.timeScale;
            Time.timeScale = TIMESCALE_AT_REWIND;
            rewindWindow.SetActive(true);
            isRewind = true;
            RewindSnakeDirection();
        }
        else{
            GameOverHandler();
        }
    }

    private void GameOverHandler(){
        Debug.Log("Game over!!");
        AudioHandler.StopAudioForGameOver();
        AudioHandler.PlayAudio(AudioHandler.Sound.GameOver, false);
        Time.timeScale = 1.0f;
        Level.score = 0;
        Level.scoreCopy = 0;
        gameOver = true;
        rewindWindow.SetActive(false);
        if (gameOverAnimator.gameObject.activeSelf) {
            gameOverAnimator.SetTrigger("SnakeGameOver"); //that snake animation on Game Over screen
        }
    }

    public bool IsPointOnSnakeBody(int x, int y){
        foreach (ASnakePart snakePart in snakeBody){
            if (Vector2.Distance(snakePart.GetSnakePartTransform().position, new Vector2(x, y)) < CHANGE_DIR_PRECISION){
                //Debug.Log("Not spawning point cause its too close to snake part");
                return false;
            }
        }

        return true;
    }


    private class ASnakePart{
        private Transform snakePartTransform;
        private bool isHead;
        private bool shouldChangeDir; //probably not needed
        private MOVE_DIR direction;

        public ASnakePart(Transform snakePartTransform, bool isHead, MOVE_DIR direction){
            this.snakePartTransform = snakePartTransform;
            this.isHead = isHead;
            this.shouldChangeDir = false;
            this.direction = direction;
        }

        public Transform GetSnakePartTransform(){
            return snakePartTransform;
        }

        public void SetSnakePartTransform(Vector3 snakePartPosition){
            this.snakePartTransform.position = snakePartPosition;
        }

        public bool IsThisSnakeHead(){
            return isHead;
        }

        public bool ShouldPartChangeDirection(){
            return shouldChangeDir;
        }

        public void SetShouldPartChangeDirection(bool shouldChangeDir){
            this.shouldChangeDir = shouldChangeDir;
        }

        public MOVE_DIR GetDirection(){
            return direction;
        }

        public void SetDirection(MOVE_DIR direction){
            this.direction = direction;
        }
    }

    private class DirectionChangePoint{
        private Vector3 directionChangeCoords;
        private MOVE_DIR directionTurned;
        private MOVE_DIR rewindDirection;
        private bool active; //to ensure that DCP which are not in the snakes body dont affect the snake

        public DirectionChangePoint(Vector3 directionChangeCoords, 
                                    MOVE_DIR directionTurned, 
                                    MOVE_DIR rewindDirection){
            this.directionChangeCoords = directionChangeCoords;
            this.directionTurned = directionTurned;
            this.rewindDirection = rewindDirection;
            this.active = true; //by default a DCP is active
        }

        public Vector3 GetDirectionChangeCoords(){
            return directionChangeCoords;
        }
        public void SetDirectionChangeCoords(Vector3 directionChangeCoords){
            this.directionChangeCoords = directionChangeCoords;
        }
        public MOVE_DIR GetDirectionTurned(){
            return directionTurned;
        }
        public void SetDirectionTurned(MOVE_DIR directionTurned){
            this.directionTurned = directionTurned;
        }
        public MOVE_DIR GetRewindDirection(){
            return rewindDirection;
        }
        public void SetRewindDirection(MOVE_DIR rewindDirection){
            this.rewindDirection = rewindDirection;
        }
        public bool isActive(){
            return active;
        }
        public void SetActive(bool active){
            this.active = active;
        }
    }


}

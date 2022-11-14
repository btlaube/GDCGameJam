using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{

    public Sprite[] sprites;
    public GameObject NPC;
    public Transform player;

    [SerializeField] private float waveSize = 10f;
    [SerializeField] private float xRange = 5f;
    [SerializeField] private float yRange = 5f;
    [SerializeField] private float waveRate = 5f;
    [SerializeField] private float buffer = 10f;
    [SerializeField] private float coveredThreshold = 0.5f;
    private float timer;
    private int score = -1;
    private int highscore = -1;
    private CanvasGroupScript canvasGroup;
    private AudioManager audioManager;
    private TMP_Text timerText;
    private TMP_Text scoreText;
    private TMP_Text highscoreText;

    void Start() {
        canvasGroup = GameObject.Find("Canvas Group").GetComponent<CanvasGroupScript>();
        scoreText = GameObject.Find("Score").GetComponent<TMP_Text>();
        highscoreText = GameObject.Find("High").GetComponent<TMP_Text>();
        timerText = GameObject.Find("Time").GetComponent<TMP_Text>();
        audioManager = AudioManager.instance;

        timer = Time.deltaTime;
        timerText.text = ((int)(waveRate - timer)).ToString();

        float vertExtent = Camera.main.GetComponent<Camera>().orthographicSize;    
        float horzExtent = vertExtent * Screen.width / Screen.height;

        xRange = horzExtent + buffer;
        yRange = vertExtent + buffer;
        StartRound();
    }

    void Update() {
        //string tempTimer = timerText.text;
        scoreText.text = $"{score}";
        highscoreText.text = $"{highscore}";
        timerText.text = $"{((int)(waveRate - timer)+1).ToString()}s";
        //if (timerText.text != tempTimer) {
        //    audioManager.Play("Timer");
        //}
        timer += Time.deltaTime;
        
        if (timer >= waveRate) {
            timer = Time.deltaTime;
            float covered = FindPlayer();
            if (covered >= coveredThreshold) {
                StartRound();
            }
            else {
                GameOver();
            }
            
        }
    }

    public void GameOver() {
        //Time.timeScale = 0f;
        score = 0;
        //canvasGroup.GameOver();
        audioManager.Play("GameOver");
    }

    public void StartRound() {
        score++;
        if(score > highscore) {
            highscore = score;
        }
        if (score >= 1) {
            audioManager.Play("Point");
        }        
        SpawnEnemies();
        SetFurthestNPC();
    }

    public void SpawnEnemies() {
        foreach (Transform NPC in transform) {
            Destroy(NPC.gameObject);
        }

        for (int i = 0; i <= waveSize; i++) {
            //Pick which enemy to spawn with varying probability
            int whichSprite = Random.Range(0, sprites.Length);

            float randX;
            float randY;
            bool extremeAxis = Random.value > 0.5;
            bool whichExtreme = Random.value > 0.5;
            if(extremeAxis) {
                if(whichExtreme) {
                    randX = xRange;
                }
                else {
                    randX = -xRange;
                }
                randY = Random.Range(-yRange, yRange);
            }
            else {
                if(whichExtreme) {
                    randY = yRange;
                }
                else {
                    randY = -yRange;
                }
                randX = Random.Range(-xRange, xRange);
            }

            Vector3 spawnPosition = new Vector3(randX, randY, transform.position.z);

            //Chooses z position
            bool zPosition = Random.value > 0.3;
            if (zPosition) {
                spawnPosition.z = -1f;
            }
            else {
                spawnPosition.z = 1f;
            }

            GameObject newEnemy = Instantiate(NPC, spawnPosition, Quaternion.identity, transform);
            newEnemy.GetComponent<SpriteRenderer>().sprite = sprites[whichSprite];
        }
    }

    private void SetFurthestNPC() {
        float maxDist = 0f;
        Transform furthestNPC = transform.GetChild(0);
        foreach (Transform NPC in transform) {
            float distance = Vector2.Distance(NPC.position, NPC.GetComponent<NPCBehavior>().target);
            if (distance > maxDist) {
                furthestNPC = NPC;
                maxDist = distance;
            }
        }

        foreach (Transform NPC in transform) {
            NPC.GetComponent<NPCBehavior>().furthestNPC = furthestNPC;
        }
    }

    private float FindPlayer() {
        float percentXUncovered = 1f;
        float percentYUncovered = 1f;
        float totalCovered = 0f;
        foreach (Transform NPC in transform) {
            if (NPC.position.z == -1f) {
                percentXUncovered = Mathf.Clamp(Mathf.Abs(player.position.x - NPC.position.x)/0.6244822f, 0, 1);
                percentYUncovered = Mathf.Clamp(Mathf.Abs(player.position.y - NPC.position.y)/0.7714447f, 0, 1);
                totalCovered += (1-percentXUncovered) * (1-percentYUncovered);
            }
        }
        return totalCovered;
    }

}

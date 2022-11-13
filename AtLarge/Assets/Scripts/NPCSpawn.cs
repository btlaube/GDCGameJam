using UnityEngine;

public class NPCSpawn : MonoBehaviour
{
    public GameObject NPC;
    public Sprite[] sprites;

    [SerializeField] private float waveSize = 10f;
    [SerializeField] private int xRange = 5;
    [SerializeField] private int yRange = 5;
    [SerializeField] private float waveRate = 5f;
    private float timer;

    void Start() {
        SpawnEnemies();
        timer = Time.deltaTime;  
    }

     void Update() {
        timer += Time.deltaTime;
        if (timer >= waveRate) {
            SpawnEnemies();
            timer = Time.deltaTime;  
        }
    }

    public void SpawnEnemies() {        
        for (int i = 0; i <= waveSize; i++) {
            //Pick which NPC to spawn with varying probability
            int whichSprite = Random.Range(0, sprites.Length);

            float randX = Random.Range(-xRange, xRange);
            float randY = Random.Range(-yRange, yRange);

            //determine z value
            bool zValue = Random.value > 0.5;

            Vector3 spawnPosition = new Vector3(randX, randY, 0f);

            if (zValue) {
                spawnPosition.z = -1f;
            }
            else {
                spawnPosition.z = 1f;
            }

            GameObject newNPC = Instantiate(NPC, spawnPosition, Quaternion.identity, transform);
            newNPC.GetComponent<SpriteRenderer>().sprite = sprites[whichSprite];
        }
    }
}

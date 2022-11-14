using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasGroupScript : MonoBehaviour
{

    [SerializeField] private LevelLoaderScript levelLoader;

    void Start() {
        levelLoader = LevelLoaderScript.instance;
        this.gameObject.transform.GetChild(2).gameObject.SetActive(false);
    }

    public void GameOver() {
        this.gameObject.transform.GetChild(2).gameObject.SetActive(true);
    }

    public void LoadLevel(int levelIndex) {
        levelLoader.LoadScene(levelIndex);
    }


}

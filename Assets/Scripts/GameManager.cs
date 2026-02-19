using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    menu,
    inGame,
    pause,
    gameOver
}

public class GameManager : MonoBehaviour {

    public GameState currentGameState = GameState.menu;
    public static GameManager sharedInstance;
    public Canvas menuCanvas, gameCanvas, pauseCanvas, gameOverCanvas;
    public int countCoin, countDiamond;
    
    void Awake() {
        sharedInstance = this;
    }

    void Start() {
        BackToMenu();
    }

    void Update() {
        if (Input.GetButtonDown("Pause") && PlayerController.sharedInstance.isAlive) {
            Pause();
        }
    }

    public void BackToMenu() {
        SetGameState(GameState.menu);
    }

    public void StartGame() {
        SetGameState(GameState.inGame);
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraFollow cameraFollow = camera.GetComponent<CameraFollow>();
        cameraFollow.ResetCameraPosition();
        if (PlayerController.sharedInstance.transform.position.x > 0.1f) {
            LevelGenerator.sharedInstance.RemoveAllBlocks();
            LevelGenerator.sharedInstance.GenerateInitialBlocks();
        }
        PlayerController.sharedInstance.StartGame();
        countCoin = 0;
        countDiamond = 0;
        GetComponent<AudioSource>().Play();
    }

    public void Pause() {
        Time.timeScale = 0.0f;
        SetGameState(GameState.pause);
        GetComponent<AudioSource>().Pause();
    }

    public void Reanudar() {
        Time.timeScale = 1.0f;
        SetGameState(GameState.inGame);
        GetComponent<AudioSource>().Play();
    }

    public void GameOver() {
        SetGameState(GameState.gameOver);
    }

    void SetGameState(GameState newGameState) {
        if (newGameState == GameState.menu) {
            menuCanvas.enabled = true;
            gameCanvas.enabled = false;
            pauseCanvas.enabled = false;
            gameOverCanvas.enabled = false;
        }
        else if (newGameState == GameState.inGame) {
            menuCanvas.enabled = false;
            gameCanvas.enabled = true;
            pauseCanvas.enabled = false;
            gameOverCanvas.enabled = false;
        }
        else if (newGameState == GameState.pause)
        {
            menuCanvas.enabled = false;
            gameCanvas.enabled = false;
            pauseCanvas.enabled = true;
            gameOverCanvas.enabled = false;
        }
        else if (newGameState == GameState.gameOver) {
            menuCanvas.enabled = false;
            gameCanvas.enabled = false;
            pauseCanvas.enabled = false;
            gameOverCanvas.enabled = true;
        }
        this.currentGameState = newGameState;
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void CollectCoin(int value) {
        this.countCoin += value;
    }

    public void CollectDiamond(int value) {
        this.countDiamond += value;
    }

    public void StopSoundtrack() {
        GetComponent<AudioSource>().Stop();
    }
}
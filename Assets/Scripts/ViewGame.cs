using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewGame : MonoBehaviour {

    public Text coinText, diamondText, scoreText;
    private float travelledDistance;

    void Update() {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame) {
            int countCoin = GameManager.sharedInstance.countCoin;
            this.coinText.text = countCoin.ToString();
            int countDiamond = GameManager.sharedInstance.countDiamond;
            this.diamondText.text = countDiamond.ToString();
            travelledDistance = PlayerController.sharedInstance.GetDistance();
            this.scoreText.text = "Score\n" + travelledDistance.ToString("f0") + " mts";
        }
        if (GameManager.sharedInstance.currentGameState == GameState.gameOver) {
            this.coinText.text = "Game Over";
            float maxScore = PlayerPrefs.GetFloat("maxScore", 0);
            this.diamondText.text = "Max Score: " + maxScore.ToString("f0") + " mts";
            this.scoreText.text = "Current Score: " + travelledDistance.ToString("f0") + " mts";
        }
    }
}
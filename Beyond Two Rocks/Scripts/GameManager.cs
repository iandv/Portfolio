using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public string savePath = "GameManager.dat";
    public int winScore;
    public int score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;
    public Image powerBar;

    void Start()
    {
        powerBar.fillAmount = 0;
        scoreText.text = score + "/" + winScore;
        OEventManager.Instance.Subscribe("OnAsteroidDestruction", OnAsteroidDestruction);
        OEventManager.Instance.Subscribe("OnPlayerDamage", OnPlayerDamage);
        OEventManager.Instance.Subscribe("OnBatteryCharge", OnBatteryCharge);
        OEventManager.Instance.Subscribe("OnSave", SaveData);
        OEventManager.Instance.Subscribe("OnLoad", LoadData);
    }

    void SaveData(params object[] parameters)
    {
        var gmData = new GameManagerData(this);
        gmData.SaveBinary(Application.dataPath + savePath);
    }

    void LoadData(params object[] parameters)
    {
        var gmData = BinarySerializer.LoadBinary<GameManagerData>(Application.dataPath + savePath);
        winScore = gmData.winScore;
        score = gmData.score;
        scoreText.text = score + "/" + winScore;
    }

    private void OnBatteryCharge(params object[] parameters)
    {
        float charge = (float)parameters[0];
        float maxCharge = (float)parameters[1];
        powerBar.fillAmount = charge / maxCharge;
    }

    private void OnPlayerDamage(params object[] parameters)
    {
        int health = (int)parameters[0];
        healthText.text = health + " HP";
    }

    public void OnAsteroidDestruction(params object[] parameters)
    {
        Asteroid asteroid = (Asteroid)parameters[0];
        float median = AsteroidFlyweightPointer.config.maxSize - AsteroidFlyweightPointer.config.minSize;
        float percentage = median / 3;
        float smallSize = AsteroidFlyweightPointer.config.minSize + percentage;
        float mediumSize = median + percentage;
        int highScore = AsteroidFlyweightPointer.config.score;
        int mediumScore = highScore / 2;
        int lowScore = highScore / 4;
        if (asteroid.size < smallSize)
            score += highScore;
        else if (asteroid.size < mediumSize)
            score += mediumScore;
        else
            score += lowScore;
        scoreText.text = score + "/" + winScore;
        if (score >= winScore)
        {
            OnVictory();          
        }
    }

    private void OnVictory()
    {
        OEventManager.Instance.Trigger("OnVictory");
    }
}

using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI levelText;
    public GameObject winMessageObject;

    public void SetCoinText(int collected, int total)
    {
        coinText.text = $"Coins: {collected} / {total}";
    }

    public void SetLevelText(int currentLevel, int totalLevels)
    {
        levelText.text = $"Level: {currentLevel} / {totalLevels}";
    }

    public void ShowWinMessage(bool show)
    {
        winMessageObject.SetActive(show);
    }
}
using UnityEngine;
using TMPro;

public class  UIManager : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public GameObject winMessageObject;

    public void SetCoinText(int collected, int total)
    {
        coinText.text = $"Coins: {collected} / {total}";
    }

    public void ShowWinMessage(bool show)
    {
        winMessageObject.SetActive(show);
    }
}
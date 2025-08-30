using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Slider hpBar;
    public GameObject optionViewer;
    public void UpdateScore(int score)
    {
        scoreText.text = $"score : {score}";
    }
    public void UpdateHpSlide(float hp , float maxHp)
    {
        hpBar.value = hp / maxHp;
    }
    public bool OpenOption()
    {   
        optionViewer.SetActive(!optionViewer.activeSelf);
        return optionViewer.activeSelf;
    }
}

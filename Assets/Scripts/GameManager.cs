using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int score;

    public UIManager uiManager;

    public bool pause;
    public void AddScore(int add)
    {
        score += add;
        uiManager.UpdateScore(score);
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pause = uiManager.OpenOption();
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement; 


public class Gameover : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject Button;

      private void Start()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen  .SetActive(false); 
            Button.SetActive(false);
        }
    }

    public void ShowGameOverScreen()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("SampleScene");
}
}
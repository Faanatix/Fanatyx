using UnityEngine;

public class ClasssSelection : MonoBehaviour
{
    [SerializeField] public GameObject archerButton; // Przycisk Archera
    [SerializeField] public GameObject samuraiButton; // Przycisk Samuraia

    private void Start()
    {
        archerButton.SetActive(true);
        samuraiButton.SetActive(true);
    }
public void SelectArcher()
{
    PlayerPrefs.SetString("SelectedClass", "Archer");
    PlayerPrefs.Save();
    LoadGameScene();
}

public void SelectSamurai()
{
    PlayerPrefs.SetString("SelectedClass", "Samurai");
    PlayerPrefs.Save();
    LoadGameScene();
}

    private void LoadGameScene()
    {

        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}

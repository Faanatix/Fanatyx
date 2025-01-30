using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour
{
    public GameObject character1;
    public GameObject character2;
    public Button character1Button;
    public Button character2Button;
    public Camera cameraScript;

    public GameObject healthBar1; // Pasek zdrowia dla postaci 1
    public GameObject healthBar2; // Pasek zdrowia dla postaci 2

    private GameObject selectedCharacter;

    void Start()
    {
        character1Button.onClick.AddListener(() => SelectCharacter(character1, character2, healthBar1, healthBar2));
        character2Button.onClick.AddListener(() => SelectCharacter(character2, character1, healthBar2, healthBar1));
        DisableCharacters();
    }

    void SelectCharacter(GameObject chosen, GameObject toDisable, GameObject selectedHealthBar, GameObject unselectedHealthBar)
    {
        // Aktywuj wybraną postać, a dezaktywuj inną
        chosen.SetActive(true);
        toDisable.SetActive(false);

        // Pokaż odpowiedni pasek zdrowia dla wybranej postaci
        selectedHealthBar.SetActive(true);
        unselectedHealthBar.SetActive(false);

        EnableColliders(chosen);
        DisableColliders(toDisable);

        if (cameraScript != null)
        {
            Camera camScript = cameraScript.GetComponent<Camera>();
            if (camScript != null)
            {
                camScript.SetTarget(chosen.transform);
            }
        }

        character1Button.gameObject.SetActive(false);
        character2Button.gameObject.SetActive(false);
    }

    void DisableCharacters()
    {
        character1.SetActive(false);
        character2.SetActive(false);
    }

    void EnableColliders(GameObject character)
    {
        Collider2D[] colliders = character.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = true;
        }
    }

    void DisableColliders(GameObject character)
    {
        Collider2D[] colliders = character.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
    }
}

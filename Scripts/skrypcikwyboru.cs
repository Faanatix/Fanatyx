using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    public GameObject luczniksprite; // Sprite Archera
    public GameObject samuraiSprite; // Sprite Samuraia

    public GameObject Button;
    public GameObject ButtonŁU;

    public GameObject Oknowyboru;

    public void WyborLucznik()
    {
        if (luczniksprite != null)
        {
            Debug.Log("Wybrano Łucznika!");
            Oknowyboru.SetActive(false);
            luczniksprite.SetActive(true);
            Destroy(samuraiSprite);
        }
    }
      public void WyborSamurai()
    {
        if (luczniksprite != null)
        {
            Debug.Log("Wybrano sam!");
            Oknowyboru.SetActive(false);
            samuraiSprite.SetActive(true);
            Destroy(luczniksprite);
        }
    }

}
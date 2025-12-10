using UnityEngine;

public class CharacterSingleton : MonoBehaviour
{
    public static CharacterSingleton instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
            instance = this;
    }
}

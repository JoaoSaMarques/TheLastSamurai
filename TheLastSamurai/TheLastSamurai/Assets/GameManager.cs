using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool playerHasArmor = false;
    private int playerHealth = 100;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPlayerArmorState(bool hasArmor)
    {
        playerHasArmor = hasArmor;
    }

    public bool GetPlayerArmorState()
    {
        return playerHasArmor;
    }

    public void SetPlayerHealth(int health)
    {
        playerHealth = health;
    }

    public int GetPlayerHealth()
    {
        return playerHealth;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

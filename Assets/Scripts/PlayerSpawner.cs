using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePlayerSpawner : MonoBehaviour
{
    public GameObject[] spawner;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main")
        {
            GameManager.Instance.SpawnPlayer(spawner[GameManager.Instance.spawnIndex].transform.position);
        }
    }
}

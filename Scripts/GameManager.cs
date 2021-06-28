using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void ChangeStage (int lvlNr) {
        SceneManager.LoadScene(lvlNr);
    }

    public void GameOver () {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

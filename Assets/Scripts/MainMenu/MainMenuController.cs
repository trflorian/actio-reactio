using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        private bool isSimulationRunning;
        
        public void LoadLevel(int level)
        {
            SceneManager.LoadScene(level == -1 ? SceneManager.sceneCountInBuildSettings - 1 : level + 1);
        }
        
        public void ResetLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ShowScoreboard()
        {
            SceneManager.LoadScene("Scoreboard");
        }
    }
}

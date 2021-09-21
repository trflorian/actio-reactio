using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scoreboard
{
    public class ScoreboardManager : MonoBehaviour
    {
        public void BackToMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}

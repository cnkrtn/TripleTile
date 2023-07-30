using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        private const string ModeIndexKey = "ModeIndex";
        
        public void NormalMode()
        {
            PlayerPrefs.SetInt(ModeIndexKey, 0);
            PlayerPrefs.Save();
            NextScene();
        }

        public void TimedMode()
        {
            PlayerPrefs.SetInt(ModeIndexKey, 1);
            PlayerPrefs.Save();
            NextScene();
        }

        private void NextScene()
        {
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex +1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    
    public void OpenScene(int levelId)
    {
        string levelname = "Level" + (levelId);
        SceneManager.LoadScene(levelname);
    }
}

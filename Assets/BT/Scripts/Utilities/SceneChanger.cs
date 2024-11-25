using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BombThrower
{
    public class SceneChanger : SingletonBehavior<SceneChanger>
    {
        public static int score;
        // Start is called before the first frame update


        // If you want call to this method, you should write to:"SceneChanger.instance.LoadLevel("SceneName")".
        public void LoadLevel(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void LaodScene01()
    {
        SceneManager.LoadScene("Stage 01");
    }

    public void LaodScene02()
    {
        SceneManager.LoadScene("Stage 02");
    }
}

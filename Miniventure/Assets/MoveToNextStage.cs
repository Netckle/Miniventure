using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MoveToNextStage : MonoBehaviour
{
    public string stageName;
    public float waitTime;

    public TextMeshProUGUI tmp;
    public string content;

    private void Start() 
    {
        StartCoroutine(CoMoveToNext());
    }

    IEnumerator CoMoveToNext()
    {
        tmp.text = "";

        foreach (char letter in content)
        {       
            tmp.text += letter;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(stageName);
    }
}

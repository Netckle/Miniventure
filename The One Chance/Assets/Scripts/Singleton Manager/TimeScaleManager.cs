using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleManager : MonoBehaviour
{
    bool isPause = false;

    public static TimeScaleManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (isPause == false)
            {
                Time.timeScale = 0;
                isPause = true;
                //DialogueManager.instance.StartDialogue(JsonManager.instance.Load<Dialogue>(), 0, 3);
                return;
            }

            if (isPause == true)
            {
                Time.timeScale = 1;
                isPause = false;
                return;
            }
        }        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTest : MonoBehaviour
{
    public Fade fade;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            fade.FadeIn(1.0f, null);            
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            fade.FadeOut(1.0f, null);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectStage : MonoBehaviour 
{
    public Button[] buttons;

    private JsonManager jsonManager;

    SaveData[] saves;

    private void Start() 
    {
        jsonManager = GameObject.Find("Json Manager").GetComponent<JsonManager>();
        saves = jsonManager.Load<SaveData>("SaveData", "Save.json");
    }

    private void Update() 
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            if(saves[i].stageClear)
            {
                ColorBlock cb = buttons[i].colors;
                Color newColor = Color.gray;

                cb.normalColor = newColor;
                buttons[i].colors = cb;
            }
            else if (!saves[i].stageClear)
            {
                ColorBlock cb = buttons[i].colors;
                Color newColor = Color.white;

                cb.normalColor = newColor;
                buttons[i].colors = cb;
            }
        }
    }
}
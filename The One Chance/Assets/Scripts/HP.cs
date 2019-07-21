using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HP : MonoBehaviour
{
    public MoveSlimeDot slime;
    public Image HP_BAR;
    public TextMeshProUGUI HP_TEXT;

    void Update()
    {
        PlayerHPbar();
    }

    public void PlayerHPbar()
    {
        float HP = slime.HP;
        HP_BAR.fillAmount = HP/20.0f;
        HP_TEXT.text = string.Format("HP {0} / 20", HP);
        
    }
}

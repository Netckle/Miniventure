using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DogDrip : MonoBehaviour
{
    GameObject npc;

    IEnumerator CoDogDrip(int start, int end)
    {

        // 다른 요소 전부 정지
        npc.GetComponent<BossMovement>().Pause();

        DialogueManager.instance.StartDialogue(JsonManager.instance.Load<Dialogue>(), start, end);

        // until dialogue is end

        yield return null;

        // Camera?
        npc.GetComponent<BossMovement>().Release();

        npc.GetComponent<BossMovement>().TakeDamage(10); // + 날라가기 폭발 이펙트 추가 + 깜박임 + 정지 시간 조절 변수
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class MoveManager : MonoBehaviour
{
    // Message Box Variables
    public Image messageBox;
    public TextMeshProUGUI message;

    public bool moveIsEnd = false;

    public void Move(GameObject obj, float xEndPos, float yEndPos, float moveTime, Ease moveType = Ease.Linear)
    {
        moveIsEnd = false;
        Vector2 endPos = new Vector3(xEndPos, yEndPos, 0);

        obj.transform
            .DOMove(endPos, moveTime)
            .SetEase(moveType)
            .OnComplete(EndMove);
    }

    public void Move(GameObject obj, Vector3 _endPos, float moveTime, Ease moveType = Ease.Linear)
    {
        moveIsEnd = false;
        Vector3 endPos = new Vector3(_endPos.x, _endPos.y, 0);

        obj.transform
            .DOMove(endPos, moveTime)
            .SetEase(moveType)
            .OnComplete(EndMove);
    }

    private void EndMove()
    {
        moveIsEnd = true;
    }

    public void SpawnMessageBox(Vector2 pos, Vector2 offset, string _message, float typeTime = 0.01f)
    {
        StartCoroutine(CoSpawnMessageBox(pos, offset, _message, typeTime));
    }

    private IEnumerator CoSpawnMessageBox(Vector3 pos, Vector3 offset, string _message, float typeTime = 0.01f)
    {
        messageBox.gameObject.SetActive(true);

        // Set Message Box
        Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
        
        screenPos += offset;
        screenPos.z = 0;

        messageBox.transform.position = screenPos;

        // Set Message
        message.text = "";

        foreach (char letter in _message)
        {       
            message.text += letter;
            yield return new WaitForSeconds(typeTime);
        }

        yield return null;
    }

    public void MessageBoxFollow(Vector3 pos, Vector3 offset)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
        
        screenPos += offset;
        screenPos.z = 0;

        //essageBox.transform.position = screenPos;
    }
}

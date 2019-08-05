using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class DialogueManager : MonoBehaviour
{
    public void SetActivePanel(bool flag)
    {
        panel.gameObject.SetActive(flag);
        namePanel.gameObject.SetActive(flag);
    }

    Queue<Dialogue> sentences = new Queue<Dialogue>();

    [Header("UI")]
    public Image panel;
    public TextMeshProUGUI content;
    public Image namePanel;
    public TextMeshProUGUI nameContent;

    public bool dialogueIsEnd = false;

    RectTransform rt;

    public void StartDialogue(Dialogue[] data, int start, int end)
    {
        dialogueIsEnd = false;
        FindObjectOfType<PlayerMovement>().isTalking = true;

        panel.gameObject.SetActive(true);
        namePanel.gameObject.SetActive(true);

        sentences.Clear();

        rt = panel.gameObject.GetComponent<RectTransform>(); 

        for (int i = start; i < end + 1; ++i)
        {
            sentences.Enqueue(data[i]);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        Dialogue sentence = sentences.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(Dialogue sentence)
    {
        content.text = "";
        nameContent.text = "";

        nameContent.text = sentence.name;
        
        float xPos = 0.0f;

        if (sentence.target == "none")
        {
            //if (Camera.main.GetComponent<MultipleTargetCamera>().targets.Length > 1)
            {
                //Camera.main.GetComponent<MultipleTargetCamera>().SetTarget();
            }
        }
        else if (sentence.target != "none")
        {
            Camera.main.GetComponent<MultipleTargetCamera>().SetTarget(GameObject.FindGameObjectWithTag(sentence.target).transform);
        }

        foreach (char letter in sentence.content)
        {       
            namePanel.rectTransform.anchoredPosition = new Vector2(rt.anchoredPosition.x - (rt.sizeDelta.x * 0.5f) + (namePanel.rectTransform.sizeDelta.x * 0.5f), (rt.anchoredPosition.y + rt.sizeDelta.y * 0.5f));

            content.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
        yield return null;
    }

    void EndDialogue()
    {
        panel.gameObject.SetActive(false);
        namePanel.gameObject.SetActive(false);

        dialogueIsEnd = true;
        FindObjectOfType<PlayerMovement>().isTalking = false;
    }
}

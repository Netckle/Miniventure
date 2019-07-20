using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    Queue<Dialogue> sentences = new Queue<Dialogue>();

    [Header("UI")]
    public Image panel;
    public TextMeshProUGUI content;
    public Image namePanel;
    public TextMeshProUGUI nameContent;

    public bool dialogueIsEnd = false;

    public void StartDialogue(Dialogue[] data, int start, int end)
    {
        dialogueIsEnd = false;
        FindObjectOfType<PlayerMovement>().isTalking = true;

        panel.gameObject.SetActive(true);
        namePanel.gameObject.SetActive(true);

        sentences.Clear();

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
            xPos = panel.rectTransform.position.x - (panel.rectTransform.sizeDelta.x / 2) + (nameContent.rectTransform.sizeDelta.x / 2);
            namePanel.rectTransform.position = new Vector3(xPos, namePanel.rectTransform.position.y, namePanel.rectTransform.position.z);

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

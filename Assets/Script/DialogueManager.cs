using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextAsset inkFile;
    [SerializeField] private GameObject textBox;
    [SerializeField] private GameObject customButton;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject Background;
    [SerializeField] private Texture endingImage;
    [SerializeField] private bool isTalking = false;

    static Story story;
    TextMeshProUGUI nametag;
    TextMeshProUGUI message;
    List<string> tags;
    static Choice choiceSelected;

    // Start is called before the first frame update
    void Start()
    {
        story = new Story(inkFile.text);
        message = textBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        nametag = textBox.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        tags = new List<string>();
        choiceSelected = null;
        AdvanceDialogue();
    }

    public void next(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log(choiceSelected);
            if (story.canContinue)
            {
                //nametag.text = "Phoenix";
                AdvanceDialogue();

                //Are there any choices?
                if (story.currentChoices.Count != 0)
                {
                    StartCoroutine(ShowChoices());
                }
            }
            else
            {
                FinishDialogue();
            }
        }        
    }


    // Finished the Story (Dialogue)
    private void FinishDialogue()
    {
        Debug.Log("End of Dialogue!");
    }

    // Advance through the story 
    void AdvanceDialogue()
    {
        string currentSentence = story.Continue();
        ParseTags();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));
    }

    // Type out the sentence letter by letter and make character idle if they were talking
    IEnumerator TypeSentence(string sentence)
    {
        message.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            message.text += letter;
            yield return null;
        }
        yield return null;
    }

    // Create then show the choices on the screen until one got selected
    IEnumerator ShowChoices()
    {
        List<Choice> _choices = story.currentChoices;

        for (int i = 0; i < _choices.Count; i++)
        {
            GameObject temp = Instantiate(customButton, optionPanel.transform);
            temp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _choices[i].text;
            temp.transform.position += Vector3.down * 200*i;
            temp.GetComponent<Selectable>().element = _choices[i];
        }

        optionPanel.SetActive(true);

        yield return new WaitUntil(() => { return choiceSelected != null; });

        AdvanceFromDecision();
    }

    // Tells the story which branch to go to
    public static void SetDecision(Choice element)
    {
        choiceSelected = element;
        Debug.Log(choiceSelected);
        story.ChooseChoiceIndex(choiceSelected.index);
    }

    // After a choice was made, turn off the panel and advance from that choice
    void AdvanceFromDecision()
    {
        optionPanel.SetActive(false);
        for (int i = 0; i < optionPanel.transform.childCount; i++)
        {
            Destroy(optionPanel.transform.GetChild(i).gameObject);
        }


        choiceSelected = null; // Forgot to reset the choiceSelected. Otherwise, it would select an option without player intervention.
        Debug.Log(choiceSelected);

        AdvanceDialogue();
  
    }

    /*** Tag Parser ***/
    /// In Inky, you can use tags which can be used to cue stuff in a game.
    /// This is just one way of doing it. Not the only method on how to trigger events. 
    void ParseTags()
    {
        tags = story.currentTags;
        foreach (string t in tags)
        {
            string prefix = t.Split(": ")[0];
            string param = t.Split(": ")[1];

            switch(prefix.ToLower())
            {
                case "speaker":
                    setSpeakerName(param);
                    break;
                case "end":
                    ending();
                    break;
                default:
                    break;
            }
        }
    }

    void ending()
    {
        Background.GetComponent<Image>().image = endingImage;
    }

    void setSpeakerName(string _name)
    {
        switch(_name)
        {
            case "player":
                nametag.text = "you";
                break;
            case "Ange":
                nametag.text = "Ange";
                break;
            case "narration":
                nametag.text = "";
                break;
        }
    }
}

using Ink.Runtime;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public Choice element;
    public void Decide()
    {
        DialogueManager.SetDecision(element);
    }

}

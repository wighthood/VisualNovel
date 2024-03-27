using System.Collections;
using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    [SerializeField] private float Delay;
    private string CurrentText = "";

    IEnumerator TextDisplay(string m_Text)
    {
        for (int i  = 0; i <= m_Text.Length; i++)
        {
            CurrentText = m_Text.Substring(0, i);
            this.GetComponent<TMP_Text>().text = CurrentText;
            yield return new WaitForSeconds(Delay);
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NumberChooser : MonoBehaviour
{
    public UnityEvent QuestEnded;
    [SerializeField] private int targetAge;
    [SerializeField] private int currentAge;
    [SerializeField] private TMP_Text text;

    [SerializeField] private ParticleSystem success;
    [SerializeField] private ParticleSystem failure;

    private TreesQuest quest;
    private static int doneCount = 0;

    private void OnEnable()
    {
        quest = FindObjectOfType<TreesQuest>();
    }

    public void Next()
    {
        var num = currentAge + 1;
        if (num > 20)
            num = 0;
        currentAge = num;

        text.text = num.ToString();
    }

    public void Previous()
    {
        var num = currentAge - 1;
        if (num < 0)
            num = 20;
        currentAge = num;

        text.text = num.ToString();
    }

    public void Apply()
    {
        if (targetAge == currentAge)
        {
            success.Play();
            doneCount += 1;
            if (doneCount == 3)
            {
                quest.EndQuest();
                QuestEnded.Invoke();
            }
            gameObject.SetActive(false);
        }
        else
        {
            failure.Play();
        }
    }
}

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Chooser : MonoBehaviour
{
    public UnityEvent QuestEnded;
    [SerializeField] private Animal targetAnimal;
    [SerializeField] private Animal currentAnimal;
    [SerializeField] private Sprite[] animalSprites;
    [SerializeField] private Image image;

    [SerializeField] private ParticleSystem success;
    [SerializeField] private ParticleSystem failure;

    private PathwaysQuest quest;
    private static int doneCount = 0;

    private void OnEnable()
    {
        quest = FindObjectOfType<PathwaysQuest>();
    }

    public void Next()
    {
        var num = (int)currentAnimal + 1;
        if (num > 3)
            num = 0;
        currentAnimal = (Animal)num;

        image.sprite = animalSprites[(int)currentAnimal];
    }

    public void Previous()
    {
        var num = (int)currentAnimal - 1;
        if (num < 0)
            num = 3;
        currentAnimal = (Animal)num;

        image.sprite = animalSprites[(int)currentAnimal];
    }

    public void Apply()
    {
        if (targetAnimal == currentAnimal)
        {
            success.Play();
            doneCount += 1;
            if (doneCount == 4)
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

public enum Animal
{
    Duck = 0,
    Sqrl = 1,
    Hare = 2,
    Crtr = 3,
}

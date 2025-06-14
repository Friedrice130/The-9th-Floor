using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CScenes1 : MonoBehaviour
{
    public Image black;
    public Animator anim;
    public List<GameObject> pages;
    private int currentPageIndex = 0;

    private List<GameObject> currentSteps = new List<GameObject>();
    private int currentStepIndex = 0;

    public AudioClip clickSound;
    public AudioClip sound;

    private AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        ShowPage(0);
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShowNextStep();
        }
    }

    void ShowPage(int index)
    {
        foreach (var page in pages) page.SetActive(false);

        currentPageIndex = index;
        if (currentPageIndex >= pages.Count)
        {
            EndCutscene();
            return;
        }

        GameObject currentPage = pages[currentPageIndex];
        currentPage.SetActive(true);

        currentSteps.Clear();
        foreach (Transform child in currentPage.transform)
        {
            if (child.CompareTag("CutscenesElement"))
            {
                child.gameObject.SetActive(false);
                currentSteps.Add(child.gameObject);
            }
        }

        Button nextButton = currentPage.transform.Find("Next")?.GetComponent<Button>();
        if (nextButton != null) nextButton.gameObject.SetActive(false);
        if (nextButton != null) nextButton.onClick.RemoveAllListeners();
        if (nextButton != null) nextButton.onClick.AddListener(ShowNextPage);

        currentStepIndex = 0;
        ShowNextStep();
    }

    void ShowNextStep()
    {
        if (currentStepIndex < currentSteps.Count)
        {
            GameObject step = currentSteps[currentStepIndex];
            step.SetActive(true);

            Animator anim = step.GetComponent<Animator>();
            if (anim != null)
            {
                anim.ResetTrigger("Appear");
                anim.SetTrigger("Appear");
            }

            SpritesSFX soundPlayer = step.GetComponent<SpritesSFX>();
            if (soundPlayer != null)
            {
                soundPlayer.PlaySound(audioSource);
            }

            if (clickSound != null)
            {
                audioSource.PlayOneShot(clickSound);
            }

            currentStepIndex++;

            if (currentStepIndex == currentSteps.Count)
            {
                Button nextButton = pages[currentPageIndex].transform.Find("Next")?.GetComponent<Button>();
                if (nextButton != null)
                {
                    nextButton.gameObject.SetActive(true);
                }
            }
        }
    }

    void ShowNextPage()
    {
        ShowPage(currentPageIndex + 1);
    }

    void EndCutscene()
    {
        //StartCoroutine(Fading());
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    IEnumerator Fading()
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a == 1);
        SceneManager.LoadScene("MainMenu");
    }
}

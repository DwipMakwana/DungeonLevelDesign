using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;

    [SerializeField]
    TextMeshProUGUI missionText;
    [SerializeField]
    TextMeshProUGUI GameOverUIText;

    [SerializeField]
    GameObject Key;
    [SerializeField]
    GameObject GameOverUI;
    [SerializeField]
    GameObject[] Scrolls;

    [SerializeField]
    bool hasKey;
    [SerializeField]
    bool hasScrolls;
    [SerializeField]
    bool destroyedScrolls;
    [SerializeField]
    bool isPrisonOpen;
    [SerializeField]
    bool TrapFell;
    [SerializeField]
    bool Looted;

    [SerializeField]
    Animator doorAnimator;
    [SerializeField]
    Animator doorAnimator2;
    [SerializeField]
    Animator libraryAnimator;
    [SerializeField]
    Animator kettleAnimator;
    [SerializeField]
    Animator prisonAnimator;
    [SerializeField]
    Animator prison1Animator;
    [SerializeField]
    Animator fallTrapAnimator;
    [SerializeField]
    Animator lootAnimator;

    [SerializeField]
    GameObject playerController;

    private void Awake()
    {
        instance = this;
        missionText.text = "Find key";
    }

    public void GetKey()
    {
        Key.SetActive(false);
        hasKey = true;

        missionText.text = "Find path to lower level";
    }

    public void GetScrolls()
    {
        foreach(var scroll in Scrolls) 
        {
            scroll.SetActive(false);
        }

        hasScrolls = true;

        missionText.text = "Find path to treasure";
    }

    public void OpenDoor()
    {
        if (hasKey)
        {
            doorAnimator.Play("MainDoor");
            missionText.text = "Find sacred scrolls";
        }
    }

    public void OpenLibrary()
    {
        if (hasKey)
            libraryAnimator.Play("Library");
    }

    public void OpenDoor2()
    {
        if (destroyedScrolls)
        {
            doorAnimator2.Play("MainDoor2");
            missionText.text = "Find treasure!";
        }
    }

    public void DestroyScrolls()
    {
        if (hasScrolls)
        {
            kettleAnimator.Play("DestroyScrolls");
            destroyedScrolls = true;
        }
    }

    public void OpenPrisonGate()
    {
        isPrisonOpen = !isPrisonOpen;
        prisonAnimator.Play(!isPrisonOpen ? "PrisonGateClose" : "PrisonGateOpen");
    }

    public void FallTrap()
    {
        TrapFell = true;

        fallTrapAnimator.Play("TrapFall");
        fallTrapAnimator.transform.GetComponent<Transform>().transform.GetChild(1).gameObject.SetActive(true);
        fallTrapAnimator.transform.GetComponent<Transform>().transform.GetChild(2).gameObject.SetActive(true);
        fallTrapAnimator.transform.GetComponent<Transform>().transform.GetChild(3).gameObject.SetActive(true);
        fallTrapAnimator.transform.GetComponent<Transform>().transform.GetChild(4).gameObject.SetActive(true);

        prison1Animator.Play("PrisonGate2");
    }

    public void Loot()
    {
        if (TrapFell)
        {
            lootAnimator.Play("LootTreasure");
            missionText.text = "Exit dungeon!";
            Looted = true;
        }
    }

    public void GameEnd(bool gameOver)
    {
        if (Looted && !gameOver)
        {
            playerController.GetComponent<FirstPersonController>().enabled = false;
            GameOverUI.SetActive(true);
            GameOverUIText.text = "Treasure Looted!";
        }
        else if (gameOver)
        {
            StartCoroutine(DeadCameraAnim());
        }
    }

    IEnumerator DeadCameraAnim()
    {
        playerController.GetComponent<FirstPersonController>().enabled = false;

        float camRotX = Camera.main.transform.eulerAngles.x;
        float tick = 0f;
        while (camRotX != -90.0f)
        {
            tick += Time.deltaTime * 1.0f;
            camRotX = Mathf.Lerp(camRotX, -90.0f, tick);
            Camera.main.transform.eulerAngles = new Vector3(
                camRotX,
                Camera.main.transform.rotation.eulerAngles.y,
                Camera.main.transform.rotation.eulerAngles.z);
            yield return null;
        }
        GameOverUI.SetActive(true);
        GameOverUIText.text = "You are DEAD!";
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && GameOverUI.activeInHierarchy)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

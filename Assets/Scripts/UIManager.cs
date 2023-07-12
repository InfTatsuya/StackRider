using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance {  get; private set; }

    [SerializeField] GameObject ingameUI;
    [SerializeField] GameObject winUI;
    [SerializeField] GameObject loseUI;
    [SerializeField] GameObject startUI;

    [SerializeField] Button startBtn;
    [SerializeField] Button replayBtn;
    [SerializeField] Button nextLevelBtn;

    [SerializeField] Transform plusOnePanel;
    [SerializeField] Transform plusFivePanel;

    [SerializeField] GameObject plusOnePrefab;
    [SerializeField] GameObject plusFivePrefab;

    [SerializeField] TextMeshProUGUI coinText;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        startBtn.onClick.AddListener(StartBtnClicked);
        replayBtn.onClick.AddListener(ReplayBtnClicked);
        nextLevelBtn.onClick.AddListener(NextLevelBtnClicked);

        OpenStartPanel();
    }

    private void StartBtnClicked()
    {
        GameManager.Instance.OnNewGame();
        OpenIngameUI();
    }

    private void ReplayBtnClicked()
    {
        GameManager.Instance.ResetLevel();
        OpenStartPanel();
    }

    private void NextLevelBtnClicked()
    {
        GameManager.Instance.NextLevel();
        OpenStartPanel();
    }

    private void HideAll()
    {
        ingameUI.SetActive(false);
        winUI.SetActive(false);
        loseUI.SetActive(false);
        startUI.SetActive(false);
    }

    public void OpenIngameUI()
    {
        HideAll();
        ingameUI.SetActive(true);
    }

    public void OpenStartPanel()
    {
        HideAll();
        startUI.SetActive(true);
    }

    public void OpenWinPanel()
    {
        HideAll();
        winUI.SetActive(true);
    }

    public void OpenLosePanel()
    {
        HideAll();
        loseUI.SetActive(true);
    }

    public void ShowScoreByBall_Win(int index)
    {
        GameObject instance = Instantiate(plusFivePrefab, plusFivePanel);
        instance.GetComponent<PlusText>().Setup(index * 5);
        Destroy(instance, 2f);
    }

    public void ShowScoreByCollectBall()
    {
        GameObject instance = Instantiate(plusOnePrefab, plusOnePanel);
        Destroy(instance, 2f);
    }

    public void SetCoinText(int amt)
    {
        coinText.text = amt.ToString();
    }
}

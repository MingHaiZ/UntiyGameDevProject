using System.Collections;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [Header("End Screen")]
    [SerializeField] private UI_FadesScreen fadesScreen;

    [SerializeField] private GameObject endText;

    [Space]
    [SerializeField] private GameObject characterUI;

    [SerializeField] private GameObject skillTreeUI;

    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;

    public UI_SkillToolTip skillToolTip;
    public UI_ItemTooltip itemTooltip;
    public UI_StatToolTip statTooltip;
    public UI_CraftWindow craftWindow;

    private void Awake()
    {
        SwitchTo(skillTreeUI);
    }

    // Start is called before the first frame update
    void Start()
    {
        SwitchTo(inGameUI);
        itemTooltip.gameObject.SetActive(false);
        statTooltip.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchWithKeyTo(characterUI);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            SwitchWithKeyTo(craftUI);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchWithKeyTo(skillTreeUI);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SwitchWithKeyTo(optionsUI);
        }
    }

    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            bool isFadeScreen = transform.GetChild(i).GetComponent<UI_FadesScreen>() != null;
            if (!isFadeScreen)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        if (_menu != null)
        {
            _menu.SetActive(true);
        }
    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }

        SwitchTo(_menu);
    }

    private void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                return;
            }
        }

        SwitchTo(inGameUI);
    }

    public void SwitchOnEndScreen()
    {
        fadesScreen.FadeOut();
        StartCoroutine(EndScreenCorutine());
    }

    private IEnumerator EndScreenCorutine()
    {
        yield return new WaitForSeconds(1);
        endText.SetActive(true);
    }
}
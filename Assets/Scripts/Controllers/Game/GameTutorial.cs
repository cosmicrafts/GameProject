using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameTutorial : MonoBehaviour
{
    public enum tevent
    {
        dialog,
        tooltip,
        objective
    }

    public GameObject[] DeckUnits = new GameObject[4];
    public UIGameCard[] GameCards = new UIGameCard[8];

    public GameObject Dialog;

    public Image IconCharacter;
    public Text DialogText;

    [Range(1, 120)]
    public int CharactersPerSec = 30;

    tevent[] Events = new tevent[23]
    {
        tevent.dialog,
        tevent.dialog,
        tevent.dialog,
        tevent.tooltip,
        tevent.tooltip,
        tevent.tooltip,
        tevent.dialog,
        tevent.dialog,
        tevent.dialog,
        tevent.dialog,
        tevent.dialog,
        tevent.dialog,
        tevent.tooltip,
        tevent.objective,
        tevent.dialog,
        tevent.objective,
        tevent.dialog,
        tevent.objective,
        tevent.tooltip,
        tevent.dialog,
        tevent.dialog,
        tevent.dialog,
        tevent.tooltip
    };
    string[] Dialogs;

    public Sprite[] Characters = new Sprite[23];
    public GameObject[] Tips = new GameObject[5];
    public Text[] Objectives = new Text[3];
    public GameObject ObjectiveAlert;

    public GameObject AliadUnits;
    public GameObject EnemyUnits;

    string CurrentString;
    string TargetString;

    int ichar;
    int ievent;
    float delaychar;

    WaitForSeconds alertDelay;
    WaitForSeconds deployDelay;

    Unit EnemyUnit;

    // Start is called before the first frame update
    void Start()
    {
     
        InitDialogs();
        CurrentString = string.Empty;
        TargetString = Dialogs[0];

        ichar = 0;
        ievent = 0;
        delaychar = 1f / (float)CharactersPerSec;

        foreach (UIGameCard card in GameCards)
        {
            card.gameObject.SetActive(false);
        }
        GameCards[0].gameObject.SetActive(true);
        GameMng.P.CurrentEnergy = 10;
        GameMng.P.SetInControl(false);
        GameMng.GM.Targets[1].IsInmortal = true;
        EnemyUnit = GameMng.GM.Targets[0];
        EnemyUnit.IsInmortal = true;
        alertDelay = new WaitForSeconds(3f);
        deployDelay = new WaitForSeconds(0.5f);
        Objectives[0].gameObject.SetActive(true);
        StartCoroutine(ShowAlert());
    }

    // Update is called once per frame
    void Update()
    {
        if (delaychar > 0)
        {
            delaychar -= Time.deltaTime;
        }
        else
        {
            delaychar = 1f / (float)CharactersPerSec;
            if (ichar < TargetString.Length)
            {
                ichar++;
                CurrentString = TargetString.Substring(0, ichar);
            }
            CheckForObjectives();
        }
        DialogText.text = CurrentString;
    }

    public void NextDialog()
    {
        if (ievent < Events.Length)
        {
            if (ichar >= TargetString.Length)
            {
                GoNextEvent();
            }
            else
            {
                ichar = TargetString.Length;
                CurrentString = TargetString;
            }
        }
    }

    void CheckForEvents()
    {
        switch (ievent)
        {
            case 3:
                {
                    Tips[0].SetActive(true);
                    GameMng.P.SetInControl(true);
                    Objectives[0].color = Color.gray;
                    Objectives[1].gameObject.SetActive(true);
                    StartCoroutine(ShowAlert());
                }
                break;
            case 4:
                {
                    Tips[1].SetActive(true);
                }
                break;
            case 5:
                {
                    Tips[2].SetActive(true);
                }
                break;
            case 12:
                {
                    Tips[3].SetActive(true);
                    GameCards[1].gameObject.SetActive(true);
                    Objectives[1].color = Color.gray;
                    Objectives[2].gameObject.SetActive(true);
                    StartCoroutine(ShowAlert());
                }
                break;
            case 15:
                {
                    StartCoroutine(DeployEnnemyFleet());
                }
                break;
            case 17:
                {
                    StartCoroutine(DeployAlliadFleet());
                }
                break;
            case 18:
                {
                    GameCards[2].gameObject.SetActive(true);
                    GameCards[3].gameObject.SetActive(true);
                }
                break;
            case 19:
                {
                    Unit[] units = FindObjectsOfType<Unit>();

                    foreach (Unit unit in units)
                    {
                        unit.DisableUnit();
                    }
                    Objectives[2].color = Color.gray;
                }
                break;
            case 22:
                {
                    GameMng.GM.EndGame(Team.Blue);

                    foreach (Text objective in Objectives)
                    {
                        objective.gameObject.SetActive(false);
                    }

                    Animator anim = EnemyUnit.GetAnimator();
                    anim.speed = 0;
                }
                break;
        }
    }

    void CheckForObjectives()
    {
        switch (ievent)
        {
            case 3:
                {
                    if (GameMng.P.IsPreparingDeploy())
                    {
                        GoNextEvent();
                    }
                }
                break;
            case 4:
                {
                    if (GameMng.MT.GetDeploys() > 0)
                    {
                        GoNextEvent();
                    }
                }
                break;
            case 5:
                {
                    if (GameMng.MT.GetDeploys() > 1)
                    {
                        GoNextEvent();
                    }
                }
                break;
            case 12:
                {
                    if (GameMng.MT.GetDeploys() > 3)
                    {
                        GoNextEvent();
                    }
                }
                break;
            case 13:
                {
                    if (EnemyUnit.HitPoints < EnemyUnit.GetMaxHitPoints() / 2)
                    {
                        Unit[] units = FindObjectsOfType<Unit>();

                        foreach (Unit unit in units)
                        {
                            unit.DisableUnit();
                        }
                        EnemyUnit.HitPoints = EnemyUnit.GetMaxHitPoints() / 2;
                        EnemyUnit.Shield = EnemyUnit.GetMaxShield();
                        GoNextEvent();
                    }
                }
                break;
            case 18:
                {
                    if (EnemyUnit.HitPoints < EnemyUnit.GetMaxHitPoints() / 4)
                    {
                        GoNextEvent();
                    }
                }
                break;
        }
    }

    void GoNextEvent()
    {
        ievent++;
        ichar = 0;
        CurrentString = string.Empty;
        TargetString = Dialogs[ievent];
        IconCharacter.sprite = Characters[ievent];

        foreach (GameObject tip in Tips)
        {
            tip.SetActive(false);
        }

        Dialog.SetActive(Events[ievent] == tevent.dialog);
        GameMng.P.SetInControl(Events[ievent] != tevent.dialog);

        CheckForEvents();
    }

    void InitDialogs()
    {
        Dialogs = new string[23]
        {
            Lang.GetText("arg_tut_01"),
            Lang.GetText("arg_tut_02"),
            Lang.GetText("arg_tut_03"),
            string.Empty,
            string.Empty,
            string.Empty,
            Lang.GetText("arg_tut_04"),
            Lang.GetText("arg_tut_05"),
            Lang.GetText("arg_tut_06"),
            Lang.GetText("arg_tut_07"),
            Lang.GetText("arg_tut_08"),
            Lang.GetText("arg_tut_09"),
            string.Empty,
            string.Empty,
            Lang.GetText("arg_tut_10"),
            string.Empty,
            Lang.GetText("arg_tut_11"),
            string.Empty,
            string.Empty,
            Lang.GetText("arg_tut_12"),
            Lang.GetText("arg_tut_13"),
            Lang.GetText("arg_tut_14"),
            string.Empty,
        };

        for (int i = 0; i < Dialogs.Length; i++)
        {
            Dialogs[i] = Dialogs[i].Replace("$PLAYER", GameMng.PlayerData.NikeName);
        }
    }

    IEnumerator ShowAlert()
    {
        ObjectiveAlert.SetActive(true);
        yield return alertDelay;
        ObjectiveAlert.SetActive(false);
    }

    IEnumerator DeployEnnemyFleet()
    {
        GameMng.P.SetInControl(false);

        Unit[] units = FindObjectsOfType<Unit>();

        foreach (Unit unit in units)
        {
            unit.DisableUnit();
        }

        foreach (Transform child in EnemyUnits.transform)
        {
            child.gameObject.SetActive(true);
            child.GetComponent<Unit>().DisableUnit();
            yield return deployDelay;
        }

        GoNextEvent();
    }

    IEnumerator DeployAlliadFleet()
    {
        GameMng.P.SetInControl(false);

        foreach (Transform child in AliadUnits.transform)
        {
            child.gameObject.SetActive(true);
            child.GetComponent<Unit>().DisableUnit();
            yield return deployDelay;
        }

        GoNextEvent();

        Unit[] units = FindObjectsOfType<Unit>();

        foreach (Unit unit in units)
        {
            unit.EnableUnit();
        }
    }

    public void EndTutorial()
    {
        SceneManager.LoadScene(2);
       
    }
}

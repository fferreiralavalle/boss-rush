using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    public StageInfo baseStage;
    public UITransition formBlackTransition;

    protected Stage currentStage;
    protected Coroutine changeStageRoutine;

    private void Start()
    {
        GoToBaseStage();
    }

    public void GoToBaseStage()
    {
        ChangeStage(baseStage.stage, baseStage.transition);
    }

    public void GoToBaseStageFromBlack()
    {
        ChangeStage(baseStage.stage, formBlackTransition);
    }

    public void ChangeStage(Stage stage, UITransition transition)
    {
        if (changeStageRoutine != null) StopCoroutine(changeStageRoutine);
        TransitionMaster.Instance.PlayTransition(transition);
        GameMaster.Instance.Player?.StartListening();
        if (GameMaster.Instance.Player) GameMaster.Instance.Player.health.InvulnerableTime = transition.timeToCoverScreen;
        changeStageRoutine = StartCoroutine(ChangeAfter(stage, transition.timeToCoverScreen));
    }

    protected IEnumerator ChangeAfter(Stage stage, float time)
    {
        yield return new WaitForSeconds(time);
        GameMaster.Instance.Player?.StopListening();
        if (currentStage) Destroy(currentStage.gameObject);
        currentStage = Instantiate(stage, transform);
        GameMaster.Instance.SpawnPlayer(currentStage.playerSpawn.position);
        AudioMaster.Instance.PlayMusic(stage.onEnterAudio);
        if (currentStage.showPlayerGUI)
        {
            UIPlayerInfo.Instance.Show();
            UIPlayerInfo.Instance.UpdateMaxHearts();
            UIPlayerInfo.Instance.UpdateSpecialbar();
        }
        else
        {
            UIPlayerInfo.Instance.Hide();
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    public Stage GetActiveStage()
    {
        return currentStage;
    }
}

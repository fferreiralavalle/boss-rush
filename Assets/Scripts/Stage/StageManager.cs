using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    public StageInfo baseStage;

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

    public void ChangeStage(Stage stage, UITransition transition)
    {
        if (changeStageRoutine != null) StopCoroutine(changeStageRoutine);
        TransitionMaster.Instance.PlayTransition(transition);
        GameMaster.Instance.Player?.StartListening();
        changeStageRoutine = StartCoroutine(ChangeAfter(stage, transition.timeToCoverScreen));
    }

    protected IEnumerator ChangeAfter(Stage stage, float time)
    {
        yield return new WaitForSeconds(time);
        GameMaster.Instance.Player?.StopListening();
        if (currentStage) Destroy(currentStage.gameObject);
        currentStage = Instantiate(stage, transform);
        GameMaster.Instance.SpawnPlayer(currentStage.playerSpawn.position);
        if (currentStage.showPlayerGUI)
        {
            UIPlayerInfo.Instance.Show();
            UIPlayerInfo.Instance.UpdateMaxHearts();
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

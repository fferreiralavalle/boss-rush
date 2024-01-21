using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDefeater : MonoBehaviour
{
    public Enemy boss;
    public float slowTimeDuration = 4f;
    public float slowTimeAmount = 0.5f;
    public AudioData onDefeatMusic;

    public List<DialogueData> dialoguesOnDefeat = new List<DialogueData>();


    public void Start()
    {
        boss.health.onDeath += HandleDefeat;
    }

    public void HandleDefeat()
    {
        Time.timeScale = slowTimeAmount;
        GameMaster.Instance.Player.health.InvulnerableTime = 7;
        AudioMaster.Instance.PlayMusic(onDefeatMusic);
        StartCoroutine(HandleDefeatRoutine());
    }

    IEnumerator HandleDefeatRoutine()
    {
        yield return new WaitForSeconds(slowTimeDuration * slowTimeAmount);
        BossInfo.Instance.Hide();
        Time.timeScale = 1f;
        DialogueMaster.Instance.StartDialogue(dialoguesOnDefeat);
    }
}

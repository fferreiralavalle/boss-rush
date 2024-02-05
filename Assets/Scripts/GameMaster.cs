using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance;

    public Player playerPrefab;
    public List<GameStateVariableData> variables = new List<GameStateVariableData>();

    protected Dictionary<string, GameStateVariable> gameStateVariables = new Dictionary<string, GameStateVariable>();

    protected Player player;

    private void Awake()
    {
        Instance = this;
        IntiateGameVariables();
        // SpawnPlayer(transform.position);
    }

    protected void IntiateGameVariables()
    {
        gameStateVariables.Clear();
        foreach (GameStateVariableData variableData in variables)
        {
            GameStateVariable variable = new GameStateVariable(variableData.id, variableData.GetInitialValue());
            gameStateVariables.Add(variable.id, variable);
        }
    }

    public Player SpawnPlayer(Vector3 pos)
    {
        if (!player)
        {
            player = Instantiate(playerPrefab);
            PowerManager.Instance.ReapplyPowers();
        }
        player.transform.position = pos;
        return player;
    }

    public GameStateVariable GetGameStateVariable(string id)
    {
        if (gameStateVariables.ContainsKey(id)) return gameStateVariables[id];
        return null;
    }

    public T GetGameStateVariableValue<T>(string id)
    {
        GameStateVariable variable = GetGameStateVariable(id);
        if (variable != null) return variable.GetValue<T>();
        return default(T);
    }

    public void SetGameStateVariable(string id, object value)
    {
        GameStateVariable variable = GetGameStateVariable(id);
        if (variable != null)
        {
            variable.value = value;
        }
    }

    public void ResetGameState()
    {
        IntiateGameVariables();
        PowerManager.Instance.RemoveAllPowers();
        Player.HandlePowerChange(null);
    }

    public void ResetPlayBars()
    {
        Player.health.Heal(99999, true);
        Player.specialBar.Damage(new DamageSummary(99999));
        Player.Revive();
    }

    public Player Player { get { return player; } }
}

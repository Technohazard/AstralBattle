using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAction 
{
    public enum AgentActionTypesEnum
    {
        Damage,
        DamageOverTime,
        Heal,
        HealOverTime,
        Buff,
        Debuff
    }

    public AgentActionTypesEnum ActionType;

    // How much of the Action to do.
    public int Amount;
    
    // How long to deal this dmg/heal over time.
    // How long this lasts, if it's a buff
    public float TimeSpan;

    /// <summary>
    /// Use these stats to buff/debuff the Agent
    /// </summary>
    public AgentStats BuffStats;

    public AgentAction()
    {
        ActionType = AgentActionTypesEnum.Damage;
        Amount = 1;
    }
    
    public AgentAction(AgentActionTypesEnum type)
    {
        ActionType = type;
        Amount = 1;
    }
}

public class AgentActionTask
{
    // The task to execute
    public AgentAction agentAction;

    // Battle time at which to execute this action
    public float taskTime;
    
    // The index of target in the opposing party
    public int targetIndex;

    public AgentActionTask(float time, AgentAction action, int target)
    {
        taskTime = time;
        agentAction = action;
        targetIndex = target;
    }
}
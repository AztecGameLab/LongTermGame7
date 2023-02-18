﻿using Application.Gameplay.Combat;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The information needed to start a battle.
/// </summary>
public struct BattleData
{
    public List<GameObject> PlayerTeamInstances;
    public List<GameObject> EnemyTeamInstances;
    public List<Hook> Hooks;

    public EnemyOrderDecider Decider;
}
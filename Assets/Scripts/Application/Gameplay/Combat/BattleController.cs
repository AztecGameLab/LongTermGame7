﻿using Application.Core;
using Application.Gameplay.Combat;
using Application.StateMachine;
using ImGuiNET;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// The generic controller for all battles.
/// Handles the common turn sequencing and logic, win and lose conditions.
/// </summary>
public class BattleController : MonoBehaviour
{
    public List<GameObject> PlayerTeam => _playerTeam;
    public List<GameObject> EnemyTeam => _enemyTeam;

    private List<GameObject> _playerTeam = new List<GameObject>();
    private List<GameObject> _enemyTeam = new List<GameObject>();
    private List<Hook> _hooks = new List<Hook>();

    // todo: statemachinify
    private bool _isBattling;

    public StateMachine BattleStateMachine { get; private set; }
    
    public BattleIntro BattleIntro {get; private set;}
    public BattleLoss BattleLoss {get; private set;}
    public BattleVictory BattleVictory {get; private set;}
    public BattleRound BattleRound {get; private set;}
    public EnemyOrderDecider Decider { get; private set; }

    private void Awake()
    {
        ImGuiUtil.Register(DrawImGuiWindow).AddTo(this);

        BattleStateMachine = new StateMachine();

        BattleIntro = new BattleIntro { Controller = this };
        BattleLoss = new BattleLoss { Controller = this };
        BattleVictory = new BattleVictory { Controller = this };
        BattleRound = new BattleRound { Controller = this };
    }

    private void Update()
    {
        if (_isBattling)
        {
            BattleStateMachine.Tick();
        }
    }

    public void BeginBattle(BattleData data)
    {
        if (_isBattling)
        {
            return;
        }

        _isBattling = true;
        
        Debug.Log("Starting battle!");
        Decider = data.Decider;

        _hooks.Clear();
        _hooks.AddRange(data.Hooks);
        
        foreach (Hook dataHook in _hooks)
        {
            dataHook.Controller = this;
            dataHook.OnBattleStart();
        }

        _playerTeam.Clear();
        _playerTeam.AddRange(data.PlayerTeamInstances);
        
        _enemyTeam.Clear();
        _enemyTeam.AddRange(data.EnemyTeamInstances);
        
        BattleStateMachine.SetState(BattleIntro);
    }

    public void EndBattle()
    {
        _isBattling = false;
        
        // todo: we may have to pass more information on the ending of battle, e.g. win vs. loss and whatnot
        Debug.Log("Ending battle!");

        foreach (var hook in _hooks)
        {
            hook.OnBattleEnd();
        }
        
        BattleStateMachine.SetState(null);

        _hooks.Clear();
        _playerTeam.Clear();
        _enemyTeam.Clear();
    }
    
    private void DrawImGuiWindow()
    {
        ImGui.Begin("Battle Controller");
        
        ImGui.Text($"Current State: {BattleStateMachine.CurrentState?.GetType().Name}");

        if (ImGui.Button("End Battle"))
            EndBattle();
        
        ImGui.Text("Enemy team:");
        
        foreach (GameObject enemyTeamInstance in EnemyTeam)
            ImGui.Text($"\t{enemyTeamInstance.name}");
        
        ImGui.Text("Player team:");

        foreach (GameObject playerTeamInstance in PlayerTeam)
            ImGui.Text($"\t{playerTeamInstance.name}");
        
        ImGui.Text("Hooks:");

        foreach (Hook hook in _hooks)
            ImGui.Text($"\t{hook.GetType().Name}");

        ImGui.End();
    }
}
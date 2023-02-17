using Application.Gameplay;

public class KillAllEnemiesObjective : Hook
{
    private float _remainingHealth;
    
    public override void OnBattleStart()
    {
        foreach (var enemy in Controller.EnemyTeam)
        {
            if (enemy.TryGetComponent(out Health health))
            {
                health.OnHealthChange += HandleEnemyHealthChange;
            }
        }
    }

    private void HandleEnemyHealthChange(HealthChangeData data)
    {
        _remainingHealth += data.Delta;

        if (_remainingHealth <= 0)
        {
            Controller.EndBattle();
        }
    }
}
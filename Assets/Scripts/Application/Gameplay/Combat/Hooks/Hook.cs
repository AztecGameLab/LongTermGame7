public abstract class Hook
{
    public BattleController Controller { get; set; }
    
    public virtual void OnBattleStart() {}
    public virtual void OnBattleUpdate() {}
    public virtual void OnBattleEnd() {}
}
namespace Application.Gameplay.Combat
{
    using System;
    using Core;

    /// <summary>
    /// Listens for the beginning of overworld battles, and prepares the world state
    /// for the <see cref="BattleController"/>.
    /// </summary>
    [Serializable]
    public class OverworldBattleSetup : IDisposable
    {
        private BattleController _controller;
        private IDisposable _disposable;

        /// <summary>
        /// Initializes battle setup.
        /// </summary>
        /// <param name="controller">The battle controller that is used to handling start requests.</param>
        /// <returns>This instance.</returns>
        public OverworldBattleSetup Init(BattleController controller)
        {
            _controller = controller;
            _disposable = Services.EventBus.AddListener<OverworldBattleStartData>(HandleBattleStart, "Overworld battle setup");
            return this;
        }

        /// <summary>
        /// Cleans up battle setup resources.
        /// </summary>
        public void Dispose()
        {
            _disposable.Dispose();
        }

        private void HandleBattleStart(OverworldBattleStartData data)
        {
            var battleData = new BattleData(data.PlayerTeamInstances, data.EnemyTeamInstances, data.Hooks, data.EnemyOrderDecider);
            _controller.BeginBattle(battleData);
        }
    }
}

namespace Application.Gameplay.Combat.Hooks
{
    using System.Collections;
    using Cysharp.Threading.Tasks;
    using Dialogue;
    using UnityEngine;

    public class EmoteOnStart : Hook
    {
        public string emoteId = "surprised";

        public override IEnumerator OnBattleStart()
        {
            yield return base.OnBattleStart();
            yield return Object.FindObjectOfType<EmoteSystem>().PlayEmote(emoteId, Controller.EnemyTeam[0]).ToCoroutine();
        }
    }
}

namespace Application.Gameplay.Dialogue
{
    using System.Collections.Generic;
    using UnityEngine;
    using Yarn.Unity;

    /// <summary>
    /// Handles requests from the dialogue to play emotes.
    /// </summary>
    public class EmoteSystem : MonoBehaviour
    {
        [SerializeField]
        private DialogueRunner runner;

        [SerializeField]
        private DictionaryGenerator<string, EmotePlayer> emotePlayerGenerator;

        private Dictionary<string, EmotePlayer> _emotePlayerLookup;

        private void Start()
        {
            _emotePlayerLookup = emotePlayerGenerator.GenerateDictionary();
            runner.AddCommandHandler<string, GameObject>("emote", HandleEmoteCommand);
        }

        private Coroutine HandleEmoteCommand(string emoteId, GameObject target)
        {
            if (_emotePlayerLookup.TryGetValue(emoteId, out EmotePlayer value))
            {
                var emoteOrigin = target.GetComponentInChildren<EmoteOrigin>();

                if (emoteOrigin != null)
                {
                    target = emoteOrigin.gameObject;
                }

                return value.Play(target);
            }
            else
            {
                throw new KeyNotFoundException($"Yarn tried to play emote {emoteId}, but it doesn't exist!");
            }
        }
    }
}

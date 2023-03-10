using System;

namespace Application.Gameplay
{
    using System.Collections.Generic;
    using Cinemachine;
    using Combat.UI;
    using Core;
    using UniRx;
    using UnityEngine;

    /// <summary>
    /// Loads the selected members of the player team into the overworld.
    /// </summary>
    public class PlayerTeamWorldLoader : MonoBehaviour
    {
        private readonly Dictionary<TeamMemberData, TeamMemberWorldView> _memberViewLookup =
            new Dictionary<TeamMemberData, TeamMemberWorldView>();

        private readonly List<TeamMemberWorldView> _worldViewList =
            new List<TeamMemberWorldView>();

        public GroupFollowTarget MonsterFollowPlayer { get; set; } = new GroupFollowTarget();

        [SerializeField]
        private CinemachineVirtualCamera playerCamera;

        /// <summary>
        /// Gets a list of every spawned player team member in the scene.
        /// </summary>
        public IReadOnlyCollection<TeamMemberWorldView> SpawnedMembers => _worldViewList;

        /// <summary>
        /// Gets the currently spawned player object in the scene.
        /// </summary>
        public TeamMemberWorldView SpawnedPlayer { get; private set; }

        private void Start()
        {
            IReadOnlyReactiveCollection<TeamMemberData> selectedMembers = Services.PlayerTeamData.SelectedMembers;

            // Spawn all initial members.
            foreach (var member in selectedMembers)
            {
                SpawnWorldView(member);
            }

            SpawnedPlayer = Services.PlayerTeamData.Player.CreateWorldView();
            playerCamera.Follow = SpawnedPlayer.transform;

            MonsterFollowPlayer.Target = SpawnedPlayer.transform;

            // Update spawned members when the list data changes.
            selectedMembers.ObserveAdd()
                .Select(addEvent => addEvent.Value)
                .Subscribe(SpawnWorldView)
                .AddTo(this);

            selectedMembers.ObserveRemove()
                .Select(removeEvent => removeEvent.Value)
                .Subscribe(DestroyWorldView)
                .AddTo(this);
        }

        private void Update()
        {
            MonsterFollowPlayer.Tick();
        }

        private void SpawnWorldView(TeamMemberData member)
        {
            var instance = member.CreateWorldView();

            _worldViewList.Add(instance);
            _memberViewLookup.Add(member, instance);
            MonsterFollowPlayer.GroupMembers.Add(instance.transform);
        }

        private void DestroyWorldView(TeamMemberData member)
        {
            _worldViewList.Remove(_memberViewLookup[member]);
            var instance = _memberViewLookup[member].gameObject;
            MonsterFollowPlayer.GroupMembers.Remove(instance.transform);
            Destroy(_memberViewLookup[member].gameObject);
            _memberViewLookup.Remove(member);
        }
    }
}

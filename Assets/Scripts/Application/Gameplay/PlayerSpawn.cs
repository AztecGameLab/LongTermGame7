namespace Application.Gameplay
{
    using System.Collections.Generic;
    using Cinemachine;
    using Core;
    using UniRx;
    using UnityEngine;

    /// <summary>
    /// Loads the selected members of the player team into the overworld.
    /// </summary>
    public class PlayerSpawn : MonoBehaviour
    {
        private readonly Dictionary<TeamMemberData, TeamMemberWorldView> _memberViewLookup =
            new Dictionary<TeamMemberData, TeamMemberWorldView>();

        private readonly List<TeamMemberWorldView> _worldViewList =
            new List<TeamMemberWorldView>();

        [SerializeField]
        private CinemachineVirtualCamera playerCameraPrefab;

        /// <summary>
        /// Gets the logic that makes the selected team follow behind the player.
        /// </summary>
        public GroupFollowTarget MonsterFollowPlayer { get; } = new GroupFollowTarget(3);

        /// <summary>
        /// Gets a list of every spawned player team member in the scene.
        /// </summary>
        public IReadOnlyCollection<TeamMemberWorldView> SpawnedMembers => _worldViewList;

        /// <summary>
        /// Gets the currently spawned player object in the scene.
        /// </summary>
        public TeamMemberWorldView SpawnedPlayer { get; private set; }

        /// <summary>
        /// Creates GameObject instances of the player and their currently selected team.
        /// </summary>
        public void Spawn()
        {
            IReadOnlyReactiveCollection<TeamMemberData> selectedMembers = Services.PlayerTeamData.SelectedMembers;
            Observable.EveryUpdate().Subscribe(_ => MonsterFollowPlayer.Tick()).AddTo(this);

            // Spawn all initial members.
            foreach (var member in selectedMembers)
            {
                SpawnWorldView(member);
            }

            SpawnedPlayer = Services.PlayerTeamData.Player.CreateWorldView();
            SpawnedPlayer.transform.position = transform.position;
            CinemachineVirtualCamera playerCamera = Instantiate(playerCameraPrefab, transform);
            playerCamera.Follow = SpawnedPlayer.transform;

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

        private void SpawnWorldView(TeamMemberData member)
        {
            var instance = member.CreateWorldView();
            instance.transform.position = transform.position;

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

using Cinemachine;

namespace Application.Gameplay
{
    using System.Collections.Generic;
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

        [SerializeField]
        private float followSpacing = 2;

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
            for (int i = 0; i < selectedMembers.Count; i++)
            {
                SpawnWorldView(selectedMembers[i]);
                _memberViewLookup[selectedMembers[i]].transform.position += Vector3.left * (followSpacing * (i + 1));
            }

            SpawnedPlayer = Services.PlayerTeamData.Player.CreateWorldView();
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

        // private void Update()
        // {
        //     if (followTarget != null)
        //     {
        //         for (int i = 0; i < _worldViewList.Count; i++)
        //         {
        //             _worldViewList[i].transform.position =
        //                 followTarget.position + (Vector3.left * (followSpacing * (i + 1)));
        //         }
        //     }
        // }

        private void SpawnWorldView(TeamMemberData member)
        {
            var instance = member.CreateWorldView();

            _worldViewList.Add(instance);
            _memberViewLookup.Add(member, instance);
        }

        private void DestroyWorldView(TeamMemberData member)
        {
            _worldViewList.Remove(_memberViewLookup[member]);
            Destroy(_memberViewLookup[member].gameObject);
            _memberViewLookup.Remove(member);
        }
    }
}

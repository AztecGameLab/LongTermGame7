// using Application.Core;
// using UnityEngine;
//
// public class LevelEntrance : MonoBehaviour
// {
//     // has an id
// }
//
// public class LevelExit : MonoBehaviour
// {
//     // When a player steps inside a trigger with this component, fire a levelChangeEvent.
//     public void FireEvent()
//     {
//         Services.EventBus.Invoke(new LevelChangeEvent{sceneName = ...., entranceId = ....});
//     }
// }
//
// public class LevelChangeEvent
// {
//     public string sceneName;
//     public string entranceId;
// }
//
// public class LevelLoader
// {
//     public LevelLoader()
//     {
//         Services.EventBus.AddListener(HandleEventChange, "Level Loader");
//     }
//
//     private void HandleLevelChange()
//     {
//         
//     }
// }
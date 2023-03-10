namespace Application.Gameplay.Combat
{
    using UnityEngine;

    /// <summary>
    /// Provides a reusable API for converting mouse / screen aiming into world coordinates.
    /// </summary>
    public class AimSystem
    {
        private const int BufferSize = 50;
        private const int MaxGroundAngle = 45;


        // If we are single-threaded, we could statically reuse the buffer.
        // Might be a premature optimization though, so leave it like this for now.
        private readonly RaycastHit[] _resultBuffer;

        private Camera _camera;
        private int _layerMask;
        private bool _groundSnap;

        /// <summary>
        /// Initializes a new instance of the <see cref="AimSystem"/> class.
        /// </summary>
        /// <param name="layerMask">The layers that should be included when aiming.</param>
        /// <param name="groundSnap">Should the aim snap to the group, or be able to target walls. </param>
        public AimSystem()
        {
            _resultBuffer = new RaycastHit[BufferSize];
        }

        /// <summary>
        /// Sets up this instance with a camera to raycast from.
        /// </summary>
        /// <param name="camera">The camera to be used for ray-casting.</param>
        public void Initialize(Camera camera = null, int layerMask = -1, bool groundSnap = true)
        {
            if (camera == null)
            {
                _camera = Camera.main;
            }
            else
            {
                _camera = camera;
            }

            if (layerMask == -1)
            {
                _layerMask = ~LayerMask.GetMask("Ignore Raycast");
            }
            else
            {
                _layerMask = layerMask;
            }

            _groundSnap = groundSnap;
        }

        /// <summary>
        /// Calculate where in the world the user is aiming at on the screen.
        /// </summary>
        /// <param name="screenPosition">The X and Y screen coordinates to raycast from. For example, Input.MousePosition.</param>
        /// <returns>The hit information in the world where the raycast ended.</returns>
        public RaycastHit Update(Vector2 screenPosition)
        {
            Ray ray = _camera.ScreenPointToRay(screenPosition);
            int hits = Physics.RaycastNonAlloc(ray, _resultBuffer, float.PositiveInfinity, _layerMask, QueryTriggerInteraction.Ignore);
            RaycastHit nearest = GetNearest(_resultBuffer, hits);

            if (_groundSnap && Vector3.Angle(nearest.normal, Vector3.up) > MaxGroundAngle)
            {
                return SnapToGround(nearest.point);
            }

            return nearest;
        }

        /// <summary>
        /// Calculate where in the world the user is aiming at on the screen.
        /// Uses Input.MousePosition as the coordinates to raycast from.
        /// </summary>
        /// <returns>The hit information in the world where the raycast ended.</returns>
        public RaycastHit Update()
        {
            return Update(Input.mousePosition);
        }

        private static RaycastHit GetNearest(RaycastHit[] hitBuffer, int hitCount)
        {
            if (hitCount == 1)
            {
                return hitBuffer[0];
            }
            else if (hitCount > 1)
            {
                RaycastHit nearest = hitBuffer[0];

                for (int i = 1; i < hitCount; i++)
                {
                    if (hitBuffer[i].distance < nearest.distance)
                    {
                        nearest = hitBuffer[i];
                    }
                }

                return nearest;
            }
            else
            {
                // We didn't hit anything, so return a default value.
                return default;
            }
        }

        private RaycastHit SnapToGround(Vector3 point)
        {
            int hits = Physics.RaycastNonAlloc(new Ray(point, Vector3.down), _resultBuffer, float.PositiveInfinity, _layerMask, QueryTriggerInteraction.Ignore);
            return GetNearest(_resultBuffer, hits);
        }
    }
}

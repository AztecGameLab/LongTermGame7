namespace Application.Gameplay.Dialogue
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class IndicatorPopup : MonoBehaviour
    {
        public GameObject indic;
        private Dictionary<Collider, GameObject> activeIndics = new Dictionary<Collider, GameObject>();

        private void OnTriggerEnter(Collider other)
        {
            GameObject indicator = Instantiate(indic);

            indicator.transform.position = new Vector3(other.transform.position.x, 
                                            other.transform.position.y + 5f, other.transform.position.z);

            activeIndics.Add(other, indicator);
        }

        private void OnTriggerExit(Collider other)
        {
            //if (activeIndics.TryGetValue(other, out indicator))
            //{
            //    activeIndics.Remove(other);
            //    Destroy(indicator);
            //}

            GameObject indicator = activeIndics[other];
            Destroy(indicator);
            activeIndics.Remove(other);
        }
    }
}

using UnityEngine;

namespace FCS
{
    public class Environment : MonoBehaviour
    {
        void OnTriggerEnter(Collider col)
        {
            var ability = col.gameObject.GetComponent<Ability>();
            if (ability != null)
            {
                ability.OnCollideWithEnvironment(this);
            }
        }
    }
}
using FCS.Abilities;
using UnityEngine;

namespace FCS
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
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
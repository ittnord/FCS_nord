﻿using UnityEngine;
using UnityEngine.Networking;

namespace FCS.Abilities
{
    public class FireBall : Ability 
    {
        public int Damage = 15;
        public float ExplosionDistance = 10f;
        public  float ExplosionSpeed = 25f;

        public float InnerRadius = 5f;
        public float MaxRadius = 10f;


        [ServerCallback]
        public override void OnMaxDistance()
        {
            Explode(false);
        }

        [ServerCallback]
        public override void OnCollideWithEnvironment(Environment env)
        {
            Explode(false);
        }

        [ServerCallback]
        public override void OnCollideWithCharacter(CharacterBehaviour character)
        {
            if (character == Caster && Distance < 1)
            {
                return;
            }

            Explode(false);
        }

        protected void Explode(bool ignoreSelf)
        {
            PlayEffect();

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, MaxRadius);
            foreach (Collider collider in hitColliders)
            {
                var character = collider.GetComponent<CharacterBehaviour>();
                if (character != null)
                {
                    if (ignoreSelf && character == Caster)
                    {
                        continue;
                    }
                    

                    var distance = Vector3.Distance(transform.position, collider.transform.position);
                    var direction = (collider.transform.position - transform.position).normalized;
//                    character.CmdAddMoveEffect(direction, distance, this.InnerRadius, Damage, ExplosionDistance, ExplosionSpeed, MaxRadius);
                    character.AddMoveEffect(direction, distance, this.InnerRadius, Damage, ExplosionDistance, ExplosionSpeed, MaxRadius);
                }
            }

            Destroy(gameObject);
        }

        private void PlayEffect()
        {
            if (ImpactEffect == null)
            {
                Debug.LogWarning("No explosion effect!");
                return;
            }

            var effect = Instantiate(ImpactEffect, transform.position, Quaternion.identity);
            NetworkServer.Spawn(effect.gameObject);
        }

    }
}

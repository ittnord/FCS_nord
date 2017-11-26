using System;
using System.Collections.Generic;
using Character;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.ThirdPerson;

namespace FCS
{
    public class CharacterBehaviour : ThirdPersonCharacter
    {
        private CharacterHealth _characterHealth;
        private CharacterHealth Health => _characterHealth ?? (_characterHealth = GetComponent<CharacterHealth>());
      
        [SerializeField]
        private Transform _shieldTransform;


        public Transform Shield { get { return _shieldTransform; } }

        private readonly HashSet<IStat> _stats = new HashSet<IStat>();

        public event Action<IStat> OnStatChanged;
        public event Action<IStat> OnStatDie;

        public LineRenderer LineRenderer;
        public Transform Sights;

        protected override void Start()
        {
            base.Start();
            if (isLocalPlayer && LineRenderer != null)
            {
                LineRenderer.enabled = true;
            }
        }

        protected void Update()
        {
            if (isLocalPlayer)
            {
                LineRenderer.SetPosition(0, new Vector3(transform.position.x, transform.position.y-0.05f, transform.position.z));
                LineRenderer.SetPosition(1, Sights.position);
            }
        }

        public void Change(StatType type, int value)
        {
            Health.Damage(type, value);
        }

        private void OnTriggerEnter(Collider col)
        {
            var ability = col.gameObject.GetComponent<Ability>();
            if (ability != null)
            {
                ability.OnCollideWithCharacter(this);
            }
        }

        public void SetPosition(Vector3 position)
        {
            GetComponent<CharacterMovement>().SetPosition(position);
        }

        public void AddMoveEffect(Vector3 direction, float distance, float InnerRadius, int Damage, float ExplosionDistance, float ExplosionSpeed, float MaxRadius)
        {
            if (isLocalPlayer)
            {
                var move = gameObject.AddComponent<MoveEffect>();

                if (distance <= InnerRadius)
                {
                    Change(StatType.Hp, -Damage);
                    move.Init(direction, ExplosionSpeed, ExplosionDistance);

                }
                else
                {
                    Change(StatType.Hp, (int) (1 - distance / MaxRadius) * -Damage);
                    move.Init(direction, ExplosionSpeed, (int) (1 - distance / MaxRadius) * ExplosionDistance);
                }
            }
            else
            {
                RpcAddMoveEffect(direction, distance, InnerRadius, Damage, ExplosionDistance, ExplosionSpeed, MaxRadius);
            }
            
        }

        [ClientRpc]
        public void RpcAddMoveEffect(Vector3 direction, float distance, float InnerRadius, int Damage, float ExplosionDistance, float ExplosionSpeed, float MaxRadius)
        {
            var move = gameObject.AddComponent<MoveEffect>();

            if (distance <= InnerRadius)
            {
                Change(StatType.Hp, -Damage);
                move.Init(direction, ExplosionSpeed, ExplosionDistance);

            }
            else
            {
                Change(StatType.Hp, (int) (1 - distance / MaxRadius) * -Damage);
                move.Init(direction, ExplosionSpeed, (int) (1 - distance / MaxRadius) * ExplosionDistance);
            }
        }
//
//        [Command]
//        public void CmdAddMoveEffect(Vector3 direction, float distance, float InnerRadius, int Damage, float ExplosionDistance, float ExplosionSpeed, float MaxRadius)
//        {
//            var move = gameObject.AddComponent<MoveEffect>();
//
//            if (distance <= InnerRadius)
//            {
//                Change(StatType.Hp, -Damage);
//                move.Init(direction, ExplosionSpeed, ExplosionDistance);
//
//            }
//            else
//            {
//                Change(StatType.Hp, (int) (1 - distance / MaxRadius) * -Damage);
//                move.Init(direction, ExplosionSpeed, (int) (1 - distance / MaxRadius) * ExplosionDistance);
//            }
//        }
    }
}
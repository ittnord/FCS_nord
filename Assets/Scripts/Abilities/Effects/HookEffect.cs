using UnityEngine;
using System.Collections;

namespace FCS
{
    public class HookEffect : MonoBehaviour
    {
        private LineRenderer _lineRenderer;
        private Transform _hookOrigin;
        private Transform _hookTarget;

        private float _hookDuration;
        private float _hookSpeed;

        public void Init(Transform hookOrigin, Transform hookTarget, float hookSpeed, float hookDuration, Material hookMaterial)
        {
            var material = (Material)Resources.Load("Assets/Materials/Chain");

            _hookOrigin = hookOrigin;
            _hookTarget = hookTarget;

            _hookSpeed = hookSpeed;
            _hookDuration = hookDuration;

            gameObject.AddComponent<LineRenderer>();
            _lineRenderer = GetComponent<LineRenderer>();

            _lineRenderer.SetWidth(0.1f, 0.1f);
            _lineRenderer.SetPosition(0, _hookOrigin.transform.position);
            _lineRenderer.SetPosition(1, hookTarget.transform.position);
            _lineRenderer.material = hookMaterial;
            _lineRenderer.textureMode = LineTextureMode.Tile;
        }

        protected void Update()
        {
            if (_lineRenderer != null)
            {
                _lineRenderer.SetPosition(0, new Vector3(_hookOrigin.transform.position.x, 1, _hookOrigin.transform.position.z));
                _lineRenderer.SetPosition(1, new Vector3(_hookTarget.transform.position.x, 1, _hookTarget.transform.position.z));
            }

            var direction = (_hookOrigin.transform.position - _hookTarget.position).normalized;

            //_moveDistance += _moveSpeed * Time.fixedDeltaTime;
            Vector3 v = direction * _hookSpeed * Time.fixedDeltaTime;
            //transform.Translate(v);
            transform.position += v;

            if (Vector3.Distance(_hookOrigin.position, _hookTarget.position) <= 2f)
            {
                Destroy(_lineRenderer);
                Destroy(this);
            }
        }

    }
}
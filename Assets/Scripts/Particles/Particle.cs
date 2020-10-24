using System.Collections;
using enjoythevibes.Managers;
using UnityEngine;

namespace enjoythevibes.Particles
{
    public class Particle : MonoBehaviour
    {
        [SerializeField]
        private float delayTime = 1f;
        [SerializeField]
        private string poolTagName = default;
        private ParticleSystem particle;

        private void Awake()
        {
            particle = GetComponent<ParticleSystem>();
            particle.Play();
        }

        public void PlayParticle()
        {
            particle.Play();
            StartCoroutine(DestroyDelayed());
        }

        private IEnumerator DestroyDelayed()
        {
            yield return new WaitForSeconds(delayTime);
            particle.Stop();
            PoolsManager.GetGameObjectsPool(poolTagName).Put(gameObject);
        }
    }
}
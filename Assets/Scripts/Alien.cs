using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Alien : MonoBehaviour
{
    public UnityEvent OnDestroy;
    public Transform target;

    public float navigationUpdate;
    private float navigationTime = 0;

    public Rigidbody head;
    public bool isAlive = true;

    private DeathParticles deathParticles;

    public DeathParticles GetDeathParticles()
    {
        if (deathParticles == null)
        {
            deathParticles = GetComponentInChildren<DeathParticles>();
        }
        return deathParticles;
    }

    public void Die()
    {
        isAlive = false;
        head.GetComponent<Animator>().enabled = false;
        head.isKinematic = false;
        head.useGravity = true;
        head.GetComponent<SphereCollider>().enabled = true;
        head.gameObject.transform.parent = null;
        head.velocity = new Vector3(0, 26.0f, 3.0f);

        OnDestroy.Invoke();
        OnDestroy.RemoveAllListeners();
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDeath);

        // head.GetComponent<SelfDestruct>().Initiate(); son para borrar cadaveres en 3 sec pero no me deja destruir los objetos pag 225

        if (deathParticles)
        {
            deathParticles.transform.parent = null;
            deathParticles.Activate();
        }

        Destroy(gameObject);
    }

    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {

            if (target != null)
            {
            navigationTime += Time.deltaTime;
               if (navigationTime > navigationUpdate)
               {
                agent.destination = target.position;
                navigationTime = 0;
               }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isAlive)
        {
            Die();
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDeath);
        }
    }

  

   

}

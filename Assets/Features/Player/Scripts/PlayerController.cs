using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//[RequireComponent(typeof(AttackAbility))]
//[RequireComponent(typeof(BlockAbility))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour {

    
    private Vector2 movementDirection = new Vector2(0,0);
    private NavMeshAgent agent;
    private TopDownInput controls;
    private Vector3 attentionDirection = new Vector3(0,0,1);
    [SerializeField]
    public AudioClip[] dirtClips;
    public AudioClip[] metalClips;
    [SerializeField]
    public AudioClip clip;
    [SerializeField]
    private AudioSource source;


   [Header("References")]
    [SerializeField] public Animator animator;


    [Header("General Parameters")]

    [SerializeField] private float speed = 8.0f;

    private int currMask;

    public OrreryAudio orrery;

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
   
	void Awake ()
    {
        agent = GetComponent<NavMeshAgent>();
        controls = new TopDownInput();
        source = GetComponent<AudioSource>();
    }
	
	void Update ()
    {
        
        // Fetch control Input.
        movementDirection = controls.Character.Move.ReadValue<Vector2>();

        // Update velocity.
        agent.velocity = new Vector3(movementDirection.x, 0, movementDirection.y) * speed ;


        // Update animator with input specific
        animator.SetFloat("velocity", agent.velocity.magnitude);
        animator.SetBool("isMoving", agent.velocity.magnitude > 0.3f);

        
        

    }

    void FixedUpdate()
    {
        StartCoroutine(determineGroundandPlayAudio());
    }

    IEnumerator determineGroundandPlayAudio()
    {
       

        if (!(this.animator.GetCurrentAnimatorStateInfo(0).IsName("Move")))
        {
            source.volume = 0f;
            yield return null;
        }
        while (source.isPlaying)
        {
            yield return null;
        }
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Move"))
        {
            int betong_mask = LayerMask.GetMask("Betong");
            int dirt_mask = LayerMask.GetMask("Dirt");
            int orrery_mask = LayerMask.GetMask("Orrery");
            if (Physics.Raycast(transform.position, Vector3.down, 0.17f, dirt_mask))
            {
                if (currMask == orrery_mask)
                {
                    orrery.LeaveOrrery();
                }
                currMask = dirt_mask;
                source.volume = 0.02f;
                clip = dirtClips[UnityEngine.Random.Range(0, dirtClips.Length)];
                source.pitch = 0.8f;
                source.PlayOneShot(clip);
                
                //yield return new WaitForSeconds(0.2f);
            }
            else if (Physics.Raycast(transform.position, Vector3.down, 0.17f, betong_mask))
            {
                if (currMask == orrery_mask)
                {
                    orrery.LeaveOrrery();
                }
                currMask = betong_mask;
                source.volume = 0.05f;
                clip = metalClips[UnityEngine.Random.Range(0, metalClips.Length)];
                source.pitch = 1.4f;
                source.PlayOneShot(clip);
            }
            else if (Physics.Raycast(transform.position, Vector3.down, 0.17f, orrery_mask))
            {
                if (currMask == dirt_mask || currMask == betong_mask)
                {
                    orrery.EnterOrrery();
                }
                currMask = orrery_mask;
                source.volume = 0.05f;
                clip = metalClips[UnityEngine.Random.Range(0, metalClips.Length)];
                source.pitch = 1.4f;
                source.PlayOneShot(clip);
            }
        }

    }

}
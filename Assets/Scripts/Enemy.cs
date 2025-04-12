using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MyStates
{
    WANDER,
    PURSUE,
    ATTACK,
    RECOVER
}
public class Enemy : MonoBehaviour
{
    [SerializeField] MyStates m_States;

    GameObject player;
    private Animator anim;
    public float speed;
    int randomDir;
    private bool chooseDir = false;

    float wanderRange;
    Vector3 startingLocation;
    float playerSightRange;
    float playerAttackRange;
    float currentStateElapsed;
    float recoveryTime = 30;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();      
    }

    // Update is called once per frame
    void Update()
    {
        switch(m_States)
        {
            case MyStates.WANDER:
                UpdateWander();
                break;
            case MyStates.PURSUE:
                UpdatePursue();
                break; 
            case MyStates.ATTACK:
                UpdateAttack();
                break; 
            case MyStates.RECOVER:
                UpdateRecover();
                break; 
        }
        
    }

    private bool IsPlayerInSightRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) <= playerSightRange;
    }

    private bool IsPlayerInAttackRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) <= playerAttackRange;
    }

    private IEnumerator ChooseDirection()
    {
        chooseDir = true;
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        randomDir = Random.Range(1, 8);
        chooseDir = false;
    }

    void UpdateWander()
    {
        if (!chooseDir)
        {
            StartCoroutine(ChooseDirection());
        }

        if (randomDir == 1)
        {
            anim.SetBool("IsRunningLeft", false);
            anim.SetBool("IsRunningRight", true);
            transform.position += transform.right * speed * Time.deltaTime;

        }
        if (randomDir == 2)
        {
            anim.SetBool("IsRunningRight", false);
            anim.SetBool("IsRunningLeft", true);
            transform.position += -transform.right * speed * Time.deltaTime;
        }
        if (randomDir == 3)
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }
        if (randomDir == 4)
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }
        if (randomDir == 5)
        {
            transform.position += transform.right * speed * Time.deltaTime;
            transform.position += transform.up * speed * Time.deltaTime;
        }
        if (randomDir == 6)
        {
            transform.position += transform.right * speed * Time.deltaTime;
            transform.position += -transform.up * speed * Time.deltaTime;
        }
        if (randomDir == 7)
        {
            transform.position += -transform.right * speed * Time.deltaTime;
            transform.position += transform.up * speed * Time.deltaTime;
        }
        if (randomDir == 8)
        {
            transform.position += -transform.right * speed * Time.deltaTime;
            transform.position += -transform.up * speed * Time.deltaTime;
        }

        if (IsPlayerInSightRange())
        {
            UpdatePursue();
        }
    }
    
    void UpdatePursue()
    {
        currentStateElapsed += Time.deltaTime;
        if (IsPlayerInAttackRange())
        {
            UpdateAttack();
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime); 
        }
    }
    
    void UpdateAttack()
    {
        currentStateElapsed += Time.deltaTime;
        GetComponent<Rigidbody>().AddForce(0, 20,0);
        UpdateRecover();
    }

    void UpdateRecover()
    {
        currentStateElapsed += Time.deltaTime;
        if (recoveryTime < currentStateElapsed)
        {
            UpdateWander();
        }
    }
}

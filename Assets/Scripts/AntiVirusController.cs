using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthHealper))]
public class AntiVirusController : EntityController, IMovable, IAttacker
{
    void Start()
    {
        ground = GameObject.Find("Ground");
        Speed = 5f;
        timeoutAttack = 0.3f;
        
        EntityState = State.SearchTarget;
        //Время жизни антивирусной клетки
        Destroy(this.gameObject, 30);
    }

    // Update is called once per frame
    void Update()
    {
        RotateObject(this.transform, 200);
        ChangeState();
    }

    public void ChangeState()
    {
        switch (EntityState)
        {
            case State.SearchTarget:
                {
                    SearchTarget();
                    break;
                }
            case State.FoundTarget:
                {
                    Move();
                    break;
                }
            case State.AttackTarget:
                {
                    Attack();
                    break;
                }
        }
    }

    /// <summary>
    /// Передвижение антивируса
    /// </summary>
    public void Move()
    {
        //Если цель пуста ищем новую
        if (!Target)
        {
            EntityState = State.SearchTarget;
            return;
        }

        if (Target && Vector3.Distance(this.transform.position, Target.position) >= 2)
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Time.deltaTime * Speed * 5);
        else
            EntityState = State.AttackTarget;
    }
    /// <summary>
    /// Атака по вирусу
    /// </summary>
    public void Attack()
    {
        //Если цель пуста ищем новую
        if (!Target)
        {
            EntityState = State.SearchTarget;
            return;
        }

        curTimeout += Time.deltaTime;


        if (Vector3.Distance(this.transform.position, Target.position) < 2)
        {
            if (curTimeout > timeoutAttack)
            {
                Target.gameObject.GetComponent<HealthHealper>().SetDamage(25);
                curTimeout = 0.0f;
            }
        }
        else
            EntityState = State.FoundTarget;
    }

    /// <summary>
    /// Поиск вируса
    /// </summary>
    public void SearchTarget()
    {
        FindClosestTarget(this.gameObject, "Virus");

        if (Vector3.Distance(transform.position, FreeTarget) < 1 || FreeTarget == new Vector3(0, 0, 0))
            FreeTarget = GetRandomOriginGround();


        transform.position = Vector3.MoveTowards(transform.position, FreeTarget, Time.deltaTime * Speed);
    }
}

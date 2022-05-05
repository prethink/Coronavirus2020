using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthHealper))]
public class VirusController : EntityController, IMovable, IAttacker
{
    /// <summary>
    /// Количество мертвых вирусов
    /// </summary>
    public static int DeadVirus;

    void Start()
    {
        ground = GameObject.Find("Ground");
        Speed = 10f;
        Damage = 10;
        timeoutAttack = 0.5f;
        EntityState = State.SearchTarget;

        //Подписка на события
        GetComponent<HealthHealper>().OnDamage += OnDamage;
        GetComponent<HealthHealper>().OnDead += OnDead;

        //Время жизни вируса 60 секунд
        Destroy(this.gameObject, 60);
    }

    /// <summary>
    /// Событие получения урона
    /// </summary>
    /// <param name="dmg">количество урона</param>
    private void OnDamage(int dmg)
    {
        //Меняем цвет вируса если получил урон
        GetComponent<Renderer>().material.color = new Color(255, 0, 0, 1);
    }

    /// <summary>
    /// Событие смерти
    /// </summary>
    private void OnDead()
    {
        DeadVirus++;
    }

    void Update()
    {
        RotateObject(this.transform, 360);
        ChangeState();
    }

    /// <summary>
    /// Смена состояния объекта
    /// </summary>
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
    /// Рисует линию между клеткой и вирусом
    /// </summary>
    void OnDrawGizmos()
    {
        if (Target)
            Gizmos.DrawLine(this.gameObject.transform.position, Target.transform.position);
    }

    /// <summary>
    /// Передвижение вируса
    /// </summary>
    public void Move()
    {
        if (Target && Vector3.Distance(this.transform.position, Target.position) < 2)
            EntityState = State.AttackTarget;

        if (Target || Target && Vector3.Distance(this.transform.position, Target.position) >= 2)
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Time.deltaTime * Speed);

        if (!Target)
            EntityState = State.SearchTarget;
    }

    /// <summary>
    /// Атака вируса на клетку
    /// </summary>
    public void Attack()
    {
        //Если цель пустая, ищем новую цель
        if (!Target)
        {
            EntityState = State.SearchTarget;
            return;
        }

        curTimeout += Time.deltaTime;


        if (Vector3.Distance(this.transform.position, Target.position) < 2)
        {
            var tx = Time.time * 5;
            int amp = 2;
            //Когда вирус атакует клетку меняем его размер
            transform.localScale = new Vector3(Mathf.Sin(tx) * amp, Mathf.Sin(tx) * amp, Mathf.Sin(tx) * amp);
            if (curTimeout > timeoutAttack)
            {
                Target.gameObject.GetComponent<HealthHealper>().SetDamage(Damage);
                curTimeout = 0.0f;
            }
            EntityState = State.AttackTarget;
        }
        else if (Target)
            EntityState = State.FoundTarget;
        else
            EntityState = State.SearchTarget;
    }

    /// <summary>
    /// Поиск ближайшей клетки для атаки
    /// </summary>
    public void SearchTarget()
    {
        FindClosestTarget(this.gameObject, "Cell");

        if (Vector3.Distance(transform.position, FreeTarget) < 1 || FreeTarget == new Vector3(0, 0, 0))
            FreeTarget = GetRandomOriginGround();

        transform.position = Vector3.MoveTowards(transform.position, FreeTarget, Time.deltaTime * Speed);
        transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * Speed);
    }
}

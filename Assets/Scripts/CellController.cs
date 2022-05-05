using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthHealper))]
public class CellController : EntityController, IMovable
{
    //Всего мертвых клеток
    public static int DeadCells;
    
    private void Start()
    {
        Speed = 3f;
        ground = GameObject.Find("Ground");

        //Подписываемся на события получения урона и смерти
        GetComponent<HealthHealper>().OnDamage += OnDamage;
        GetComponent<HealthHealper>().OnDead += OnDead;
    }

    //Событие смерти
    private void OnDead()
    {
        DeadCells++;
        SpawnVirus();
        SpawnAntivirus();
    }
    /// <summary>
    /// Событие получение урона
    /// </summary>
    /// <param name="dmg">количество урона</param>
    private void OnDamage(int dmg)
    {
        //Останавливаем клетку
        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        //Меняем цвет клетки
        GetComponent<Renderer>().material.color = new Color(0, 0, 255, 1);
    }

    /// <summary>
    /// Спавн вируса на месте смерти клетки
    /// </summary>
    public void SpawnVirus()
    {
        var attacker = gameObject.GetComponent<EntityController>().Attacker;
        if (attacker != null)
        {
            attacker.GetComponent<EntityController>().Mutation((Instantiate(attacker, transform.position, Quaternion.identity)));
        }
    }
    /// <summary>
    /// Спавн антивирусной клетки
    /// </summary>
    public void SpawnAntivirus()
    {
        if (HaveNeedProcentDead(5))
        {
            //шанс спавна клетки 30%
            bool CanSpawnAntiVirus = UnityEngine.Random.Range(0.0f, 10.0f) < 3;
            if (CanSpawnAntiVirus)
            {
                var AntiVirus = Resources.Load("Prefabs/AntiVirus") as GameObject;
                Spawner.SpanwEntity(AntiVirus, ground);
            }
        }
    }

    /// <summary>
    /// Проверка что нужный процент клеток уничтожен
    /// </summary>
    /// <param name="needProcent">Требуемый процент</param>
    /// <returns>true/false</returns>
    public bool HaveNeedProcentDead(int needProcent)
    {
        return ((Data.StartCells / 100) * needProcent) >= DeadCells;
    }

    void Update()
    {
        RotateObject(this.transform, 160);

        if (Attacker == null)
        {
            //Позволяем клетке снова двигается, если она была заморожена
            if(this.gameObject.GetComponent<Rigidbody>().isKinematic)
                this.gameObject.GetComponent<Rigidbody>().isKinematic = false;

            //Если дошли до нужной точки ищем новую
            if (FreeTarget == new Vector3(0, 0, 0))
                FreeTarget = GetRandomOriginGround();
            else
                Move();
        }
    }
    //Передвижение клетки в свободную точку на локации
    public void Move()
    {
        if (Vector3.Distance(transform.position, FreeTarget) < 1)
            FreeTarget = GetRandomOriginGround();

        transform.position = Vector3.MoveTowards(transform.position, FreeTarget, Time.deltaTime * Speed);
    }
}

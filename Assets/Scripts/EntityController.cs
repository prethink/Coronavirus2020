using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    /// <summary>
    /// Таймаут между атаками
    /// </summary>
    internal float timeoutAttack;
    /// <summary>
    /// текущее время между атаками
    /// </summary>
    internal float curTimeout;
    /// <summary>
    /// Локация
    /// </summary>
    internal GameObject ground;
    /// <summary>
    /// Найденная цель
    /// </summary>
    public Transform Target;
    /// <summary>
    /// Сущность которая атакует текущую сущность
    /// </summary>
    public GameObject Attacker;
    /// <summary>
    /// Скорость сущности
    /// </summary>
    public float Speed;
    /// <summary>
    /// Наносимый урон
    /// </summary>
    public int Damage;
    /// <summary>
    /// Версия мутации
    /// </summary>
    public int Version;
    /// <summary>
    /// Точка передвижения объекта
    /// </summary>
    public Vector3 FreeTarget;
    /// <summary>
    /// Состояние сущности
    /// </summary>
    public State EntityState;
    
    
    /// <summary>
    /// Состояние сущностей
    /// </summary>
    public enum State
    {
        SearchTarget,
        FoundTarget,
        AttackTarget,
        Escape,
        FreeRide
    }

    /// <summary>
    /// Повернуть объект
    /// </summary>
    /// <param name="ts">положение объекта</param>
    /// <param name="speed">скорость</param>
    public void RotateObject(Transform ts, int speed)
    {
        ts.Rotate(Vector3.right * Time.deltaTime * speed);
        ts.Rotate(Vector3.up * Time.deltaTime * speed); 
    }

    /// <summary>
    /// Получение случайной позиции на локации
    /// </summary>
    /// <returns>позиция</returns>
    public Vector3 GetRandomOriginGround()
    {
        Vector3 Pos = ground.transform.position;
        Vector3 LocScale = ground.transform.localScale;
        Vector3 NewPos = new Vector3(Pos.x + (Random.Range(-(LocScale.x / 2), LocScale.x / 2)), Pos.y + 1, Pos.z + (Random.Range(-(LocScale.z / 2), LocScale.z / 2)));

        return NewPos;
    }

    /// <summary>
    /// Мутация объекта
    /// </summary>
    /// <param name="gameObject">Игровой объект</param>
    public virtual void Mutation(GameObject gameObject)
    {
        var rand = Random.Range(0, 100);
        //шанс мутации 5 процентов
        if (rand < 5)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value, 1);
            gameObject.GetComponent<EntityController>().Speed += Random.Range(0, 100) > 50 ? 15.0f : 5.0f;
            gameObject.GetComponent<EntityController>().Damage += Random.Range(0, 100) > 50 ? 6 : 3;
            gameObject.GetComponent<EntityController>().Version++;
            gameObject.GetComponent<HealthHealper>().AddHealth(Random.Range(0, 100) > 50 ? 50 : 25);
        }
    }

    /// <summary>
    /// Поиск ближайшего объекта определенного типа
    /// </summary>
    /// <param name="gm">Кто ищет</param>
    /// <param name="tagTarget">тип нужного объекта</param>
    public virtual void FindClosestTarget(GameObject gm, string tagTarget)
    {
        //Если цель уже найдена, прекратить поиск
        if (Target)
            return;

        Collider[] col = Physics.OverlapSphere(gm.transform.position, 20);
        if (col.Length < 1)
            return;

        //максимальная дистанция поиска
        float distance = 1000.0f;
        GameObject Temp = null;

        //Поиск ближайшего объекта
        foreach (var item in col)
        {
            if (item.gameObject.CompareTag(tagTarget))
            {
                if (Vector3.Distance(this.transform.position, item.transform.position) < distance)
                {
                    if (item.gameObject.GetComponent<EntityController>().Attacker == null)
                    {
                        distance = Vector3.Distance(this.transform.position, item.transform.position);
                        Temp = item.gameObject;
                        break;
                    }
                }
            }
        }

        //Если цель найдена меняем состояние
        if (Temp != null)
        {
            EntityState = State.FoundTarget;
            Target = Temp.transform;
            Temp.gameObject.GetComponent<EntityController>().Attacker = this.gameObject;
        }
    }
}

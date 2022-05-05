using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHealper : MonoBehaviour
{
    //Делегаты для событий
    public delegate void Damage(int dmg);
    public delegate void Dead();

    /// <summary>
    /// Событие урона
    /// </summary>
    public event Damage OnDamage;
    /// <summary>
    /// Событие смерти
    /// </summary>
    public event Dead OnDead;
    /// <summary>
    /// Количество здоровья
    /// </summary>
    [SerializeField] private int Health;
    /// <summary>
    /// Проверка что объект живой
    /// </summary>
    public bool IsAlive => Health > 0;

    /// <summary>
    /// Количество жизней по умолчанию 50
    /// </summary>
    public HealthHealper()
    {
        Health = 50;
    }

    /// <summary>
    /// Добавляет 
    /// </summary>
    /// <param name="value"></param>
    public void AddHealth(int value) => Health += value;

    /// <summary>
    /// Нанесение урона по объекту
    /// </summary>
    /// <param name="dmg">количество урона</param>
    public void SetDamage(int dmg)
    {
        if (!IsAlive)
            return;

        Health -= dmg;
        //Вызов события получения урона
        OnDamage?.Invoke(dmg);
        if (Health <= 0)
        {
            //Вызов события смерти
            OnDead?.Invoke();
            //Уничтожение объекта
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Получение текущего количество здоровья объекта
    /// </summary>
    /// <returns></returns>
    public int GetHealth()
    {
        return Health;
    }
        
}

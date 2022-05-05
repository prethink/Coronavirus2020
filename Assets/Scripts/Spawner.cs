using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    /// <summary>
    /// Антивирус
    /// </summary>
    GameObject antivirus;
    /// <summary>
    /// Клетка
    /// </summary>
    GameObject cell;
    /// <summary>
    /// Вирус
    /// </summary>
    GameObject virus;
    /// <summary>
    /// Локация
    /// </summary>
    GameObject ground;

    void Start()
    {
        cell = Resources.Load("Prefabs/Cell") as GameObject;
        antivirus = Resources.Load("Prefabs/AntiVirus") as GameObject;
        virus = Resources.Load("Prefabs/COVID19") as GameObject;
        ground = GameObject.Find("Ground");

        //Спавн клеток при загрузке локации
        StartCoroutine(SpawnObject(Data.StartCells, cell));
        //Спавн антивирусных клетов при загрузке локации
        StartCoroutine(SpawnObject(Data.StartAntivirus, antivirus));
        StartCoroutine(SpawnBlood());
    }

    // Update is called once per frame
    void Update()
    {
        SpawnVirus();
    }

    /// <summary>
    /// Спавн вирус левым кликом мыши
    /// </summary>
    public void SpawnVirus()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Получение позиции луча от камеры
            RaycastHit hit = RayFromCamera(Input.mousePosition, 1000.0f);
            //Спавн объекта
            Instantiate(virus, hit.point, Quaternion.identity);
        }
    }

    /// <summary>
    /// Спавн объекта на локации в цикле
    /// </summary>
    /// <param name="count">Количество объектов</param>
    /// <param name="obj">тип объекта</param>
    /// <returns></returns>
    IEnumerator SpawnObject(int count, GameObject obj)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(0.001f);
            SpanwEntity(obj, ground);
        }
    }

    /// <summary>
    /// Запустить луч от камеры
    /// </summary>
    /// <param name="mousePosition">позиция мыши</param>
    /// <param name="rayLength">длина луча</param>
    /// <returns>результа луча</returns>
    public RaycastHit RayFromCamera(Vector3 mousePosition, float rayLength)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Physics.Raycast(ray, out hit, rayLength);
        return hit;
    }

    /// <summary>
    /// Переодически спавнит новые клетки
    /// </summary>
    IEnumerator SpawnBlood()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.3f, 2.0f));
            SpanwEntity(cell, ground);
        }
    }
    /// <summary>
    /// Спавн объекта на локации
    /// </summary>
    /// <param name="obj">Объект</param>
    /// <param name="ground">Локация</param>
    public static void SpanwEntity(GameObject obj, GameObject ground)
    {
        Vector3 position = ground.transform.position;
        Vector3 localPosition = ground.transform.localScale;
        Instantiate(obj, new Vector3(position.x + (Random.Range(-(localPosition.x / 2), localPosition.x / 2)), position.y + 1, position.z + (Random.Range(-(localPosition.z / 2), localPosition.z / 2))), Quaternion.identity);
    }
}

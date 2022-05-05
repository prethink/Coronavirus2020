using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    /// <summary>
    /// ������� ����� �������
    /// </summary>
    internal float timeoutAttack;
    /// <summary>
    /// ������� ����� ����� �������
    /// </summary>
    internal float curTimeout;
    /// <summary>
    /// �������
    /// </summary>
    internal GameObject ground;
    /// <summary>
    /// ��������� ����
    /// </summary>
    public Transform Target;
    /// <summary>
    /// �������� ������� ������� ������� ��������
    /// </summary>
    public GameObject Attacker;
    /// <summary>
    /// �������� ��������
    /// </summary>
    public float Speed;
    /// <summary>
    /// ��������� ����
    /// </summary>
    public int Damage;
    /// <summary>
    /// ������ �������
    /// </summary>
    public int Version;
    /// <summary>
    /// ����� ������������ �������
    /// </summary>
    public Vector3 FreeTarget;
    /// <summary>
    /// ��������� ��������
    /// </summary>
    public State EntityState;
    
    
    /// <summary>
    /// ��������� ���������
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
    /// ��������� ������
    /// </summary>
    /// <param name="ts">��������� �������</param>
    /// <param name="speed">��������</param>
    public void RotateObject(Transform ts, int speed)
    {
        ts.Rotate(Vector3.right * Time.deltaTime * speed);
        ts.Rotate(Vector3.up * Time.deltaTime * speed); 
    }

    /// <summary>
    /// ��������� ��������� ������� �� �������
    /// </summary>
    /// <returns>�������</returns>
    public Vector3 GetRandomOriginGround()
    {
        Vector3 Pos = ground.transform.position;
        Vector3 LocScale = ground.transform.localScale;
        Vector3 NewPos = new Vector3(Pos.x + (Random.Range(-(LocScale.x / 2), LocScale.x / 2)), Pos.y + 1, Pos.z + (Random.Range(-(LocScale.z / 2), LocScale.z / 2)));

        return NewPos;
    }

    /// <summary>
    /// ������� �������
    /// </summary>
    /// <param name="gameObject">������� ������</param>
    public virtual void Mutation(GameObject gameObject)
    {
        var rand = Random.Range(0, 100);
        //���� ������� 5 ���������
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
    /// ����� ���������� ������� ������������� ����
    /// </summary>
    /// <param name="gm">��� ����</param>
    /// <param name="tagTarget">��� ������� �������</param>
    public virtual void FindClosestTarget(GameObject gm, string tagTarget)
    {
        //���� ���� ��� �������, ���������� �����
        if (Target)
            return;

        Collider[] col = Physics.OverlapSphere(gm.transform.position, 20);
        if (col.Length < 1)
            return;

        //������������ ��������� ������
        float distance = 1000.0f;
        GameObject Temp = null;

        //����� ���������� �������
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

        //���� ���� ������� ������ ���������
        if (Temp != null)
        {
            EntityState = State.FoundTarget;
            Target = Temp.transform;
            Temp.gameObject.GetComponent<EntityController>().Attacker = this.gameObject;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public GameObject gm;
    public List<GameObject> Paths;
    // Start is called before the first frame update

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit = RayFromCamera(Input.mousePosition, 1000.0f);
            Paths.Add(Instantiate(gm, hit.point, Quaternion.identity)) ;
        }
    }

    public RaycastHit RayFromCamera(Vector3 mousePosition, float rayLength)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Physics.Raycast(ray, out hit, rayLength);
        return hit;
    }

    private void OnDrawGizmos()
    {
        if (Paths.Count < 2)
            return;

        for (int i = 1; i < Paths.Count; i++)
        {
            Gizmos.DrawLine(Paths[i].transform.position, Paths[i - 1].transform.position);
        }
    }
}

using UnityEngine;

public class DestroyFood : MonoBehaviour

{

    private Vector3 topBounds = new Vector3(30, 30, 30);
    


    void Update()
    {
        Vector3 pos = transform.position;

        if (pos.x > topBounds.x || pos.x < -topBounds.x ||
            pos.y > topBounds.y || pos.y < -topBounds.y ||
            pos.z > topBounds.z || pos.z < -topBounds.z)
        {
            Destroy(gameObject);
        }
    }
}
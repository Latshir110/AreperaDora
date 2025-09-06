using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] Transform target;            // jugador
    [SerializeField] Transform aimPoint;          // opcional: empty en pecho/cabeza
    [SerializeField] Vector3 offset = new Vector3(0, 5, -8);
    [SerializeField, Range(0.01f, 1f)] float posSmoothTime = 0.12f;
    [SerializeField, Range(0f, 20f)] float rotSmooth = 10f;
    [SerializeField] bool offsetEnEspacioDelTarget = true; // hombro detrás del jugador

    Vector3 vel;

    void LateUpdate()
    {
        if (!target) return;

        //posicion de la camamara respecto al jugador
        Vector3 desiredPos = offsetEnEspacioDelTarget ? target.TransformPoint(offset)
                                                      : target.position + offset;

        //el movimiento suave de posicion
        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref vel, posSmoothTime);

        //para que siempre mire al jugador
        Vector3 lookAt = (aimPoint ? aimPoint.position : target.position);
        Quaternion desiredRot = Quaternion.LookRotation(lookAt - transform.position);

        //movimiento suave de la rotacion de camara
        float t = 1f - Mathf.Exp(-rotSmooth * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, t);
    }
}

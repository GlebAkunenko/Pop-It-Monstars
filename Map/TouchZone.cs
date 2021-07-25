using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class TouchZone : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public new Transform camera;
    
    [SerializeField]
    private float sensorics = 15;
    private float velocity;

    private Coroutine stopping;

    public void OnDrag(PointerEventData eventData)
    {
        velocity = -eventData.delta.y * sensorics / Screen.height;
        Vector3 delta = new Vector3(0, velocity, 0);
        float y = camera.position.y + delta.y;

        if (stopping != null)
            StopCoroutine(stopping);

        if (DownBarrier.Value < y && y < UpBarrier.Value)
            camera.position += delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (stopping != null)
            StopCoroutine(stopping);
        stopping = StartCoroutine(Inertia());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        velocity = 0;
        if (stopping != null)
            StopCoroutine(stopping);
    }

    private IEnumerator Inertia()
    {
        velocity *= 0.5f;
        float step = velocity / 40;
        for(int i = 0; i < 40; i++) {

            Vector3 delta = new Vector3(0, velocity, 0);
            float y = camera.position.y + delta.y;

            if (DownBarrier.Value < y && y < UpBarrier.Value)
                camera.position += delta;

            float sing = Mathf.Sign(velocity);
            velocity -= step;
            if (sing != Mathf.Sign(velocity)) {
                velocity = 0;
                yield break;
            }

            yield return new WaitForSeconds(0.025f);
        }
    }

}

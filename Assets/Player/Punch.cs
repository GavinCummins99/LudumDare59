using UnityEngine;

public class Punch : MonoBehaviour
{
    public float PunchRange = 1.5f;

    public void DoPunch()
    {
        Vector2 Direction = new Vector2(transform.localScale.x, 0f);
        RaycastHit2D Hit = Physics2D.Raycast(transform.position, Direction, PunchRange);

        if (Hit.collider != null && Hit.collider.CompareTag("Destructable"))
            Destroy(Hit.collider.gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, new Vector3(transform.localScale.x, 0f, 0f) * PunchRange);
    }
}

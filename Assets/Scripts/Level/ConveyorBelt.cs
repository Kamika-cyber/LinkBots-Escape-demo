using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ConveyorBelt : MonoBehaviour
{
    [Header("Conveyor Settings")]
    public float speed = 2f;           
    public Vector2 direction = Vector2.right; 

    private float ConveyorXSpeed => direction.normalized.x * speed;

    private void Reset()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.isTrigger = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryApplyConveyor(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                
                rb.linearVelocity = new Vector2(speed * direction.x, rb.linearVelocity.y);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        PlayerController2D controller = collision.gameObject.GetComponent<PlayerController2D>();
        if (controller != null)
        {
            controller.ExitConveyor();
        }
    }

    private void TryApplyConveyor(Collision2D collision)
    {
        PlayerController2D controller = collision.gameObject.GetComponent<PlayerController2D>();
        if (controller == null)
            return;

        bool standingOnTop = false;

        for (int i = 0; i < collision.contactCount; i++)
        {
            ContactPoint2D contact = collision.GetContact(i);

            // Для платформы нормаль контакта сверху игрока часто будет отрицательной по Y
            if (contact.normal.y < -0.5f)
            {
                standingOnTop = true;
                break;
            }
        }

        if (!standingOnTop)
            return;

        controller.StayOnConveyor(ConveyorXSpeed);
    }
}
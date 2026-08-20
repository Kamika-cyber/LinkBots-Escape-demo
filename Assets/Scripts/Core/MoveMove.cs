using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public enum MoveDirection
    {
        Horizontal,
        Vertical
    }

    public MoveDirection moveDirection = MoveDirection.Horizontal;
    public float moveDistance = 2f;
    public float moveSpeed = 2f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;

        if (moveDirection == MoveDirection.Horizontal)
        {
            transform.position = new Vector3(startPos.x + offset, startPos.y, startPos.z);
        }
        else
        {
            transform.position = new Vector3(startPos.x, startPos.y + offset, startPos.z);
        }
    }
}
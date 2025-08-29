using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;

    public float speed = 10f;
    public float angular = 50f;
    public LayerMask floorMask;
    public Camera camera;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        float moveX = Input.GetAxis(Define.AxisHorizontal);
        float moveZ = Input.GetAxis(Define.AxisVertical);

        Vector3 movePos = new Vector3(moveX, 0, moveZ) * speed * Time.deltaTime;
        transform.Translate(movePos);

        if (moveX != 0 || moveZ != 0)
        {
            animator.SetBool(Define.ANI_PlayerMove, true);
        }
        else
        {
            animator.SetBool(Define.ANI_PlayerMove, false);
        }
    }

    private void Rotate()
    {
        RaycastHit hit;
        Vector3 mousePosition = Input.mousePosition;
        Vector3 newMousePosition = camera.ScreenToWorldPoint(new Vector3(mousePosition.x , mousePosition.y, mousePosition.y));

        Vector3 cameraPosition = camera.transform.position;

        Vector3 direction = (newMousePosition - cameraPosition).normalized;
        Debug.DrawLine(cameraPosition , direction * 100f , Color.red);

        if (Physics.Raycast(camera.transform.position, direction, out hit, float.MaxValue))
        {
            Vector3 hitPoint = new Vector3(hit.point.x, 0, hit.point.z);
            transform.LookAt(hitPoint);
        }

    }
}

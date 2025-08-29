using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;

    public float speed = 10f;
    [Range(0.01f , 1f)]
    public float angular;

    public LayerMask floorMask;
    public Camera camera;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        Rotate();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float moveX = Input.GetAxis(Define.AxisHorizontal);
        float moveZ = Input.GetAxis(Define.AxisVertical);

        Vector3 movePos = new Vector3(moveX, 0, moveZ) * speed * Time.deltaTime;
        transform.Translate(movePos);

        if (moveX != 0 || moveZ != 0)
        {
            animator.SetBool(Define.ANI_Move, true);
        }
        else
        {
            animator.SetBool(Define.ANI_Move, false);
        }
    }

    private void Rotate()
    {
        RaycastHit hit;
        Vector3 mousePosition = Input.mousePosition;
        Vector3 newMousePosition = camera.ScreenToWorldPoint(new Vector3(mousePosition.x , mousePosition.y , mousePosition.y));
        Vector3 cameraPosition = camera.transform.position;

        Vector3 direction = (newMousePosition - cameraPosition).normalized;

        if (Physics.Raycast(camera.transform.position, direction, out hit, float.MaxValue , floorMask))
        {
            Vector3 hitPoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);

            Quaternion to = Quaternion.LookRotation(hitPoint - transform.position);

            transform.rotation = Quaternion.Lerp(transform.rotation, to , angular);
        }

    }
}

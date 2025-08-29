using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;

    public float speed = 10f;

    private void Update()
    {
        float moveX = Input.GetAxis(Define.AxisHorizontal);
        float moveZ = Input.GetAxis(Define.AxisVertical);

        Vector3 movePos = new Vector3(moveX, 0, moveZ) * speed * Time.deltaTime;
        transform.Translate(movePos);
    }
}

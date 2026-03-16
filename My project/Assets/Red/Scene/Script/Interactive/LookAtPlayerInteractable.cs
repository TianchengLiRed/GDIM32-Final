using UnityEngine;

public abstract class LookAtPlayerInteractable : Interactable
{
    [SerializeField] protected Transform player;
    [SerializeField] protected float rotationSpeed = 5f;

    protected override void Awake()
    {
        base.Awake();
        TryResolvePlayer();
    }

    protected virtual void Update()
    {
        if (player == null) TryResolvePlayer();
        LookAtPlayer();
    }

    protected void LookAtPlayer()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;
        if (direction.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    protected void TryResolvePlayer()
    {
        if (player != null) return;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            return;
        }

        PlayerController controller = FindObjectOfType<PlayerController>();
        if (controller != null)
        {
            player = controller.transform;
        }
    }
}

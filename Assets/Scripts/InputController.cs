using UnityEngine;
using Zenject;
using TestWork.Units;

public class InputController : MonoBehaviour
{
    private Player _player;

    [Inject]
    public void Construct(Player player, GameManager gameManager)
    {
        _player = player;
        gameManager.OnStartGame += Enable;
        gameManager.OnLoseGame += Disable;
        gameManager.OnWinGame += Disable;
        Disable();
    }

    private void Update()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        direction = direction.normalized;
        _player.Move(direction);
    }

    private void Enable() => enabled = true;
    private void Disable() => enabled = false;
}

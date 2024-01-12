using System;
using TestWork.Units;

public class GameManager
{
    public Action OnStartGame;
    public Action OnWinGame;
    public Action OnLoseGame;

    public GameManager(Player player)
    {
        player.OnDie += LoseGame;
        OnWinGame += player.StopPlayer;
        OnStartGame += player.ActivateUnit;
        OnStartGame += player.ResetPlayer;
    }

    private void LoseGame() => OnLoseGame?.Invoke();
}

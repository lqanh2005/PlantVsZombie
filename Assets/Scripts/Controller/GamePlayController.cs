using UnityEngine;

public enum StateGame
{
    Loading = 0,
    Playing = 1,
    Win = 2,
    Lose = 3,
    Pause = 4
}

public class GamePlayController : Singleton<GamePlayController>
{
    public StateGame stateGame;
    public PlayerContain playerContain;
    public GameScene gameScene;
    protected override void OnAwake()
    {
        //  GameController.Instance.currentScene = SceneType.GamePlay;


        Init();

    }

    private void Init()
    {
        playerContain.Init();
        gameScene.Init();
    }
    private void Update()
    {
        if(playerContain.levelController.currentZombies.Count == 0)
        {
            // Check if all zombies are defeated
        }
        if(playerContain.playerHealth <= 0)
        {
            // Check if the player has lost
        }
    }
}

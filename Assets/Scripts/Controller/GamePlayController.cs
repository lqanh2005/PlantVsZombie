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

        Time.timeScale = 1f;
        Init();
    }

    private void Init()
    {
        stateGame = StateGame.Playing;
        playerContain.Init();
        gameScene.Init();
    }

    private bool isGameOver;

    private void Update()
    {
        if (isGameOver)
            return;

        LevelController levelController = playerContain.levelController;
        if (levelController != null)
            gameScene.UpdateWave(UseProfile.CurrentLevel, levelController.CurrentWave, levelController.TotalWaves);

        if (stateGame == StateGame.Lose)
        {
            isGameOver = true;
            gameScene.ShowLose();
            return;
        }

        if (levelController != null && levelController.IsFinished)
        {
            isGameOver = true;
            stateGame = StateGame.Win;
            UseProfile.UnlockLevel(UseProfile.CurrentLevel + 1);
            gameScene.ShowWin();
        }
    }

    public void Pause()
    {
        if (isGameOver || stateGame == StateGame.Pause)
            return;

        stateGame = StateGame.Pause;
        Time.timeScale = 0f;
        gameScene.SetPausePopup(true);
    }

    public void Resume()
    {
        if (stateGame != StateGame.Pause)
            return;

        stateGame = StateGame.Playing;
        Time.timeScale = 1f;
        gameScene.SetPausePopup(false);
    }

    public void BackToHome()
    {
        SceneLoader.Load(SceneLoader.Home);
    }
}

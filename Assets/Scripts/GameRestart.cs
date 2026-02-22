using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestart : MonoBehaviour
{
    public void RestartGame()
    {
        // 1️⃣ 清全局数据
        PlayerBattleData.Reset();

        // 2️⃣ 销毁 Recorder
        DrinkRecorder.DestroyInstance();

        // 3️⃣ 停掉所有时间缩放
        Time.timeScale = 1f;

        // 4️⃣ 回到第一个场景（制作界面）
        SceneManager.LoadScene("DrinkMakeScene");
    }
}

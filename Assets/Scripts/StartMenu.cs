using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public string drinkMakeSceneName = "BubbleTeaMake";

    public void OnStartClicked()
    {
        // ① 确保时间是正常的
        Time.timeScale = 1f;

        // ② 清空全局数据（如果存在）
        PlayerBattleData.Reset();
        DrinkRecorder.DestroyInstance();

        // ③ 进入奶茶制作界面
        SceneManager.LoadScene(drinkMakeSceneName);
    }
}

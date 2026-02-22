using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishDrink : MonoBehaviour
{
    public void Finish()
    {
        // 此时 currentDrink 已经是完整数据
        SceneManager.LoadScene("Fight");
    }
    public void StartBattle()
    {
        PlayerBattleData.ApplyFromRecorder();
        SceneManager.LoadScene("BattleScene");
    }

}

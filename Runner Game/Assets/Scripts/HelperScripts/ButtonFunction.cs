using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ButtonFunction : MonoBehaviour
{
    private Button _button;

    private void Awake() {
        _button = GetComponent<Button>();
    }
    private void Start() 
    {
        if(SoundManager.Instance != null)
        {
            _button.onClick.AddListener(()=>SoundManager.Instance.PlaySFX(SoundType.ButtonClick));
        }
    }
    public void RestartGame()
    {
        SceneLoader.Instance.LoadScene(StringHolder.GameplaySceneName);
    }
    public void BackToMain()
    {
        SceneLoader.Instance.LoadScene(StringHolder.MainMenuSceneName);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void OnDestroy() {
        _button.onClick.RemoveAllListeners();
    }
}

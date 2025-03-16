using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneService : MonoBehaviour
{
    [SerializeField] private Canvas _loadingCanvas;
    [SerializeField] private TMP_Text _loadingText;
    [SerializeField] private Button _startButton;
    
    private AsyncOperation asyncLoad;
    private void Start()
    {
        // Скрываем кнопку "Начать" при старте
        _startButton.gameObject.SetActive(false);
        _startButton.onClick.AddListener(OnStartButtonClicked); // Добавляем обработчик нажатия на кнопку
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Проверяем, что это игрок
        {
            LoadScene("Level_2");
        }
    }

    public void LoadScene(string sceneName)
    {
        _loadingCanvas.gameObject.SetActive(true); // Активируем Canvas с надписью "Loading"
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; // Запрещаем автоматическую активацию сцены

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // Прогресс от 0 до 1
            _loadingText.text = $"Loading: {(progress * 100f):0}%"; // Обновляем текст прогресса

            // Когда загрузка завершена, но сцена ещё не активирована
            if (asyncLoad.progress >= 0.9f)
            {
                _loadingText.text = "Loading: 100%"; // Устанавливаем 100%
                _startButton.gameObject.SetActive(true); // Показываем кнопку "Начать"
            }

            yield return null;
        }
    }

    private void OnStartButtonClicked()
    {
        // Активируем сцену, когда игрок нажимает кнопку "Начать"
        asyncLoad.allowSceneActivation = true;
        _startButton.gameObject.SetActive(false); // Скрываем кнопку после нажатия
    }

    private void UnloadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
}

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
    
    private AsyncOperation _asyncLoad;
    private int _currentSceneIndex = 0;
    private string[] _sceneNames = { "Level_1", "Level_2", "Level_3" };
    
    private void Start()
    {
        _startButton.gameObject.SetActive(false);
        _startButton.onClick.AddListener(OnStartButtonClicked); // Добавляем обработчик нажатия на кнопку
        
        _currentSceneIndex = System.Array.IndexOf(_sceneNames, SceneManager.GetActiveScene().name);   // Определяем индекс текущей сцены
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoadNextScene();
        }
    }

    public void LoadNextScene()
    {
        int nextSceneIndex = (_currentSceneIndex + 1) % _sceneNames.Length;  // Увеличиваем индекс для загрузки следующей сцены
        string nextSceneName = _sceneNames[nextSceneIndex];

        _loadingCanvas.gameObject.SetActive(true); // Активируем Canvas с надписью "Loading"
        StartCoroutine(LoadSceneAsync(nextSceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        _asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        _asyncLoad.allowSceneActivation = false; // Запрещаем автоматическую активацию сцены

        while (!_asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(_asyncLoad.progress / 0.9f); // Прогресс от 0 до 1
            _loadingText.text = $"Loading: {(progress * 100f):0}%"; // Обновляем текст прогресса

            // Когда загрузка завершена, но сцена ещё не активирована
            if (_asyncLoad.progress >= 0.9f)
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
        _asyncLoad.allowSceneActivation = true;
        _startButton.gameObject.SetActive(false); // Скрываем кнопку после нажатия
        
        // Выгружаем предыдущую сцену
        StartCoroutine(UnloadPreviousScene());
    }

    private IEnumerator UnloadPreviousScene()
    {
        // Ждем, пока новая сцена не будет активирована
        yield return new WaitUntil(() => _asyncLoad.isDone);

        // Выгружаем предыдущую сцену
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(_sceneNames[_currentSceneIndex]);
        while (!unloadOperation.isDone)
        {
            yield return null;
        }

        // Обновляем индекс текущей сцены
        _currentSceneIndex = (_currentSceneIndex + 1) % _sceneNames.Length;
    }
}

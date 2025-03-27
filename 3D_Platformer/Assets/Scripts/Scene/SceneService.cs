using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class SceneService : MonoBehaviour
{
    private static bool _isFirstLaunch = true;    //Статическая переменная сохраняет свое значение между сценами, пока игра не будет перезапущена.
    
    private Canvas _loadingCanvas;
    private TMP_Text _loadingText;
    private Button _startButton;
    private Canvas _mainMenuCanvas;
    private Button _mainMenuButton;
    
    private AsyncOperation _asyncLoad;
    private int _currentSceneIndex = 0;

    [Inject] 
    public void Construction(UILoadingPanel loadingPanel, UIMainMenu mainMenu)
    {
        _loadingCanvas = loadingPanel.CanvasPanel;
        _loadingText = loadingPanel.LoadingText;
        _startButton = loadingPanel.Button;

        _mainMenuCanvas = mainMenu.MainMenuCanvas;
        _mainMenuButton = mainMenu.MainMenuButton;
    }
    
    private void Start()
    {
        _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;   // Определяем индекс текущей сцены
        Debug.Log("Индекс текущей сцены: " + _currentSceneIndex);

        // Активируем главное меню только при первом запуске
        if (_isFirstLaunch)
        {
            _mainMenuCanvas.enabled = true;
            _mainMenuButton.onClick.AddListener(LoadFirstScene);
            _isFirstLaunch = false;
        }
        else
        {
            _mainMenuCanvas.enabled = false;
        }

        _loadingCanvas.enabled = false;
        _startButton.onClick.AddListener(OnStartButtonClicked);
    }

    private void LoadFirstScene()
    {
        _mainMenuCanvas.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoadNextScene();
            _loadingCanvas.enabled = true;
        }
    }

    public void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        Debug.Log("Индекс загружаемой сцены: " + nextSceneIndex);

        _loadingCanvas.gameObject.SetActive(true); // Активируем Canvas с надписью "Loading"
        StartCoroutine(SceneLoad(nextSceneIndex));
    }

    private IEnumerator SceneLoad(int sceneIndex)
    {
        _asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
        _asyncLoad.allowSceneActivation = false;

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

    public void OnStartButtonClicked()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        if (currentSceneIndex > 0)
        {
            int previousSceneIndex = currentSceneIndex - 1;
            if (SceneManager.GetSceneByBuildIndex(previousSceneIndex).isLoaded)
            {
                SceneManager.UnloadSceneAsync(previousSceneIndex);
            }
        }
        
        // Активируем сцену, когда игрок нажимает кнопку "Начать"
        _asyncLoad.allowSceneActivation = true;
        _startButton.gameObject.SetActive(false); // Скрываем кнопку после нажатия
        
    }
}

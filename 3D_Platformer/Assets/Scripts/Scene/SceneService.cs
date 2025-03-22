using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class SceneService : MonoBehaviour
{
    [SerializeField] private Canvas _loadingCanvas;
    [SerializeField] private TMP_Text _loadingText;
    [SerializeField] private Button _startButton;
    
    private AsyncOperation _asyncLoad;
    private int _currentSceneIndex = 0;
    private string[] _sceneNames = { "Level_1", "Level_2", "Level_3" };

    /*[Inject] 
    public void Construction(Canvas canvas)
    {
        _loadingCanvas = canvas;
        _startButton = canvas.GetComponentInChildren<Button>();
    }*/
    
    private void Start()
    {
        _startButton.gameObject.SetActive(false);
        //_startButton.onClick.AddListener(OnStartButtonClicked); // Добавляем обработчик нажатия на кнопку
        
        string activeSceneName = SceneManager.GetActiveScene().name;
        Debug.Log("Active Scene: " + activeSceneName);
        
        _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;   // Определяем индекс текущей сцены
        Debug.Log(_currentSceneIndex);
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
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        string nextSceneName = _sceneNames[nextSceneIndex];
        Debug.Log("загружаем сцену номер " + nextSceneIndex);

        _loadingCanvas.gameObject.SetActive(true); // Активируем Canvas с надписью "Loading"
        StartCoroutine(SceneLoad(nextSceneIndex));
    }

    private IEnumerator SceneLoad(int sceneName)
    {
        _asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
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

    public void OnStartButtonClicked()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            SceneManager.UnloadSceneAsync(currentSceneIndex - 1);
            Debug.Log("1");
        }
        // Активируем сцену, когда игрок нажимает кнопку "Начать"
        _asyncLoad.allowSceneActivation = true;
        _startButton.gameObject.SetActive(false); // Скрываем кнопку после нажатия
        
    }
}

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class SceneService : MonoBehaviour
{

    public static SceneService Instance;
    
    private static bool _isFirstLaunch = true;    //Статическая переменная сохраняет свое значение между сценами, пока игра не будет перезапущена.
    
    private Canvas _loadingCanvas;
    private TMP_Text _loadingText;
    private Button _startButton;
    
    private Canvas _mainMenuCanvas;
    private Button _mainMenuStartButton;
    private Button _mainMenuSettingsButton;
    private Button _mainMenuAboutButton;
    private Button _mainMenuExitButton;
    
    private Canvas _pauseMenuCanvas;
    private Button _pauseToMainMenuButton;
    private Button _pauseToSettingsButton;
    private Button _pauseToContinueButton;
    
    private Canvas _aboutCanvas;
    private Button _aboutButton;
    
    private Canvas _settingsCanvas;
    private Button _settingsToPauseButton;
    private Scrollbar _volumeScrollbar;
    
    private Canvas _gameOverCanvas;
    private Button _gameOverMainMenuButton;
    private Button _gameOverRestartButton;
    
    private AudioSource _backgroundMusic;
    private AudioSource _uIMusic;
    private AudioSource _buttonAudio;
    
    private AsyncOperation _asyncLoad;
    private int _currentSceneIndex = 0;
    private bool _bIsPaused = false;

    [Inject] 
    public void Construction(UILoadingPanel loadingPanel, UIMainMenu mainMenu, 
        UIPause pauseMenu, BackgroundMusic backgroundMusic, UIMusic uiMusic,
        ButtonClickAudio buttonClickAudio, UIAbout about, UISettings settings, UIGameOver gameOver)
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        _loadingCanvas = loadingPanel.CanvasPanel;
        _loadingText = loadingPanel.LoadingText;
        _startButton = loadingPanel.Button;

        _mainMenuCanvas = mainMenu.MainMenuCanvas;
        _mainMenuStartButton = mainMenu.MainMenuStartButton;
        _mainMenuSettingsButton = mainMenu.MainMenuSettingsButton;
        _mainMenuAboutButton = mainMenu.MainMenuAboutButton;
        _mainMenuExitButton = mainMenu.MainMenuExitButton;

        _pauseMenuCanvas = pauseMenu.PauseCanvas;
        _pauseToMainMenuButton = pauseMenu.PauseToMainMenuButton;
        _pauseToSettingsButton = pauseMenu.PauseToSettings;
        _pauseToContinueButton = pauseMenu.PauseToGame;
        
        _aboutCanvas = about.AboutCanvas;
        _aboutButton = about.AboutCloseButton;
        
        _settingsCanvas = settings.SettingsCanvas;
        _settingsToPauseButton = settings.BackToPauseButton;
        _volumeScrollbar = settings.VolumeScrollbar;
        
        _gameOverCanvas = gameOver.GameOverCanvas;
        _gameOverMainMenuButton = gameOver.GameOverBackToMainMenu;
        _gameOverRestartButton = gameOver.GameOverRestartGame;
        
        _backgroundMusic = backgroundMusic.BackgroundAudio.GetComponent<AudioSource>();
        _uIMusic = uiMusic.UIMusicObject.GetComponent<AudioSource>();
        _buttonAudio = buttonClickAudio.ButtonClickAudioObject.GetComponent<AudioSource>();
        _volumeScrollbar.value = 1;
        _volumeScrollbar.onValueChanged.AddListener(OnVolumeChanged);
    }
    
    private void Start()
    {
        _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;   // Определяем индекс текущей сцены
        
        _mainMenuStartButton.onClick.RemoveAllListeners();
        _mainMenuAboutButton.onClick.RemoveAllListeners();
        _aboutButton.onClick.RemoveAllListeners();
        _mainMenuSettingsButton.onClick.RemoveAllListeners();
        _mainMenuExitButton.onClick.RemoveAllListeners();
        _settingsToPauseButton.onClick.RemoveAllListeners();
        _pauseToSettingsButton.onClick.RemoveAllListeners();
        _startButton.onClick.RemoveAllListeners();
        _pauseToMainMenuButton.onClick.RemoveAllListeners();
        _pauseToContinueButton.onClick.RemoveAllListeners();
        _gameOverMainMenuButton.onClick.RemoveAllListeners();
        _gameOverRestartButton.onClick.RemoveAllListeners();
            

        // Добавляем обработчики событий
        _mainMenuStartButton.onClick.AddListener(LoadFirstScene);
        _mainMenuAboutButton.onClick.AddListener(ToggleAboutMenu);
        _aboutButton.onClick.AddListener(ToggleAboutMenu);
        _mainMenuSettingsButton.onClick.AddListener(ToggleSettingsMenu);
        _mainMenuExitButton.onClick.AddListener(ExitGame);
        _settingsToPauseButton.onClick.AddListener(ToggleSettingsMenu);
        _pauseToSettingsButton.onClick.AddListener(ToggleSettingsMenu);
        _startButton.onClick.AddListener(OnStartButtonClicked);
        _pauseToMainMenuButton.onClick.AddListener(ReturnToMainMenu);
        _pauseToContinueButton.onClick.AddListener(TogglePauseUI);
        _gameOverMainMenuButton.onClick.AddListener(ReturnToMainMenu);
        _gameOverRestartButton.onClick.AddListener(RestartGame);

        // Добавляем обработчики для воспроизведения звука кнопки
        _mainMenuStartButton.onClick.AddListener(PlayButtonAudio);
        _mainMenuAboutButton.onClick.AddListener(PlayButtonAudio);
        _aboutButton.onClick.AddListener(PlayButtonAudio);
        _mainMenuSettingsButton.onClick.AddListener(PlayButtonAudio);
        _mainMenuExitButton.onClick.AddListener(PlayButtonAudio);
        _settingsToPauseButton.onClick.AddListener(PlayButtonAudio);
        _pauseToSettingsButton.onClick.AddListener(PlayButtonAudio);
        _startButton.onClick.AddListener(PlayButtonAudio);
        _pauseToMainMenuButton.onClick.AddListener(PlayButtonAudio);
        _pauseToContinueButton.onClick.AddListener(PlayButtonAudio);
        _gameOverMainMenuButton.onClick.AddListener(PlayButtonAudio);
        _gameOverRestartButton.onClick.AddListener(PlayButtonAudio);
        
        // Активируем главное меню только при первом запуске
        if (_isFirstLaunch)
        {
            TogglePause();
            _uIMusic.Play();
            _aboutCanvas.enabled = false;
            _settingsCanvas.enabled = false;
            _gameOverCanvas.enabled = false;
            _mainMenuCanvas.enabled = true;
        }
        
        _isFirstLaunch = false;
        _loadingCanvas.enabled = false;
        _pauseMenuCanvas.enabled = false;
    }

    private void LoadFirstScene()
    {
        if (_bIsPaused == false)
        {
            _bIsPaused = true;
        }
        TogglePause();
        _mainMenuCanvas.enabled = false;
    }

    private void RestartGame()
    {
        if (BonusService.Instance != null)
        {
            BonusService.Instance.ResetBonusState();
        }
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        
        _gameOverCanvas.enabled = false;
        
        Time.timeScale = 1;
        _backgroundMusic.Play();
        _uIMusic.Stop();
        _bIsPaused = false;
    }

    private void ToggleAboutMenu()
    {
        _aboutCanvas.enabled = !_aboutCanvas.enabled;
    }

    private void ToggleSettingsMenu()
    {
        _settingsCanvas.enabled = !_settingsCanvas.enabled;
    }
    
    private void PlayButtonAudio()
    {
        _buttonAudio.Play();
    }
    
    private void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
    }
    
    public void ShowGameOver()
    {
        _gameOverCanvas.enabled = true;
        Time.timeScale = 0;
        _bIsPaused = true;
        _backgroundMusic.Stop();
        _uIMusic.Play();
        
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
    private void ReturnToMainMenu()
    {
        if (BonusService.Instance != null)
        {
            BonusService.Instance.ResetBonusState();
        }
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        
        _pauseMenuCanvas.enabled = false;
        _gameOverCanvas.enabled = false;
        _mainMenuCanvas.enabled = true;
        _bIsPaused = false;
        Time.timeScale = 0;
        _backgroundMusic.Stop();
        _uIMusic.Play();
    }

    public void OnTriggerFinish()
    {
        if (IsLastScene())
        {
            ShowGameOver();
        }
        else
        {
            LoadNextScene();
            TogglePause();
            _loadingCanvas.enabled = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseUI();
        }
    }

    public bool IsLastScene()
    {
        return SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1;
    }

    private void TogglePause()
    {
        _bIsPaused = !_bIsPaused;

        if (_bIsPaused)
        {
            _uIMusic.Play();
            _backgroundMusic.Stop();
        }
        else
        {
            _uIMusic.Stop();
            _backgroundMusic.Play();
        }
        
        Time.timeScale = _bIsPaused ? 0 : 1;
        //Cursor.lockState = _bIsPaused ? CursorLockMode.None : CursorLockMode.Locked;
        //Cursor.visible = _bIsPaused;
    }
    
    private void TogglePauseUI()
    {
        if (_settingsCanvas.enabled)
        {
            _settingsCanvas.enabled = false;
        }
        else
        {
            // Переключаем состояние паузы
            _bIsPaused = !_bIsPaused;

            if (_bIsPaused)
            {
                _uIMusic.Play();
                _backgroundMusic.Stop();
            }
            else
            {
                _uIMusic.Stop();
                _backgroundMusic.Play();
            }

            _pauseMenuCanvas.enabled = _bIsPaused;
            Time.timeScale = _bIsPaused ? 0 : 1;
        }
        //Cursor.lockState = _bIsPaused ? CursorLockMode.None : CursorLockMode.Locked;
        //Cursor.visible = _bIsPaused;
    }

    public void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
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
    private void OnStartButtonClicked()
    {
    if (_asyncLoad != null)
    {
        _asyncLoad.allowSceneActivation = true;
    }
    _startButton.gameObject.SetActive(false);
    _loadingCanvas.enabled = false;
    Time.timeScale = 1;
    _bIsPaused = false;
    _backgroundMusic.Play();
    _uIMusic.Stop();
    }
}

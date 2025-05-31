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
    
    private Canvas _gameEndCanvas;
    private TMP_Text _gameEndText1;
    private TMP_Text _gameEndText2;
    private TMP_Text _gameEndText3;
    private Button _gameEndRestartButton;
    private Button _gameEndMainMenuButton;
    
    private AudioSource _backgroundMusic;
    private AudioSource _uIMusic;
    private AudioSource _buttonAudio;
    
    private Canvas _timerCanvas;
    private TMP_Text _timerText;
    private TMP_Text _timeTextLevel1;
    private TMP_Text _timeTextLevel2;
    private TMP_Text _timeTextLevel3;
    private float _timeLevel1;
    private float _timeLevel2;
    private float _timeLevel3;
    private float _timer = 0f;
    private bool _isRunning = false;
    
    private AsyncOperation _asyncLoad;
    private int _currentSceneIndex = 0;
    private bool _bIsPaused = false;

    [Inject] 
    public void Construction(UILoadingPanel loadingPanel, UIMainMenu mainMenu, 
        UIPause pauseMenu, BackgroundMusic backgroundMusic, UIMusic uiMusic,
        ButtonClickAudio buttonClickAudio, UIAbout about, UISettings settings,
        UIGameOver gameOver, UITimer timer, UIGameEnd gameEnd)
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
        
        _timerCanvas = timer.TimerCanvas;
        _timerText = timer.TimerText;
        _timeTextLevel1 = timer.TextLevel1;
        _timeTextLevel2 = timer.TextLevel2;
        _timeTextLevel3 = timer.TextLevel3;

        _gameEndCanvas = gameEnd.UIGameEndCanvas;
        _gameEndText1 = gameEnd.UIGameEndTextResult1;
        _gameEndText2 = gameEnd.UIGameEndTextResult2;
        _gameEndText3 = gameEnd.UIGameEndTextResult3;
        _gameEndRestartButton = gameEnd.UIGameEndRestart;
        _gameEndMainMenuButton = gameEnd.UIGameEndMainMenu;
        
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
        _gameEndRestartButton.onClick.RemoveAllListeners();
        _gameEndMainMenuButton.onClick.RemoveAllListeners();
            

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
        _gameEndRestartButton.onClick.AddListener(RestartGame);
        _gameEndMainMenuButton.onClick.AddListener(ReturnToMainMenu);

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
        _gameEndRestartButton.onClick.AddListener(PlayButtonAudio);
        _gameEndMainMenuButton.onClick.AddListener(PlayButtonAudio);
        
        // Активируем главное меню только при первом запуске
        if (_isFirstLaunch)
        {
            TogglePause();
            _uIMusic.Play();
            _aboutCanvas.enabled = false;
            _settingsCanvas.enabled = false;
            _gameOverCanvas.enabled = false;
            _gameEndCanvas.enabled = false;
            _mainMenuCanvas.enabled = true;
        }
        
        _isFirstLaunch = false;
        _loadingCanvas.enabled = false;
        _pauseMenuCanvas.enabled = false;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LoadFirstScene()
    {
        if (_bIsPaused == false)
        {
            _bIsPaused = true;
        }
        
        TogglePause();
        _isRunning = true;
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
        _gameEndCanvas.enabled = false;
        
        Time.timeScale = 1;
        _backgroundMusic.Play();
        _uIMusic.Stop();
        _bIsPaused = false;
    }

    private void StartTimer()
    {
        int minutes = Mathf.FloorToInt(_timer / 60f);
        int seconds = Mathf.FloorToInt(_timer % 60f);
        int milliseconds = Mathf.FloorToInt((_timer * 1000f) % 1000);

        _timerText.text = $"{minutes:00}:{seconds:00}.{milliseconds:000}";
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
        _gameEndCanvas.enabled = true;
        Time.timeScale = 0;
        _bIsPaused = true;
        _backgroundMusic.Stop();
        _uIMusic.Play();
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        if (_gameEndText1 != null)
            _gameEndText1.text = "1 lvl: " + FormatTime(_timeLevel1);

        if (_gameEndText2 != null)
            _gameEndText2.text = "2 lvl: " + FormatTime(_timeLevel2);

        if (_gameEndText3 != null)
            _gameEndText3.text = "3 lvl: " + FormatTime(_timeLevel3);
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
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        if (currentScene == 0)
        {
            _timeLevel1 = _timer;
        }
        else if (currentScene == 1)
        {
            _timeLevel2 = _timer;
        }
        else if (currentScene == 2)
        {
            _timeLevel3 = _timer;
        }
        
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
    
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000);
        return $"{minutes:00}:{seconds:00}.{milliseconds:000}";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseUI();
        }

        if (_isRunning)
        {
            _timer += Time.deltaTime;
            StartTimer();
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
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            _uIMusic.Stop();
            _backgroundMusic.Play();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        Time.timeScale = _bIsPaused ? 0 : 1;
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
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                _uIMusic.Stop();
                _backgroundMusic.Play();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
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
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _timer = 0f;
        _isRunning = true;

        int currentScene = SceneManager.GetActiveScene().buildIndex;

        if (currentScene == 0)
        {
            if (_timeTextLevel1 != null)
                _timeTextLevel1.text = "1 lvl: " + FormatTime(_timeLevel1);
        }
        else if (currentScene == 1)
        {
            if (_timeTextLevel1 != null)
                _timeTextLevel1.text = "1 lvl: " + FormatTime(_timeLevel1);
            if (_timeTextLevel2 != null)
                _timeTextLevel2.text = "2 lvl: " + FormatTime(_timeLevel2);
        }
    }
}

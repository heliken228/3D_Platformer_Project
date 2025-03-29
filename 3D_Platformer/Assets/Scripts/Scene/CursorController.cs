using UnityEngine;

public class CursorController : MonoBehaviour
{
    private bool isCursorLocked = true;

    void Start()
    {
        // Начальное состояние: курсор скрыт и заблокирован
        UpdateCursorState();
    }

    void Update()
    {
        // При нажатии клавиши Escape переключаем состояние курсора
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isCursorLocked = !isCursorLocked;
            UpdateCursorState();
        }
    }

    void UpdateCursorState()
    {
        if (isCursorLocked)
        {
            // Скрыть и заблокировать курсор
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            // Показать и разблокировать курсор
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}

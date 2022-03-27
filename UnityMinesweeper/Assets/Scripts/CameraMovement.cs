using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private int fieldHeight;
    private int fieldWidth;
    
    // Zoom
    private float cameraMinSize = 3f;
    private float cameraMaxSize;
    private float zoomSpeed;

    private float mouseScrollDelta;
    private float cameraCurrentSize;

    // MOBA
    private Vector3 mousePos;
    
    private float cameraMouseSpeed;
    private float borderSize;
    
    private float topBorder;
    private float bottomBorder;
    private float leftBorder;
    private float rightBorder;

    // Keyboard
    private Vector3 keyboardDirection;

    private float cameraKeyboardSpeed;
    
    private float keyboardVertical;
    private float keyboardHorizontal;


    void Start()
    {
        // Loads variables
        fieldWidth = PlayerPrefs.GetInt("fieldWidth", 16);
        fieldHeight = PlayerPrefs.GetInt("fieldHeight", 16);
        cameraMaxSize = Mathf.Max(fieldWidth, fieldHeight) * 0.625f;

        zoomSpeed = PlayerPrefs.GetFloat("cameraZoomSpeed", 0.3f);
        cameraMouseSpeed = PlayerPrefs.GetFloat("cameraMouseMovementSpeed", 0.15f);
        borderSize = PlayerPrefs.GetFloat("screenBorderSize", 0.03f);
        cameraKeyboardSpeed = PlayerPrefs.GetFloat("cameraKeyboardMovementSpeed", 10f);

        // Places camera in center of a screen
        Camera.main.transform.position = new Vector3(fieldWidth / 2f, fieldHeight / 2f, -10f);
        Camera.main.orthographicSize = cameraMaxSize;
        cameraCurrentSize = cameraMaxSize;

        // Borders for RTS movement
        topBorder = Screen.height * (1f - borderSize);
        bottomBorder = Screen.height * borderSize;
        leftBorder = Screen.width * borderSize;
        rightBorder = Screen.width * (1f - borderSize);
    }

    void Update()
    {
        if (PauseMenu.gameIsPaused) { return; }
        
        Zoom();
        MOBAMovement();
        KeyboardMovement();
    }

    private void Zoom()
    {
        mouseScrollDelta = Input.GetAxis("Mouse ScrollWheel");

        if (mouseScrollDelta > 0f && cameraCurrentSize > cameraMinSize) // Zoom in
        {
            Camera.main.orthographicSize = Mathf.Max(cameraCurrentSize - zoomSpeed, cameraMinSize);
            cameraCurrentSize = Camera.main.orthographicSize;
        }
        else if (mouseScrollDelta < 0f && cameraCurrentSize < cameraMaxSize) // Zoom out
        {
            Camera.main.orthographicSize = Mathf.Min(cameraCurrentSize + zoomSpeed, cameraMaxSize);
            cameraCurrentSize = Camera.main.orthographicSize;
        }
    }

    private void MOBAMovement()
    {
        mousePos = Input.mousePosition;
        
        // UP
        if (transform.position.y < fieldHeight && mousePos.y >= topBorder)
        {
            transform.Translate(0, cameraMouseSpeed, 0, Space.World);
        }
        // DOWN
        if (transform.position.y > 0 && mousePos.y <= bottomBorder) 
        {
            transform.Translate(0, -cameraMouseSpeed, 0, Space.World);
        }
        // LEFT
        if (transform.position.x > 0 && mousePos.x <= leftBorder)
        {
            transform.Translate(-cameraMouseSpeed, 0, 0, Space.World);
        }
        // RIGHT
        if (transform.position.x < fieldWidth && mousePos.x >= rightBorder) 
        {
            transform.Translate(cameraMouseSpeed, 0, 0, Space.World);
        }
    }

    private void KeyboardMovement()
    {
        keyboardVertical = Input.GetAxis("Vertical");
        keyboardHorizontal = Input.GetAxis("Horizontal");

        if (keyboardVertical == 0f && keyboardHorizontal == 0) { return; }
        
        if ((keyboardHorizontal > 0f && transform.position.x >= fieldWidth) || (keyboardHorizontal < 0f && transform.position.x <= 0))
        { 
            keyboardHorizontal = 0f;
        }
        if ((keyboardVertical > 0f && transform.position.y >= fieldHeight) || (keyboardVertical < 0f && transform.position.y <= 0))
        {
            keyboardVertical = 0f;
        }
        
        keyboardDirection = (transform.up * keyboardVertical + transform.right * keyboardHorizontal).normalized;
        transform.position += keyboardDirection * cameraKeyboardSpeed * Time.deltaTime;
    }
}

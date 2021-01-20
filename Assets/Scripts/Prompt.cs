using UnityEngine;
using System.Collections;

public class Prompt : MonoBehaviour {

    private MeshRenderer _meshRenderer;
    [SerializeField]
    private MeshRenderer _bcMeshRenderer;

    [SerializeField]
    private Texture moveUp;
    [SerializeField]
    private Texture moveDown;

    [SerializeField]
    private Texture rotate;

    [SerializeField]
    private Texture powerOn;
    [SerializeField]
    private Texture powerOff;

    [SerializeField]
    private bool onRedirector = false;
    [SerializeField]
    private bool movePrompt = false;

    [SerializeField]
    private Transform redirector;

    private bool promptsEnabled;

    void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    } 

    void Start()
    {
        promptsEnabled = FindObjectOfType<GameManager>().promptsEnabled;
        _meshRenderer.enabled = false;
        _bcMeshRenderer.enabled = false;
    }

    void Update()
    {
        if (promptsEnabled)
            PositionAndRotate();
    }

    void PositionAndRotate()
    {
        if (_meshRenderer.enabled)
        {
            if (onRedirector)
            {
                if (movePrompt)
                    transform.position = redirector.position + new Vector3(-0.33f, 2.5f, -0.11f);
                else
                    transform.position = redirector.position + new Vector3(0.25f, 2.5f, -0.11f);
            }
        }
    }

    public void Enable(string prompt)
    {
        if (promptsEnabled)
        {
            switch (prompt)
            {
                case "MoveUp":
                    _meshRenderer.material.mainTexture = moveUp;
                    break;
                case "MoveDown":
                    _meshRenderer.material.mainTexture = moveDown;
                    break;
                case "Rotate":
                    _meshRenderer.material.mainTexture = rotate;
                    break;
                case "PowerOn":
                    _meshRenderer.material.mainTexture = powerOn;
                    break;
                case "PowerOff":
                    _meshRenderer.material.mainTexture = powerOff;
                    break;
            }

            _meshRenderer.enabled = true;
            _bcMeshRenderer.enabled = true;
        }
    }

    public void Rotate()
    {
        //transform.LookAt(2 * transform.position - Camera.main.transform.position);
        transform.rotation = Quaternion.Euler(new Vector3(50, 0, 0));
    }

    public void Disable()
    {
        if (promptsEnabled)
        {
            _meshRenderer.enabled = false;
            _bcMeshRenderer.enabled = false;
        }
    }
}

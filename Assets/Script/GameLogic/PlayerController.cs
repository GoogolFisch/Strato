//using System.ComponentModel.DataAnnotations;
using System.Transactions;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
using System.Runtime.CompilerServices;

public class PlayerController : MonoBehaviour
{
    [Range(0.1f,10f)]
    public float playerSpeed = 0.5f;
    private Vector3 playerVelocity;
    //private CharacterController controller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Vector2 lastMousePosition = Vector2.zero;
    float scrollSet = 0;
    float timeOffset = 0;
    bool oldMouseDown = false;
    //
    [Header("Input Actions")]
    public InputActionReference moveAction; // expects Vector2
    public InputActionReference fastAction; // expects Vector2
    public InputActionReference mouseDownAct; // ???
    public InputActionReference mouseMoveAct; // ???
    public InputActionReference mouseScrollAct; // ???
    //
    //UnityEvent m_MyEvent;
    void Start()
    {
        scrollSet = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = Vector3.ClampMagnitude(move, 1f);
        float fastDown = fastAction.action.ReadValue<float>();
        if(fastDown > 0.5){
            move *= 2.0f;
        }
        move *= Time.deltaTime;

        float mouseDown = mouseDownAct.action.ReadValue<float>();
        if(mouseDown > 0.5 && !oldMouseDown){
            timeOffset = 0;
        }
        if(mouseDown <= 0.5 && oldMouseDown && timeOffset < 0.2){
            TrySelectEntity();
        }
        if(mouseDown > 0.5){
            Vector2 minput = mouseMoveAct.action.ReadValue<Vector2>();
            minput *= 0.002f;
            move -= new Vector3(minput.x,0,minput.y);
        }

        // Combine horizontal and vertical movement
        move *= playerSpeed;
        move *= scrollSet * 0.1f;
        Vector2 scrolling = mouseScrollAct.action.ReadValue<Vector2>();
        // IDK
        if(scrolling.y > 0.01 || scrolling.y < -0.01){
            scrollSet -= scrolling.y;
            if(scrollSet < 5)scrollSet = 5;
            if(scrollSet > 50)scrollSet = 50;
            move -= Vector3.up * (transform.position.y - scrollSet);

        }
        
        transform.position += move;
        timeOffset += Time.deltaTime;
        oldMouseDown = (mouseDown > 0.5);
    }
    void TrySelectEntity()
    {
        int count = 9;
        RaycastHit _hit;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        //Physics.Raycast(origin, direction, out hit, distance, layerMask);
        Ray rayCast = Camera.main.ScreenPointToRay(mousePos);
        int layerMask = LayerMask.GetMask("Selectable");
        while(count-- > 0 && Physics.Raycast(rayCast,out _hit,99,layerMask)){
        //while(count-- > 0 && Physics.Raycast(rayCast,out _hit)){
            GameObject item = _hit.transform.gameObject;
            //item.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
            Debug.Log(item);
        }
    }

}

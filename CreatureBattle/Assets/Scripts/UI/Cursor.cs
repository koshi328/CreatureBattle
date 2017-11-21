using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cursor : MonoBehaviour {

    Actor _target;
    [SerializeField]
    RectTransform _cursorObject;
    // Use this for initialization
    void Start()
    {
        _cursorObject.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GetClickActor();

        if (!_target) return;
        _cursorObject.position = Camera.main.WorldToScreenPoint(_target.transform.position);
        _cursorObject.Rotate(new Vector3(0, 0, 2));
    }

    void GetClickActor()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            Ray ray = new Ray();
            RaycastHit hit = new RaycastHit();
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 20))
            {
                _target = hit.collider.gameObject.GetComponent<Actor>();
                _cursorObject.gameObject.SetActive(_target ? true : false);
            }
        }
    }

    public Actor GetTarget()
    {
        return _target;
    }
}

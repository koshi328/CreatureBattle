//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SelectActorConnector : MonoBehaviour
//{

//    PhotonView _myPhotonView;
//    bool _done;

//    int _actorID;

//    int _element;
//    private void Awake()
//    {
//        _actorID = -1;
//        _done = false;
//        _myPhotonView = GetComponent<PhotonView>();
//    }

//    [PunRPC]
//    void RPCInitialize(int elem)
//    {
//        GameObject.Find("SelectController").GetComponent<SelectController>().SetConnector(elem, this);
//        _element = elem;
//    }
//    public void Initialize(int elem)
//    {
//        _myPhotonView.RPC("RPCInitialize", PhotonTargets.AllBufferedViaServer, elem);
//    }

//    [PunRPC]
//    void RPCDone(bool isDone)
//    {
//        _done = isDone;
//    }
//    public void Done(bool isDone)
//    {
//        _myPhotonView.RPC("RPCDone", PhotonTargets.AllBufferedViaServer, isDone);
//    }
//    public bool isDone()
//    {
//        return _done;
//    }

//    [PunRPC]
//    void RPCSetActorID(int id)
//    {
//        _actorID = id;
//    }
//    public void SetActorID(int id)
//    {
//        _myPhotonView.RPC("RPCSetActorID", PhotonTargets.AllBufferedViaServer, id);
//    }
//    public int GetActorID()
//    {
//        return _actorID;
//    }

//    private void OnDestroy()
//    {
//        GameObject.Find("SelectController").GetComponent<SelectController>().SetConnector(_element, null);
//    }
//}

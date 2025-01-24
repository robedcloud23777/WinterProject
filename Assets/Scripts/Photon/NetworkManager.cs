using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("DisconnectPanel")]
    public GameObject DisconnectPanel;
    public TMP_InputField NickNameInput;

    [Header("LobbyPanel")]
    public GameObject LobbyPanel;
    public TMP_InputField RoomInput;
    public Button[] CellBtn;
    public Button PreviousBtn;
    public Button NextBtn;

    [Header("UsersPanel")]
    public GameObject UsersPanel;
    public TMP_Text[] PlayerName;
    public Image[] UserSpace;
    public Image[] ReadyState;
    public TMP_Text ReadyBtn;
    public bool[] IsReady;

    [Header("ETC")]
    public TMP_Text StatusText;
    public PhotonView PV;

    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;

    #region 방리스트 갱신
    public void MyListClick(int num)
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else PhotonNetwork.JoinRoom(myList[multiple + num].Name);
        MyListRenewal();
    }

    void MyListRenewal()
    {
        maxPage = (myList.Count % CellBtn.Length == 0) ? myList.Count / CellBtn.Length : myList.Count / CellBtn.Length + 1;
        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        multiple = (currentPage - 1) * CellBtn.Length;
        for (int i = 0; i < CellBtn.Length; i++)
        {
            CellBtn[i].interactable = (multiple + i < myList.Count) ? true : false;
            CellBtn[i].transform.GetChild(0).GetComponent<TMP_Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].Name : "";
            CellBtn[i].transform.GetChild(1).GetComponent<TMP_Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
                else myList[myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
        }
        MyListRenewal();
    }
    #endregion

    #region 서버연결
    void Awake() => Screen.SetResolution(960, 540, false);

    void Update()
    {
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby()
    {
        DisconnectPanel.SetActive(false);
        LobbyPanel.SetActive(true);
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        myList.Clear();
    }

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause)
    {
        DisconnectPanel.SetActive(true);
        LobbyPanel.SetActive(false);
        UsersPanel.SetActive(false);
    }
    #endregion

    #region 방
    public void CreateRoom() => PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Room" + Random.Range(0, 100) : RoomInput.text, new RoomOptions { MaxPlayers = 2 });

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnJoinedRoom()
    {
        LobbyPanel.SetActive(false);
        UsersPanel.SetActive(true);

        // 모든 UI 초기화
        for (int i = 0; i < PlayerName.Length; i++)
        {
            PlayerName[i].text = "Waiting...";
            Color color = UserSpace[i].color;
            color.a = 0.5f;
            UserSpace[i].color = color;

            Color readyColor = ReadyState[i].color;
            readyColor.a = 0.2f;
            ReadyState[i].color = readyColor;

            IsReady[i] = false;
        }

        // 플레이어 UI 업데이트
        foreach (KeyValuePair<int, Player> entry in PhotonNetwork.CurrentRoom.Players)
        {
            int index = entry.Value.ActorNumber - 1;
            Player player = entry.Value;

            PlayerName[index].text = player.NickName;

            Color color = UserSpace[index].color;
            color.a = 1;
            UserSpace[index].color = color;

            // IsReady 상태를 동기화 (필요 시 RPC 또는 다른 방법 사용)
            bool readyState = IsReady[index];
            Color readyColor = ReadyState[index].color;
            readyColor.a = readyState ? 1.0f : 0.2f;
            ReadyState[index].color = readyColor;
        }
    }


    public void Ready()
    {
        if (ReadyBtn.text == "시작") photonView.RPC("GameStart", RpcTarget.All);
        int index = PhotonNetwork.LocalPlayer.ActorNumber;
        photonView.RPC("SetReadyState", RpcTarget.All, index - 1);
    }

    [PunRPC]
    void SetReadyState(int playerIndex)
    {
        IsReady[playerIndex] = !IsReady[playerIndex] ? true : false;
        Color color = ReadyState[playerIndex].color;
        color.a = color.a != 1.0f ? 1.0f : 0.2f;
        ReadyState[playerIndex].color = color;
        if (PhotonNetwork.IsMasterClient && IsReady[0] && IsReady[1]) ReadyBtn.text = "시작";
        else ReadyBtn.text = "준비";
    }

    [PunRPC]
    void GameStart()
    {
        PhotonNetwork.LoadLevel("Main");
    }


    public override void OnCreateRoomFailed(short returnCode, string message) { RoomInput.text = ""; CreateRoom(); }

    public override void OnJoinRandomFailed(short returnCode, string message) { RoomInput.text = ""; CreateRoom(); }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        int index = newPlayer.ActorNumber - 1;
        PlayerName[index].text = newPlayer.NickName;

        Color color = UserSpace[index].color;
        color.a = 1;
        UserSpace[index].color = color;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = otherPlayer.ActorNumber - 1;
        PlayerName[index].text = "Waiting...";
        Color color = UserSpace[index].color;
        color.a = 0.5f;
        UserSpace[index].color = color;

        Color readyColor = ReadyState[index].color;
        readyColor.a = 0.2f;
        ReadyState[index].color = readyColor;

        IsReady[index] = false;
    }
    #endregion
}

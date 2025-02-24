using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("StartPanel")]
    public GameObject StartPanel;
    public Image[] BackGround;
    public GameObject TutorialPanel;
    public GameObject[] Pages;
    int curPage = 0;

    [Header("NicknamePanel")]
    public GameObject NicknamePanel;
    public TMP_InputField NicknameInput;

    [Header("LobbyPanel")]
    public GameObject LobbyPanel;
    public TMP_Text CurrentNick;
    public Button CharacterBtn;
    public Button CreateRoomBtn;
    public Button FindRoomBtn;

    [Header("CharacterPanel")]
    public GameObject CharacterPanel;
    public GameObject[] CharacterInfo;
    public GameObject[] Characters;
    public TMP_Text SelectState;
    public Button Left;
    public Button Right;
    public TMP_Text CantExit;
    int curInfo = 0;

    [Header("CreaterRoomPanel")]
    public GameObject CreateRoomPanel;
    public TMP_InputField RoomInput;

    [Header("FindRoomPanel")]
    public GameObject FindRoomPanel;
    public Button[] CellBtn;
    public Button PreviousBtn;
    public Button NextBtn;

    [Header("UsersPanel")]
    public GameObject UsersPanel;
    public TMP_Text[] PlayerName;
    public Image[] UserSpace;
    public Image[] ReadyState;
    public Button StartBtn;
    public bool[] IsReady;

    [Header("ETC")]
    public TMP_Text StatusText;

    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;
    bool already;

    private void Start()
    {
        // 알파값을 1로 늘리며 Fade In
        for (int i = 0; i < BackGround.Length; i++)
            BackGround[i].DOFade(1f, 1f).SetEase(Ease.InOutQuad);
    }

    public void Play()
    {
        StartPanel.SetActive(false);
        NicknamePanel.SetActive(true);
    }

    public void OpenTutorial()
    {
        curPage = 0;
        TutorialPanel.SetActive(true);
        for (int i = 0; i < Pages.Length; i++)
        {
            if (i == curPage) Pages[i].SetActive(true);
            else Pages[i].SetActive(false);
        }
    }

    public void CloseTutorial()
    {
        TutorialPanel.SetActive(false);
    }

    public void Previous()
    {
        curPage = curPage > 0 ? curPage - 1 : curPage;
        for (int i = 0; i < Pages.Length; i++)
        {
            if (i == curPage) Pages[i].SetActive(true);
            else Pages[i].SetActive(false);
        }
    }

    public void Next()
    {
        curPage = curPage < Pages.Length - 1 ? curPage + 1 : curPage;
        for (int i = 0; i < Pages.Length; i++)
        {
            if (i == curPage) Pages[i].SetActive(true);
            else Pages[i].SetActive(false);
        }
    }

    public void Exit()
    {
        if (NicknamePanel.activeSelf)
        {
            StartPanel.SetActive(true);
            NicknamePanel.SetActive(false);
        }else if (CharacterPanel.activeSelf)
        {
            if(SelectState.text != "선택완료")
            {
                Sequence fadeSequence = DOTween.Sequence();
                fadeSequence.Append(CantExit.DOFade(1f, 0.5f).SetEase(Ease.InOutQuad));
                fadeSequence.AppendInterval(1f);
                fadeSequence.Append(CantExit.DOFade(0f, 0.5f).SetEase(Ease.InOutQuad));
                fadeSequence.Play();
                return;
            }
            LobbyPanel.SetActive(true);
            CharacterPanel.SetActive(false);
            Characters[curInfo].SetActive(false);
        }
        else if (CreateRoomPanel.activeSelf)
        {
            LobbyPanel.SetActive(true);
            CreateRoomPanel.SetActive(false);
        }else if (FindRoomPanel.activeSelf)
        {
            LobbyPanel.SetActive(true);
            FindRoomPanel.SetActive(false);
        }else if (UsersPanel.activeSelf)
        {
            LobbyPanel.SetActive(true);
            UsersPanel.SetActive(false);
            PhotonNetwork.LeaveRoom();
        }
    }

    public void LeftCharacter()
    {
        if(curInfo == 0) return;
        curInfo--;
        for (int i = 0; i < CharacterInfo.Length; i++)
        {
            if (i == curInfo)
            {
                CharacterInfo[i].SetActive(true);
                Characters[i].SetActive(true);
            }
            else
            {
                CharacterInfo[i].SetActive(false);
                Characters[i].SetActive(false);
            }
        }
    }

    public void RightCharacter()
    {
        if (curInfo == CharacterInfo.Length - 1) return;
        curInfo++;
        for (int i = 0; i < CharacterInfo.Length; i++)
        {
            if (i == curInfo)
            {
                CharacterInfo[i].SetActive(true);
                Characters[i].SetActive(true);
            }
            else
            {
                CharacterInfo[i].SetActive(false);
                Characters[i].SetActive(false);
            }
        }
    }

    public void SelectCharacter()
    {
        SelectState.text = SelectState.text == "선택" ? "선택완료" : "선택";
        GameManager.Instance.myCharacter = curInfo;
        if(SelectState.text == "선택완료")
        {
            Left.interactable = false;
            Right.interactable = false;
        }
        else {
            Left.interactable = true;
            Right.interactable = true;
        }
    }

    public void EnterCharacterPanel()
    {
        LobbyPanel.SetActive(false);
        CharacterPanel.SetActive(true);
        Characters[curInfo].SetActive(true);
    }
    public void EnterCreateRoomPanel()
    {
        LobbyPanel.SetActive(false);
        CreateRoomPanel.SetActive(true);
    }
    public void EnterFindRoomPanel()
    {
        LobbyPanel.SetActive(false);
        FindRoomPanel.SetActive(true);

    }

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
        NicknamePanel.SetActive(false);
        LobbyPanel.SetActive(true);
        if (!already)
        {
            curInfo = 0;
            SelectState.text = "선택";
            Left.interactable = true;
            Right.interactable = true;
        }
        already = true;
        PhotonNetwork.LocalPlayer.NickName = NicknameInput.text;
        CurrentNick.text = PhotonNetwork.LocalPlayer.NickName;
        myList.Clear();
    }

    public void Disconnect()
    {
        already = false;
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        NicknamePanel.SetActive(true);
        LobbyPanel.SetActive(false);
    }
    #endregion

    #region 방
    public void CreateRoom() => PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Room" + Random.Range(0, 100) : RoomInput.text, new RoomOptions { MaxPlayers = 2 });

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnJoinedRoom()
    {
        CreateRoomPanel.SetActive(false);
        FindRoomPanel.SetActive(false);
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
            StartBtn.interactable = false;
        }

        // 플레이어 UI 업데이트
        foreach (KeyValuePair<int, Photon.Realtime.Player> entry in PhotonNetwork.CurrentRoom.Players)
        {
            int index = entry.Value.ActorNumber - 1;
            Photon.Realtime.Player player = entry.Value;

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
        if (PhotonNetwork.IsMasterClient && IsReady[0] && IsReady[1])
        {
            StartBtn.interactable = true;
        }
    }

    public void StartGame()
    {
        if (IsReady[0] && IsReady[1]) photonView.RPC("GameStart", RpcTarget.All);
    }

    [PunRPC]
    void GameStart()
    {
        PhotonNetwork.LoadLevel("Main");
    }


    public override void OnCreateRoomFailed(short returnCode, string message) { RoomInput.text = ""; CreateRoom(); }

    public override void OnJoinRandomFailed(short returnCode, string message) { RoomInput.text = ""; CreateRoom(); }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        int index = newPlayer.ActorNumber - 1;
        PlayerName[index].text = newPlayer.NickName;

        Color color = UserSpace[index].color;
        color.a = 1;
        UserSpace[index].color = color;
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
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

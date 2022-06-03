using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace FPSMulti
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        public void Awake() //wlacza si� ta funkcja i wywo�uje Connect()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            Connect();
        }

        public override void OnConnectedToMaster() //wywo�uje join
        {
            Debug.Log("Connected.");
            Join();
            base.OnConnectedToMaster();
            
        }

        public override void OnJoinedRoom() //gdy juz istnieje room i gracz si� ��czy, wywo�uje si� StartGame()
        {
            StartGame();
            base.OnJoinedRoom();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Create();
            base.OnJoinRandomFailed(returnCode, message);
        }
        public void Connect()
        {
            Debug.Log("Trying to connect...");
            PhotonNetwork.GameVersion = "0.0.0";
            PhotonNetwork.ConnectUsingSettings();
            //w��cza si� OnConnecterToMaster
        }
        
        public void Join()
        {
            PhotonNetwork.JoinRandomRoom(); //do��cza, a jesli nie, wywo�uje OnJoinRandomFailed
        }

        public void Create()
        {
            PhotonNetwork.CreateRoom(""); //tworzy room, bo go nie by�o
        }

        public void StartGame()
        {
            if(PhotonNetwork.CurrentRoom.PlayerCount==1)
            {
                PhotonNetwork.LoadLevel(1); //to jest za�adowanie sceny o takim indeksie w build settings
            }
        }
    }
}
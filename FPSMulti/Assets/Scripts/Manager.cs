using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

namespace FPSMulti
{
    public class Manager : MonoBehaviour
    {
        public string playerPrefab;
        public GameObject PlayerPrefab;
        public Transform[] spawnPoints;
        public Text countTime;
        private static float startTime = ScoreManager.timeleft;
        public float timeleft;

        public Material[] materials;

        private void Start()
        {
            double minutes = Math.Floor((double)startTime / 60);
            double seconds = Math.Floor((double)startTime % 60);
            string minutesStr = minutes.ToString();
            string secondsStr = seconds.ToString();
            if (minutesStr.Length == 1) minutesStr = "0" + minutesStr;
            if (secondsStr.Length == 1) secondsStr = "0" + secondsStr;
            
            countTime.text = $"{minutesStr}:{secondsStr}";
            timeleft = startTime;
            Spawn();
        }
        public void Update()
        {
            timeleft -= Time.deltaTime;

            double minutes = Math.Floor((double)timeleft / 60);
            double seconds = Math.Floor((double)timeleft % 60);

            string minutesStr = minutes.ToString();
            string secondsStr = seconds.ToString();
            if (minutesStr.Length == 1) minutesStr = "0" + minutesStr;
            if (secondsStr.Length == 1) secondsStr = "0" + secondsStr;
            Debug.Log(secondsStr);
            countTime.text = $"{minutesStr}:{secondsStr}";


            if (countTime.text == "00:00")
            {
                SceneManager.LoadScene("MenuScene");
                PhotonNetwork.LoadLevel("MenuScene");
                PhotonNetwork.Disconnect();
            }
        }
        public void Spawn()
        {
            Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
            if (PhotonNetwork.CountOfPlayers < materials.Length)
            {
                PlayerPrefab.GetComponent<MeshRenderer>().material = materials[0];
                PhotonNetwork.Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            }
            else
            {
                Application.Quit();
            }
        }

        public void SpawnAgain(Material m)
        {
            Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

            PlayerPrefab.GetComponent<MeshRenderer>().material = m;
            PhotonNetwork.Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
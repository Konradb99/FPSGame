using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace FPSMulti
{
    public class Manager : MonoBehaviour
    {
        public string playerPrefab;
        public GameObject PlayerPrefab;
        public Transform[] spawnPoints;

        public Material[] materials;

        private void Start()
        {
            Spawn();
        }

        public void Spawn()
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
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
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            PlayerPrefab.GetComponent<MeshRenderer>().material = m;
            PhotonNetwork.Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
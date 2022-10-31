using System.Collections;
using System.Collections.Generic;
using ArthemyDevelopment.Save;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace PudimdimGames
{
    
    public class SaveAndLoadManager : MonoBehaviour
    {
        public static SaveAndLoadManager SLInstance;
        [SerializeField] private Vector3 playerPosition;
        [HideInInspector] public Vector3 getPlayerPosition;
        [SerializeField] private Vector3[] enemiesPosition;
        [HideInInspector] public Vector3[] getEnemiesPosition;
        [SerializeField] private int enemyIndex;
        [HideInInspector] public static int getEnemyIndex;
        [SerializeField] private Vector3[] itensPosition;
        [HideInInspector] public Vector3[] getItensPosition;
        [SerializeField] private int itemIndex;
        [HideInInspector] public static int getItemIndex;
        [SerializeField] private bool[] doorsOpen;
        [HideInInspector] public bool[] getDoorsOpen;
        [SerializeField] private int doorIndex;
        [SerializeField] public static int getDoorIndex;
        [SerializeField] private bool cheatMode;
        [HideInInspector] public bool getCheatMode;
        private bool created;
        public bool gameSaved;

        SaveData saveData = new SaveData();

        enum ValuesKeys
        {
            playerPos,
            enemiesPos,
            itemPos,
            doors,
            cheatControll,
        }

        void Awake ()
        {
            if (!created)
            {
                DontDestroyOnLoad(this.gameObject);
                created = true;
            }
            
            else
            {
                Destroy(this.gameObject);
            }
        }

        // Start is called before the first frame update
        void Start(){
            SLInstance = this;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.L))
            {
                if(gameSaved)
                    //Loading the save file
                    saveData.LoadDataFile();

            
            }
        }
        public void AssignValues(){
            playerPosition = getPlayerPosition;
        }

        public void Save(){
            //Saving the variables value and link them to their key
                saveData.SaveValue(ValuesKeys.playerPos, playerPosition);
               // saveData.SaveValue(ValuesKeys.enemiesPos, enemiesPosition);
               // saveData.SaveValue(ValuesKeys.itemPos, itensPosition);
               // saveData.SaveValue(ValuesKeys.doors, doorsOpen);
               // saveData.SaveValue(ValuesKeys.cheatControll, cheatMode);
                //Saving the file after every variable has been saved
                saveData.SaveDataFile();
                gameSaved = true;
        }

        public void Load(){
            if(gameSaved){   

                //   COMO CARREGAR AS INFORMAÇÕES NA CENA
                //Loading the save file
                saveData.LoadDataFile();
                
                //Retrieving the values to their variables using the keys
                saveData.LoadValue(ValuesKeys.playerPos, out playerPosition);

                /*for (enemyIndex = 0; enemyIndex <= enemiesPosition.Length; enemyIndex++){
                    saveData.LoadValue(ValuesKeys.enemiesPos, out enemiesPosition[enemyIndex]);
                }
                
                for (itemIndex = 0; itemIndex <= itensPosition.Length; itemIndex++){
                    saveData.LoadValue(ValuesKeys.itemPos, out itensPosition[itemIndex]);
                }
            
                for (doorIndex = 0; doorIndex <= doorsOpen.Length; doorIndex++){
                    saveData.LoadValue(ValuesKeys.doors, out doorsOpen[doorIndex]);
                } */
            
                //saveData.LoadValue(ValuesKeys.cheatControll, out cheatMode);
            }
        }
    }
}
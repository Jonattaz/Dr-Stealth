using UnityEngine;
using UnityEngine.SceneManagement;

namespace TylerCode.Examples
{
    public class SceneSwap : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                switch (SceneManager.GetActiveScene().buildIndex)
                {
                    case 0:
                        SceneManager.LoadScene(1);
                        break;

                    case 1:
                        SceneManager.LoadScene(0);
                        break;
                }
            }
        }
    }
}
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class Jeu : MonoBehaviour
{
    //=====Variables publiques=======

    //Temps minuterie
    public TMP_Text textTemps;
    float temps = 0;

    public GameObject UIVictoire;
    public GameObject UIDefaite;

    //fin
    bool jeuFini = false;

    float delaiRedemarrage = 5f;
    float tempsFin = 0;

    //son
    AudioSource musique;
    
    void Start()
    {
        //ne pas mettre les UI visibles
        UIVictoire.SetActive(false);
        UIDefaite.SetActive(false);

        musique = GetComponent<AudioSource>();
    }

    void Update()
    {
        //met le cronomettre, si perdu redemarre le jeu 
        if (!jeuFini)
        {

            temps += Time.deltaTime;
            //met le en letters, mais aussi enleve tout apres deciamls.
            textTemps.text = $"Temps: " + temps.ToString("F1");
        }
        else 
        {
            if (Time.time > tempsFin + delaiRedemarrage)
        {
                if (Keyboard.current.spaceKey.wasPressedThisFrame)
                {
                    RedemarrerScene();
                
                }
             }
          }
      }

                 
    ///=====Perdue=====
    public void Defaite()
    {
        if (jeuFini) return;
        jeuFini = true;

        musique.Stop();
        Debug.Log("LOSE!");
        UIDefaite.SetActive(true);

        tempsFin = Time.time;
    }

    ///===Fin de jeu====
    public void Victoire()
    {
        if (jeuFini) return;
        jeuFini = true;

        musique.Stop();
        Debug.Log("Yippie!");
        UIVictoire.SetActive(true);
        tempsFin = Time.time;
    }


    private void RedemarrerScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using TMPro;

public class Skieur : MonoBehaviour
{

    //======== Variables publiques =======/

    public float vitesse = 5f;

    public InputAction onDeplacementVertical;
    public InputAction onDeplacementHorizontal;


    //Points
    public int Points = 0;

    //Sons
    AudioSource sons;
    public AudioClip sonPointage;
    public AudioClip sonObstacles;

    //UI
    public TMP_Text PointageTexte;

    //===== Variables privées
    Rigidbody2D rigid;
    Jeu game;


    ///====== pas bas et haut (deplacement) ======
    float deplacementHor = 0;
    float deplacementVert = 0;

   
    void Start()
    {
        //fetch moi ses composants
        rigid = GetComponent<Rigidbody2D>();
        game = FindFirstObjectByType<Jeu>();
        sons = GetComponent<AudioSource>();
    }

    // Utiliser ces fonctions pour activer et désactiver les InputActions
    private void OnEnable()
    {
        onDeplacementHorizontal.Enable();
        onDeplacementVertical.Enable();
    }

    private void OnDisable()
    {
        onDeplacementHorizontal.Disable();
        onDeplacementVertical.Disable();
    }

    private void Update()
    {
        {
    /// Pour les movements de skieur
        deplacementHor = onDeplacementHorizontal.ReadValue<float>();
        deplacementVert = onDeplacementVertical.ReadValue<float>();

        if(deplacementVert > 0)
        {
            deplacementVert = 0;
        }
       
        // Definir la vélocité linéaire de skieur.
        rigid.linearVelocity = new Vector2(
            deplacementHor * vitesse,
            deplacementVert * vitesse
            );
    }}

    //une methode OnCollisionEnter2D (collison entre sapins) + collision de Yeti
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("Obstacle"))
        {
            rigid.linearVelocity = Vector2.zero;
            Debug.Log("Toucher!");
            sons.PlayOneShot(sonObstacles);
        }
        
        //collison de Yeti + UI
        if(collision.gameObject.tag == ("Danger"))
        {
            rigid.linearVelocity = Vector2.zero;
            rigid.constraints = RigidbodyConstraints2D.FreezeAll;

            game.Defaite();
        }
    }

    //une methode OnTriggerEnter2D (bon-homme de neige)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Points"))
        {
            collision.gameObject.SetActive(false);
            //Ajouter les points
            Points += 1;
            //TEXT UI
            PointageTexte.text = $"Points : {Points}";
            //ding! son
            sons.PlayOneShot(sonPointage);
        }

        //trigger collision finish
        if (collision.gameObject.tag == ("Finish"))
        {
            rigid.linearVelocity = Vector2.zero;
            rigid.constraints = RigidbodyConstraints2D.FreezeAll;

            game.Victoire();

        }
    }

    // Il faut appeller cette fonction dans la collision avec le yéti.
    void DeconnecterCamera()
    {
        Camera.main.GetComponent<PositionConstraint>().enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Female : MonoBehaviour
{
    private AudioSource wowSound;

    private void Start()
    {
        wowSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Mikkel")
        {
            wowSound.Play();
            StartCoroutine(CompleteGameWithDelay());
        }
    }

    private IEnumerator CompleteGameWithDelay()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

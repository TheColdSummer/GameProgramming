using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * This script manages the display of a message popup in the game.
 */
public class MessagePopup : MonoBehaviour
{
    private static MessagePopup _instance;
    private Text _messageText;
    private GameObject _canvasObj;
    private float _duration = 2f;

    public static void Show(string msg)
    {
        if (_instance == null)
        {
            GameObject msgPopup = new GameObject("MessagePopup");
            DontDestroyOnLoad(msgPopup);
            _instance = msgPopup.AddComponent<MessagePopup>();
            _instance.InitUI();
        }
        _instance.Display(msg);
    }

    private void InitUI()
    {
        _canvasObj = Instantiate(Resources.Load<GameObject>("PopupCanvas"));
        DontDestroyOnLoad(_canvasObj);

        _messageText = _canvasObj.transform.Find("PopupText").GetComponent<Text>();

        _messageText.gameObject.SetActive(false);
    }

    private void Display(string msg)
    {
        StopAllCoroutines();
        _messageText.text = msg;
        _messageText.gameObject.SetActive(true);
        StartCoroutine(HideAfterSeconds());
    }

    private IEnumerator HideAfterSeconds()
    {
        yield return new WaitForSeconds(_duration);
        _messageText.gameObject.SetActive(false);
    }
}
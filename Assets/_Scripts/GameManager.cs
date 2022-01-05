using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public Player player;
    public static UnityEvent<int> onUpdateHp = new UnityEvent<int>();
    public static UnityEvent<int> onUpdateLayer = new UnityEvent<int>();
    public static UnityEvent onDie = new UnityEvent();
    [SerializeField] GameObject hp_bar;
    [SerializeField] Text layer_text;

    public GameObject replay_obj;
    Button replay_button;
    int n_hp;

    // Start is called before the first frame update
    void Start()
    {
        n_hp = hp_bar.transform.childCount;
        replay_button = replay_obj.GetComponent<Button>();
        onUpdateHp.AddListener(onUpdateHpListener);
        onUpdateLayer.AddListener(onUpdateLayerListener);
        onDie.AddListener(onDieListener);
        replay_button.onClick.AddListener(replay);
    }

    void onUpdateHpListener(int hp)
    {
        for (int i = 0; i < n_hp; i++)
        {
            hp_bar.transform.GetChild(i).gameObject.SetActive(i < hp);
        }
    }

    void onUpdateLayerListener(int layer)
    {
        layer_text.text = $"¦a¤U {layer} ¼h";
    }

    void onDieListener()
    {
        replay_obj.SetActive(true);
    }

    void replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }
}

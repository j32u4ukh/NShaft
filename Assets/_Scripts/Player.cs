using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float moving_speed = 1f;
    [SerializeField] readonly int HP = 10;
    GameObject current_floor;
    SpriteRenderer sr;
    Animator animator;
    AudioSource audio_source;
    int hp;


    public enum Audio
    {
        [Description("touch_normal")]
        Normal,

        [Description("hurt")]
        Hurt,

        [Description("death")]
        Death
    }

    // Start is called before the first frame update
    void Start()
    {
        hp = HP;

        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audio_source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(moving_speed * Time.deltaTime, 0f, 0f);
            sr.flipX = false;
            animator.SetBool("run", true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Translate(-moving_speed * Time.deltaTime, 0f, 0f);
            sr.flipX = true;
            animator.SetBool("run", true);
        }
        else
        {
            animator.SetBool("run", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Ceiling"))
        {
            Debug.Log("Ceiling.");
            animator.SetTrigger("hurt");
            updateHp(-3);

            if (current_floor)
            {
                current_floor.GetComponent<BoxCollider2D>().enabled = false;
            }
        }

        // �ھڱ�Ĳ�I���k�u�A�P�_�b��Ĳ�������Ӥ�V
        else if (collision.contacts[0].normal == Vector2.up)
        {
            /* Debug.Log($"Point0: {collision.contacts[0].point}"); 
                * Debug.Log($"Point1: {collision.contacts[1].point}");
                * Debug.Log($"Point0 normal: {collision.contacts[0].normal}");
                * Debug.Log($"Point1 normal: {collision.contacts[1].normal}");*/
            current_floor = collision.gameObject;
            GameManager.onUpdateLayer.Invoke(current_floor.GetComponent<Floor>().layer);

            switch (collision.gameObject.tag)
            {
                case "Nails":
                    Debug.Log("Nails floor.");
                    animator.SetTrigger("hurt");
                    updateHp(-3);
                    break;
                default:
                    Debug.Log("Normal floor.");
                    updateHp(1);
                    playAudio(Audio.Normal);
                    break;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        current_floor = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);

        switch (collision.gameObject.name)
        {
            case "DeathLine":
            default:
                Debug.Log("DeathLine.");
                die();
                break;
        }
    }

    void updateHp(int change)
    {
        hp = Math.Min(HP, Math.Max(hp + change, 0));

        if (hp > 0)
        {
            playAudio(Audio.Hurt);
        }
        else
        {
            die();
        }

        GameManager.onUpdateHp.Invoke(hp);
    }

    void die()
    {
        GameManager.onDie.Invoke();
        playAudio(Audio.Death);
        Time.timeScale = 0f;
    }

    public void playAudio(Audio audio)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Audios/{getDescription_<Audio>(audio)}");
        audio_source.clip = clip;
        audio_source.Play();
    }

    /// <summary>
    /// �ʸ� getDescription�A²�ƿ�J���ѼƧΦ��A���o Enum �� Description �r��
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enum"></param>
    /// <returns></returns>
    public static string getDescription_<T>(Enum @enum)
    {
        return getDescription(@enum.ToString(), typeof(T));
    }

    /// <summary>
    /// ���o Enum �� Description �r��
    /// </summary>
    /// <param name="value">�r��Φ��� Enum</param>
    /// <param name="type">Enum ������</param>
    /// <returns></returns>
    public static string getDescription(string value, Type type)
    {
        string name = Enum.GetNames(type).Where(f => f.Equals(value, StringComparison.CurrentCultureIgnoreCase))
                                         .Select(d => d)
                                         .FirstOrDefault();

        //// ��L�۹������C�|
        if (name == null)
        {
            return string.Empty;
        }

        // �Q�ΤϮg��X�۹��������
        var field = type.GetField(name);

        // ���o���]�wDescriptionAttribute����
        var attribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

        //// �L�]�wDescription Attribute, �^��Enum���W��
        if (attribute == null || attribute.Length == 0)
        {
            return name;
        }

        //// �^��Description Attribute���]�w
        return ((DescriptionAttribute)attribute[0]).Description;
    }
}

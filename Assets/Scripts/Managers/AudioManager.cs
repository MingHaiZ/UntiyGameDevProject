using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMinimumDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;
    [SerializeField] private bool canPlaySFX;

    public bool playBGM;
    private int bgmIndex;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        } else
        {
            instance = this;
        }

        Invoke(nameof(AllowSFX), 1f);
    }

    private void Update()
    {
        if (!playBGM)
        {
            StopAllBGM();
        } else
        {
            if (!bgm[bgmIndex].isPlaying)
            {
                PlayBGM(bgmIndex);
            }
        }
    }

    public void PlaySFX(int _sfxIndex, Transform _source)
    {
        if (!canPlaySFX)
        {
            return;
        }

        if (sfx[_sfxIndex].isPlaying)
        {
            return;
        }

        if (_source != null &&
            Vector2.Distance(PlayerManager.instance.transform.position, _source.position) > sfxMinimumDistance)
        {
            return;
        }

        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(0.85f, 1.1f);
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _index) => sfx[_index].Stop();

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;

        StopAllBGM();

        bgm[bgmIndex].Play();
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    private void AllowSFX() => canPlaySFX = true;
}
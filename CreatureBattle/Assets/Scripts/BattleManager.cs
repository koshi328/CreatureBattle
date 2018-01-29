using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    enum RESULT
    {
        NONE,
        HUMAN_WIN,
        HUMAN_LOSE,
        DRAW,
    }

    [SerializeField]
    RESULT _result;

    [SerializeField]
    Text _resultText;

    [SerializeField]
    Actor[] _humans = new Actor[3];
    [SerializeField]
    Actor _monster;
    [SerializeField]
    Text _countDownText;
    [SerializeField]
    GameObject _startWall;
    bool _mineIsHuman;
    bool _started;


    void Awake()
    {
        _result = RESULT.NONE;
        _resultText.text = "";
        _resultText.color = Color.white;
        _mineIsHuman = true;
        _started = false;
    }

    void Update()
    {
        if (!_started) return;

        switch (_result)
        {
            case RESULT.HUMAN_WIN:
                if (_mineIsHuman) _resultText.text = "WIN";
                else _resultText.text = "LOSE";
                break;

            case RESULT.HUMAN_LOSE:
                if (_mineIsHuman) _resultText.text = "LOSE";
                else _resultText.text = "WIN";
                break;

            case RESULT.DRAW:
                _resultText.text = "DRAW";
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 生き残っているヒューマンがいるか
    /// </summary>
    /// <returns></returns>
    public bool HumanIsAlive()
    {
        for (int i = 0; i < 3; i++)
        {
            if (_humans[i] == null) return true;

            if (0.0f < _humans[i].GetStatus().GetHP())
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// モンスターが生き残っているか
    /// </summary>
    /// <returns></returns>
    public bool MonsterIsAlive()
    {
        if (_monster == null) return true;

        if (0.0f < _monster.GetStatus().GetHP())
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 戦闘中か
    /// </summary>
    /// <returns></returns>
    public bool IsBattling()
    {
        // 生き残ってるやつを調べる
        if (HumanIsAlive())
        {
            if (MonsterIsAlive())
            {
                _result = RESULT.NONE;
                return true;
            }
            else
            {
                _result = RESULT.HUMAN_WIN;
                return false;
            }
        }
        else
        {
            if (MonsterIsAlive())
            {
                _result = RESULT.HUMAN_LOSE;
            }
            else
            {
                _result = RESULT.DRAW;
            }
            return false;
        }
    }

    /// <summary>
    /// キャラクターをセット
    /// </summary>
    /// <param name="actor"></param>
    public void SetPlayer(Actor actor)
    {
        if (actor.gameObject.tag == "Human")
        {
            for (int i = 0; i < 3; i++)
            {
                if (_humans[i] == null)
                {
                    _humans[i] = actor;
                    break;
                }
            }
        }
        else
        {
            if (_monster == null)
            {
                _monster = actor;
                if(_monster.GetPhotonView().isMine)
                {
                    _mineIsHuman = false;
                }
            }
        }

        // Debug
        if (PhotonNetwork.room.PlayerCount == 1)
            StartCoroutine(CountDown());

        if (!_started)
        {
            for (int i = 0; i < 3; i++)
            {
                if (!_humans[i])
                    return;
            }

            if (!_monster)
                return;
            StartCoroutine(CountDown());
        }
    }

    IEnumerator CountDown()
    {
        for (int i = 3; i >= 1; i--)
        {
            _countDownText.text = i.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        _countDownText.text = "Start!";
        yield return new WaitForSeconds(0.4f);
        _countDownText.text = "";
        _started = true;
        _startWall.SetActive(false);
    }
}

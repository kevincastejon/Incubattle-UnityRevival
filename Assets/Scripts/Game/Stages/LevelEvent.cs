using UnityEngine;
public interface ILevelEvent
{
    public void Init();
    public bool IsOver();
}
public enum LevelEventType
{
    TEXT,
    ANIMATION,
    WAVE
}
[System.Serializable]
public class LevelEvent
{
    [SerializeField] private LevelEventType _type;
    [SerializeField] private TextScroller _text;
    [SerializeField] private AnimationPlayer _animation;
    [SerializeField] private EnemyWave _wave;
    [SerializeField] private bool _wait;

    public ILevelEvent Event
    {
        get
        {
            switch (_type)
            {
                case LevelEventType.TEXT:
                    return (_text);
                case LevelEventType.ANIMATION:
                    return (_animation);
                case LevelEventType.WAVE:
                    return (_wave);
                default:
                    return null;
            }
        }
    }

    public bool Wait { get => _wait; }
}

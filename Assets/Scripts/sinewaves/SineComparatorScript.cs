using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Sound;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

public class SineComparatorScript : MonoBehaviour
{
    public SoundSourceMasterOC SoundSourceMaster;
    public SinewaveScript target, response;
    public bool MatchIsAchieved;
    public float Epsilon = 0.2f;
    public float Tolerance = 0.25f;
    public int AttunementFrames = 10;
    public Color dotColorSuccess, responseDotColorNeutral, targetDotColorNeutral;
    public int FrameCountSinceMatch = 0;
    public UnityEvent MatchAchievedEvent;

    private PerpetualSoundEmitterOC _alignedEmitter;
    private PerpetualSoundEmitterOC _misalignedEmitter;

    void Start()
    {
        _alignedEmitter = SoundSourceMaster.StartPerpetualSound(PerpetualSoundKind.OscilloscopeAligned);
        _misalignedEmitter = SoundSourceMaster.StartPerpetualSound(PerpetualSoundKind.OscilloscopeMisaligned);
    }

    void Update()
    {
        int sum = 0;
        for (int i = 0; i < target.DotsCount; i++)
        {
            if (Mathf.Abs(target.getDotPosition(i).y - response.getDotPosition(i).y) < Epsilon)
            {
                sum++;
                response.setDotColor(i, dotColorSuccess);
                target.setDotColor(i, dotColorSuccess);
            }
            else
            {
                response.setDotColor(i, responseDotColorNeutral);
                target.setDotColor(i, targetDotColorNeutral);
            }
        }

        var matchFactor = Mathf.Clamp01(sum/(float)target.DotsCount);
        _alignedEmitter.ChangeVolumeMultiplication( Mathf.Clamp01(matchFactor+0.1f));
        _misalignedEmitter.ChangeVolumeMultiplication( Mathf.Clamp01(1-matchFactor+0.1f));

        if (matchFactor > (1f-Tolerance))
        {
            FrameCountSinceMatch++;
        }
        else
        {
            FrameCountSinceMatch = 0;
        }
        if (FrameCountSinceMatch >= AttunementFrames) MatchIsAchieved = true;
        else MatchIsAchieved = false;

        if (FrameCountSinceMatch == AttunementFrames)
        {
            MatchAchievedEvent.Invoke();
        }
    }

    public void GenerateTargetSineParameters(int id)
    {
        var random = new Random(id);
        target.sliderHandlerAmplitude((float) (random.NextDouble()*2)); //TODO: min and max values are equal to those from UI - they should be taken from this place, or some config file
        target.sliderHandlerPhase((float) (random.NextDouble()*2));
        target.sliderHandlerScale(0.1f + (float) (random.NextDouble()*1.9f));
    }
}

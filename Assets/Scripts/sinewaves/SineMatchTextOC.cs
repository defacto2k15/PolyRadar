using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.RadarBattleground;
using Assets.Scripts.Sound;
using UnityEngine;

namespace Assets.Scripts.sinewaves
{
    public class SineMatchTextOC : MonoBehaviour
    {
        public SoundSourceMasterOC SoundSourceMaster;
        public MaterialPropertyBlockColorSetterOC FriendTextObject;
        public MaterialPropertyBlockColorSetterOC FoeTextObject;

        public Color FriendInactiveColor;
        public Color FriendActiveColor;
        public Color FoeInactiveColor;
        public Color FoeActiveColor;

        public void Start()
        {
            ChangeIndicationMode(SineMatchTextIndicatorsMode.None);
        }

        public void ChangeIndicationMode(SineMatchTextIndicatorsMode mode)
        {
            if (mode == SineMatchTextIndicatorsMode.Friend)
            {
                SoundSourceMaster.StartOneShotSound(SingleShotSoundKind.TargetIsFriend);
            }else if (mode == SineMatchTextIndicatorsMode.Foe)
            {
                SoundSourceMaster.StartOneShotSound(SingleShotSoundKind.TargetIsFoe);
            }
            
            bool shouldFriendBeEnabled = mode == SineMatchTextIndicatorsMode.Friend;
            bool shouldFoeBeEnabled = mode == SineMatchTextIndicatorsMode.Foe;

            if (shouldFriendBeEnabled)
            {
                FriendTextObject.ChangeColorAndUpdate(FriendActiveColor);
            }
            else
            {
                FriendTextObject.ChangeColorAndUpdate(FriendInactiveColor);
            }

            if (shouldFoeBeEnabled)
            {
                FoeTextObject.ChangeColorAndUpdate(FoeActiveColor);
            }
            else
            {
                FoeTextObject.ChangeColorAndUpdate(FoeInactiveColor);
            }
        }

    }

    public enum SineMatchTextIndicatorsMode
    {
        Friend,
        Foe,
        None
    }
}

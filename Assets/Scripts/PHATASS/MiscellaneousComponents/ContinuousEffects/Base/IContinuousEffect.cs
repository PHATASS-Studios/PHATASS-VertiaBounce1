namespace PHATASS.Miscellaneous.ContinuousEffects
{
    //interface representing a continuous effect behaviour. Calling its float event will adjust its intensity
        interface IContinuousEffect : PHATASS.Utils.Events.IFloatEventReceiver
    {}
}
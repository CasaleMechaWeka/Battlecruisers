namespace UnityCommon.Matchers
{
    public interface IMatcher<TItem, TParameter>
    {
        bool IsMatch(TItem item, TParameter parameter);
    }
}
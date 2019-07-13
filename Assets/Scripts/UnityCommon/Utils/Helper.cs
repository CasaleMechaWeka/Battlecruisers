using UnityEngine.Assertions;

namespace UnityCommon.Utils
{
    public static class Helper
	{
        public static void AssertIsNotNull(params object[] objs)
        {
            foreach (object obj in objs)
            {
                Assert.IsNotNull(obj);
            }
        }
	}
}

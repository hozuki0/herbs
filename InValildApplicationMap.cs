using System;
using System.Collections.Generic;
using System.Text;

namespace Herbs
{
    /// <summary>
    /// 表示しない Application の BlackList
    /// </summary>
    public static class InValildApplicationMap
    {
        private static List<string> invalidApplications = new List<string>()
        {
            "Microsoft Store",
            "Microsoft Text Input Application"
        };

        public static bool IsInVaild(string appName)
        {
            return invalidApplications.Contains(appName);
        }
    }
}

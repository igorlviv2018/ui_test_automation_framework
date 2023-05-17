using System.Diagnostics;
using System.Linq;

namespace Taf.UI.Core.Helpers
{
    static class XUnitHelper
    {
        internal static string FactDisplayName()
        {
            var frame = new StackFrame(1, true);

            var method = frame.GetMethod();

            var attribute = method.GetCustomAttributes(typeof(Xunit.FactAttribute), true).First() as Xunit.FactAttribute;

            return attribute.DisplayName;
        }
    }
}

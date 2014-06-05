using System.Activities;
using System.Diagnostics;
using UseTemplateFactory.Extensions;

namespace UseTemplateFactory.Activities
{
/// <summary>
/// Throw a BreackException
/// </summary>
public sealed class Break : CodeActivity
{
    /// <summary>
    /// Execute
    /// </summary>
    /// <param name="context"></param>
    [DebuggerHidden()]
    protected override void Execute(CodeActivityContext context)
    {
        throw new BreackException();
    }

    /// <summary>
    /// Cache
    /// </summary>
    /// <param name="metadata"></param>
    protected override void CacheMetadata(CodeActivityMetadata metadata)
    {
        // Nothing to register
    }
}
}

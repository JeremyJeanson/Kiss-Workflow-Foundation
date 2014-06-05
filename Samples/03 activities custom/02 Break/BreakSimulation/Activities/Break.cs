using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Activities;
using BreakSimulation.Extensions;

namespace BreakSimulation.Activities
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

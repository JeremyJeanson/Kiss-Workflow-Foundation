using Demos.VersioningService.Extensions;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demos.VersioningService.Activities
{
    public sealed class UpdateIdParticipant:CodeActivity
    {
        public InArgument<Guid> Id { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            IdParticipant participant = context.GetExtension<IdParticipant>();
            if (participant == null) return;
            participant.Id = context.GetValue<Guid>(Id);
        }
    }
}
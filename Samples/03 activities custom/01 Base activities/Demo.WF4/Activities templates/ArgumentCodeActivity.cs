using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Activities.Validation;
using System.ComponentModel;

namespace Demo.WF4
{
    public sealed class ArgumentCodeActivity : CodeActivity
    {
        public InArgument<String> TextEntree { get; set; }
        public OutArgument<String> TextSortie { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // String entree = this.TextEntree.Get(context); 
            String entree = context.GetValue(this.TextEntree); 

            String sortie = String.Concat("Mon nouveau text ", entree);

            this.TextSortie.Set(context, sortie);
        }



        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            // Pour le momeent on en touche pas à la base
            base.CacheMetadata(metadata);

            if (this.TextEntree == null)
            {
                metadata.AddValidationError(
                    new ValidationError(
                        "La propriété [TextEntree] ne dois pas être vide.",
                        false,
                        "TextEntree"));
            }

            if (this.TextSortie == null)
            {
                metadata.AddValidationError(
                    new ValidationError(
                        "La propriété [TextSortie] ne dois pas être vide.",
                        false,
                        "TextSortie"));
            }
        }


    }
}

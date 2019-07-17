using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using DatabaseClient;

namespace DatabaseClient.EntityData
{

    [MetadataTypeAttribute(typeof(boring_bar.BoringBarMetadata))]
    public partial class boring_bar : BaseEntity
    {
        //    [RegularExpression(@"^(?=.*[a-z])(?=.*\d)[a-zA-Z\d]{8,}$",
        //ErrorMessage = "Password must contain, letters and numbers")]
        //    [Required(ErrorMessage = "Your password is too short")]
        //    [NotMapped]
        //    public string AnotherPasword { get; set; }

        //    private string anotherPasword;
        //    [RegularExpression(@"^(?=.*[a-z])(?=.*\d)[a-zA-Z\d]{8,}$",
        //ErrorMessage = "Password must contain, letters and numbers")]
        //    [Required(ErrorMessage = "Your password is too short")]
        //    [NotMapped]
        //    public string AnotherPasword
        //    {
        //        get { return anotherPasword; }
        //        set { anotherPasword = value; RaisePropertyChanged("Address1"); }
        //    }


        public void MetaSetUp()
        {
            // In wpf you need to explicitly state the metadata file.
            // Maybe this will be improved in future versions of EF.
            TypeDescriptor.AddProviderTransparent(
                new AssociatedMetadataTypeTypeDescriptionProvider(typeof(boring_bar),
                typeof(BoringBarMetadata)),
                typeof(boring_bar));
        }
        internal sealed class BoringBarMetadata
        {

            [Required(ErrorMessage="Model name required")]
            //[ExcludeChar("X", )]
            [StringLength(80, MinimumLength = 1, ErrorMessage = "Invalid model name")]
            public string Model { get; set; }
            public int OrdinalNumber { get; set; }

            private BoringBarMetadata()
            { }
        }
    }
}

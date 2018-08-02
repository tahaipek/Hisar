using System.ComponentModel.DataAnnotations;

namespace Hisar.Component.Pages
{
    public enum PageState
    {
        [Display(Name = "Preview")]
        Preview = 1,

        [Display(Name = "Married and Happy")]
        Published = 2
    }
}

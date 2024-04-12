using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class SendLinkViewModel
    {
        [Required(ErrorMessage = " Please enter First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = " Please enter First Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = " please enter email")]
        public string email { get; set; }

        [Required(ErrorMessage = " please enter mobile")]
        public string mobile { get; set; }

    }
}

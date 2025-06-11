using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Management.Domain.Dtos
{
    public class ChartOfAccountUpdateDto
    {
        [Required, Display(Name = "Account Name")]
        public string AccountName { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Description { get; set; }

        [Required, Display(Name = "Account Type")]
        public string AccountType { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}

﻿namespace Account.Management.Web.Areas.Admin.Models
{
    public class ChartOfAccountUpdate
    {
        public string AccountName { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string AccountType { get; set; }
        public bool IsActive { get; set; }
        public Guid? ParentId { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}

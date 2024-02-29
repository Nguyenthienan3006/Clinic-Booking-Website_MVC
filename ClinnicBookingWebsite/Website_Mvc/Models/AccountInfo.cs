using System;
using System.Collections.Generic;

namespace Website_Mvc.Models
{
    public partial class AccountInfo
    {
        public int IdUser { get; set; }
        public string? Phonenumber { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }

        public virtual Account IdUserNavigation { get; set; } = null!;
    }
}

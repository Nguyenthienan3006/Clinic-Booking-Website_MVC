using System;
using System.Collections.Generic;

namespace Website_Mvc.Models
{
    public partial class Roll
    {
        public Roll()
        {
            Accounts = new HashSet<Account>();
        }

        public int RollId { get; set; }
        public string? RollName { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Capstone6.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Task
    {
        public string UserName { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public System.DateTime DueDate { get; set; }
        public string Status { get; set; }
    
        public virtual User User { get; set; }
    }
}

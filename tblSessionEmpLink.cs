//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ScheduleAutomation
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblSessionEmpLink
    {
        public System.Guid empID { get; set; }
        public System.Guid sessionID { get; set; }
        public string typeOfParticipation { get; set; }
    
        public virtual tblEmployee tblEmployee { get; set; }
        public virtual tblSession tblSession { get; set; }
    }
}

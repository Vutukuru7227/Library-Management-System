//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LibraryManagmentSystemFinal
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FINES")]
    public partial class FINE
    {
        public Nullable<int> LOAN_ID { get; set; }
        public decimal FINE_AMT { get; set; }
        public Nullable<System.DateTime> PAID { get; set; }
    
        public virtual BOOK_LOANS BOOK_LOANS { get; set; }
    }
}

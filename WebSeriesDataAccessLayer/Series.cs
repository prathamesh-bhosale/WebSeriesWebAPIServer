//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebSeriesDataAccessLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class Series
    {
        public int id { get; set; }
        public string name { get; set; }
        public string categories { get; set; }
        public string language { get; set; }
        public string story { get; set; }
        public Nullable<bool> forkid { get; set; }
        public string imageurl { get; set; }
        public string videourl { get; set; }
    }
}
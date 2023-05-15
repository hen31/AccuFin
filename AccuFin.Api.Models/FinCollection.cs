using System;
using System.Collections.Generic;
using System.Text;

namespace AccuFin.Api.Models
{
    public class FinCollection<T>
    {
        public List<T> Items { get; set; }
        public int Count { get; set; }
        public int Page { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AccuFin.Api.Models.BankIntegration
{
    public class BankModel
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string Bic { get; set; }
        public string Transaction_total_days { get; set; }
        public string[] Countries { get; set; }
        public string Logo { get; set; }

    }
}

using System;

namespace AccuFin.Api.Models
{
    public class AdministrationCollectionItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AdministrationRegistryCode { get; set; }
        public string ImageFileName { get; set; }
    }
}

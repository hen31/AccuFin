namespace AccuFin.Data.Entities
{
    public class Administration : BaseEntityGuidId
    {
        public string Name { get; set; }
        /// <summary>
        /// KVK nummer
        /// </summary>
        public string AdministrationRegistryCode { get; set; }
       
        public string TelephoneNumber { get; set; }
        public string EmailAdress { get; set; }
    }
}

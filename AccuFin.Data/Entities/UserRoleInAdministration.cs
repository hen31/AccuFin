namespace AccuFin.Data.Entities
{
    [Flags]
    public enum UserRoleInAdministration
    {
        None = 0,
        Owner = 1,
        Employee = 2,
        Controller = 4,
        SalaryAdmin = 8,
        Sales = 16,
        Management = 32
    }
}

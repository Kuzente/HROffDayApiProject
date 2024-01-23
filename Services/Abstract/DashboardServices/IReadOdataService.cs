namespace Services.Abstract.DashboardServices;

public interface IReadOdataService
{
    Task<IQueryable> GetBranchesOdataService(); // Odata Şube Servisi
    Task<IQueryable> GetPositionOdataService(); // Odata Şube Servisi
    Task<IQueryable> GetPersonalOdataService(); // Odata Şube Servisi
}
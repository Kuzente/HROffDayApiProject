﻿namespace Services.Abstract.DashboardServices;

public interface IReadOdataService
{
    Task<IQueryable> GetBranchesOdataService(); // Odata Şube Servisi
    Task<IQueryable> GetAllBranchesOdataService();
    Task<IQueryable> GetPositionOdataService(); // Odata Şube Servisi
    Task<IQueryable> GetAllPositionOdataService();
    Task<IQueryable> GetPersonalOdataService(); // Odata Şube Servisi
    Task<IQueryable> GetWaitingOffDaysService(bool directorRole,List<Guid>? branches); // Odata Şube Servisi
    Task<IQueryable> GetApprovedDaysService(bool directorRole,List<Guid>? branches); // Odata Şube Servisi
    Task<IQueryable> GetMissingDayService(); // Odata Şube Servisi
    Task<IQueryable> GetPersonalCumulativesService(); // Odata Şube Servisi
}
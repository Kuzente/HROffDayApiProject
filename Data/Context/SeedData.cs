﻿using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public static class SeedData
{
    public static void Seed(this ModelBuilder modelBuilder,string pass)
    {
        var sss = 
        modelBuilder.Entity<User>().HasData(
            new User
            {
                ID = Guid.NewGuid(),
                Username = "superadmin",
                Email = "superadmin@superadmin.com",
                Password = pass,
                Role = UserRoleEnum.SuperAdmin,
                Status = EntityStatusEnum.Online,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                DeletedAt = DateTime.MinValue,
                IsDefaultPassword = true,
                MailVerificationToken = "-",
                TokenExpiredDate = DateTime.MaxValue,
                // Diğer özellikler
            }
        );
        
    }
}
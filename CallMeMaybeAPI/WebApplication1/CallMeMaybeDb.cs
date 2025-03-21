﻿using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;


public class CallMeMaybeDbContext : DbContext
{
    public CallMeMaybeDbContext(DbContextOptions<CallMeMaybeDbContext> options) : base(options) { }

    public DbSet<Service> Service { get; set; }
    public DbSet<Site> Site { get; set; }

    public DbSet<Salarie> Salarie { get; set; }

}

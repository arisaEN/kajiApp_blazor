using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace kajiApp_blazor.Components.Models;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }


    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Eat> Eats { get; set; }

    public virtual DbSet<EatDetail> EatDetails { get; set; }

    public virtual DbSet<LifeDetail> LifeDetails { get; set; }

    public virtual DbSet<LifeDetailSummary> LifeDetailSummaries { get; set; }

    public virtual DbSet<MonthlyWorkSummaryView> MonthlyWorkSummaryViews { get; set; }

    public virtual DbSet<NameList> NameLists { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Work> Works { get; set; }

    public virtual DbSet<WorkList> WorkLists { get; set; }

    public virtual DbSet<家事分類区分> 家事分類区分s { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=database.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Eat>(entity =>
        {
            entity.ToTable("eat");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Month).HasColumnName("month");
            entity.Property(e => e.Year).HasColumnName("year");
            entity.Property(e => e.Yyyymm).HasColumnName("yyyymm");
        });

        modelBuilder.Entity<EatDetail>(entity =>
        {
            entity.ToTable("eat_detail");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.InputTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("TIMESTAMP")
                .HasColumnName("input_time");
            entity.Property(e => e.Month).HasColumnName("month");
            entity.Property(e => e.Year).HasColumnName("year");
            entity.Property(e => e.Yyyymm).HasColumnName("yyyymm");
        });

        modelBuilder.Entity<LifeDetail>(entity =>
        {
            entity.ToTable("life_detail");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Electricity).HasColumnName("electricity");
            entity.Property(e => e.Gas).HasColumnName("gas");
            entity.Property(e => e.InputTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("TIMESTAMP")
                .HasColumnName("input_time");
            entity.Property(e => e.Month).HasColumnName("month");
            entity.Property(e => e.Rent).HasColumnName("rent");
            entity.Property(e => e.Water).HasColumnName("water");
            entity.Property(e => e.Year).HasColumnName("year");
            entity.Property(e => e.Yyyymm).HasColumnName("yyyymm");
        });

        modelBuilder.Entity<LifeDetailSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("life_detail_summary");

            entity.Property(e => e.Yyyymm).HasColumnName("yyyymm");
            entity.Property(e => e.生活費食費).HasColumnName("生活費_食費");
            entity.Property(e => e.荻田).HasColumnName("荻田%");
        });

        modelBuilder.Entity<MonthlyWorkSummaryView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("monthly_work_summary_view");

            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Percentage)
                .HasColumnType("INT")
                .HasColumnName("percentage");
            entity.Property(e => e.TotalPoints).HasColumnName("total_points");
            entity.Property(e => e.Yyyymm).HasColumnName("yyyymm");
        });

        modelBuilder.Entity<NameList>(entity =>
        {
            entity.HasKey(e => e.NameId);

            entity.ToTable("nameList");

            entity.Property(e => e.NameId).HasColumnName("name_id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("payment");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.InputTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("TIMESTAMP")
                .HasColumnName("input_time");
            entity.Property(e => e.Month).HasColumnName("month");
            entity.Property(e => e.NameCode).HasColumnName("name_code");
            entity.Property(e => e.Pay).HasColumnName("pay");
            entity.Property(e => e.Year).HasColumnName("year");
            entity.Property(e => e.Yyyymm)
                .HasComputedColumnSql()
                .HasColumnName("yyyymm");
        });

        modelBuilder.Entity<Work>(entity =>
        {
            entity.ToTable("works");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Day)
                .HasColumnType("date")
                .HasColumnName("day");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Percent).HasColumnName("percent");
            entity.Property(e => e.Work1).HasColumnName("work");
            entity.Property(e => e.WorkId).HasColumnName("work_id");
        });

        modelBuilder.Entity<WorkList>(entity =>
        {
            entity.HasKey(e => e.WorkId);

            entity.ToTable("workList");

            entity.Property(e => e.WorkId).HasColumnName("work_id");
            entity.Property(e => e.WorkName).HasColumnName("workName");
            entity.Property(e => e.WorkNamePoint).HasColumnName("workNamePoint");
        });

        modelBuilder.Entity<家事分類区分>(entity =>
        {
            entity.ToTable("家事分類区分");

            entity.Property(e => e.家事分類区分id).HasColumnName("家事分類区分ID");
            entity.Property(e => e.区分名).HasColumnType("varchar(40)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

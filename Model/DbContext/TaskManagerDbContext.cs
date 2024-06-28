using System.Reflection;
using Microsoft.EntityFrameworkCore;
namespace AdministradorDeTareas.Model;

    public partial class TaskManagerDbContext : DbContext
    { 
        private string database = "TaskManagerDb.db";
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(connectionString: "Filename=" + database,
                sqliteOptionsAction: op => {
                    op.MigrationsAssembly(
                        Assembly.GetExecutingAssembly().FullName
                    );
                });
            base.OnConfiguring(optionsBuilder);
        }
    public virtual DbSet<PriorityModel> Priorities { get; set; }

    public virtual DbSet<TaskModel> Tasks { get; set; }

    public virtual DbSet<TaskStatusModel> TaskStatuses { get; set; }

    public virtual DbSet<UsersModel> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PriorityModel>(entity =>
        {
            entity.HasKey(e => e.PriorityID).HasName("PK__Priority__D0A3D0DE9CF1C8B1");

            entity.ToTable("Priority");

            entity.Property(e => e.PriorityID).HasColumnName("PriorityID");
            entity.Property(e => e.PriorityStatus)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TaskModel>(entity =>
        {
            entity.HasKey(e => e.TaskID).HasName("PK__Tasks__7C6949D1A892586A");

            entity.Property(e => e.TaskID).HasColumnName("TaskID");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PriorityID).HasColumnName("PriorityID");
            entity.Property(e => e.StatusID).HasColumnName("StatusID");
            entity.Property(e => e.Title)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.UserID).HasColumnName("UserID");

            entity.HasOne(d => d.Priority).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.PriorityID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_PriorityID");

            entity.HasOne(d => d.TaskStatus).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.StatusID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StatusID");

            entity.HasOne(d => d.Users).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.UserID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserID");
        });

        modelBuilder.Entity<TaskStatusModel>(entity =>
        {
            entity.HasKey(e => e.StatusID).HasName("PK__TaskStat__C8EE2043E7FAF61A");

            entity.ToTable("TaskStatus");

            entity.Property(e => e.StatusID).HasColumnName("StatusID");
            entity.Property(e => e.StatusName)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UsersModel>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC1E276F31");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email)
                .HasMaxLength(75)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

     partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    
}
using Lib.DataAccess.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SSO_Api.Models;

namespace SSO_Api.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    //public virtual DbSet<T_User> TUsers { get; set; }
    public DbSet<TUser> TUsers { get; set; }
    public DbSet<TDeXuat> TDeXuats { get; set; }

    public virtual DbSet<TRoleGroup> TRoleGroups { get; set; }

    public virtual DbSet<TRefRoleGroupFunction> TRefRoleGroupFunctions { get; set; }

    public virtual DbSet<TRefUserRoleGroup> TRefUserRoleGroups { get; set; }

    public virtual DbSet<TFunction> TFunctions { get; set; }

    public virtual DbSet<TMenu> TMenus { get; set; }

    public virtual DbSet<TUserType> TUserTypes { get; set; }

    public virtual DbSet<TFile> TFiles { get; set; }

    public virtual DbSet<TPoData> TPoDatas { get; set; }

    public virtual DbSet<TPoFlowStage> TPoFlowStages { get; set; }

    public virtual DbSet<TRefFilePo> TRefFilePos {  get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ví dụ: đặt tên bảng khác
       
    }
}

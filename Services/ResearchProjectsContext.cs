using Microsoft.EntityFrameworkCore;
using ResearchProjectManagementSystem.Models;

namespace ResearchProjectManagementSystem.Services
{
    public class ResearchProjectsContext : DbContext
    {
        public DbSet<ResearchProjectModel> ResearchProjects { get; set; }
        public DbSet<StageModel> Stages { get; set; }
        public DbSet<AcquisitionModel> Acquisitions { get; set; }
        public DbSet<ReportModel> Reports { get; set; }
        public DbSet<ResearchResultModel> ResearchResults { get; set; }
        public DbSet<ResourceModel> Resources { get; set; }
        public DbSet<ResourceAllocationModel> ResourceAllocations { get; set; }
        public DbSet<BudgetModel> Budgets { get; set; }
        public DbSet<TeamMemberModel> TeamMembers { get; set; }
        public DbSet<TeamMemberRoleModel> TeamMemberRoles { get; set; }
        public DbSet<TeamMemberTaskModel> TeamMemberTasks { get; set; }
        public DbSet<UserModel> Users { get; set; }

        public ResearchProjectsContext(DbContextOptions<ResearchProjectsContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResearchProjectModel>()
                .HasMany(p => p.TeamMembers)
                .WithOne(t => t.ResearchProject)
                .HasForeignKey(t => t.IdResearchProject);
            modelBuilder.Entity<ResearchProjectModel>()
                .HasMany(p => p.ResourceAllocations)
                .WithOne(a => a.ResearchProject)
                .HasForeignKey(a => a.IdResearchProject);
            modelBuilder.Entity<ResearchProjectModel>()
                .HasMany(p => p.Stages)
                .WithOne(s => s.ResearchProject)
                .HasForeignKey(s => s.IdResearchProject);
            modelBuilder.Entity<ResearchProjectModel>()
                .HasMany(p => p.Acquisitions)
                .WithOne(a => a.ResearchProject)
                .HasForeignKey(a => a.IdResearchProject);
            modelBuilder.Entity<ResearchProjectModel>()
                .HasOne(p => p.Budget)
                .WithOne()
                .HasForeignKey<ResearchProjectModel>(p => p.IdBudget);

            modelBuilder.Entity<TeamMemberModel>()
                .HasOne(m => m.Researcher)
                .WithMany(u => u.Memberships)
                .HasForeignKey(m => m.IdResearcher);
            modelBuilder.Entity<TeamMemberModel>()
                .HasMany(m => m.TeamMemberTasks)
                .WithOne(t => t.TeamMember)
                .HasForeignKey(t => t.IdTeamMember);
            modelBuilder.Entity<TeamMemberModel>()
                .HasOne(m => m.TeamMemberRole)
                .WithOne(r => r.TeamMember)
                .HasForeignKey<TeamMemberRoleModel>(r => r.IdTeamMember);

            modelBuilder.Entity<ResourceAllocationModel>()
                .HasOne(a => a.Resource)
                .WithMany(r => r.ResourceAllocations)
                .HasForeignKey(a => a.IdResource);

            modelBuilder.Entity<StageModel>()
                .HasMany(s => s.Reports)
                .WithOne(r => r.ProjectStage)
                .HasForeignKey(r => r.IdProjectStage);
            modelBuilder.Entity<StageModel>()
                .HasMany(s => s.ResearchResults)
                .WithOne(r => r.ProjectStage)
                .HasForeignKey(r => r.IdProjectStage);
            modelBuilder.Entity<StageModel>()
                .HasOne(s => s.Budget)
                .WithOne()
                .HasForeignKey<StageModel>(s => s.IdBudget);
        }
    }
}

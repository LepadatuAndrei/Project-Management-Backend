namespace ResearchProjectManagementSystem.Payloads
{
    public class TeamMemberRolePayload
    {
        public int IdRole { get; set; }
        public string RoleName { get; set; }
        public string? RoleDescription { get; set; }
        public int? IdTeamMember { get; set; }
    }
}

using ProjectName.DataAccess.Enums;

namespace ProjectName.Entities;

public class Admin : User
{
    public override Role Role => Role.Admin;
}
using ProjectName.DataAccess.Enums;

namespace ProjectName.DataAccess.Entities;

public class Customer : User
{
    public override Role Role => Role.Customer;
}

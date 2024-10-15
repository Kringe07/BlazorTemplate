using ProjectName.DataAccess.Enums;

namespace ProjectName.Entities;

public class Customer : User
{
    public override Role Role => Role.Customer;
}

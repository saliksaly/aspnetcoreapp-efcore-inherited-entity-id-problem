namespace aspnetcoreapp_efcore_inherited_entity_id_problem.Data
{
    public abstract class AnimalBase : Entity<int>
    {
        public string Name { get; set; }
    }

    public class Cat : AnimalBase
    {
    }

    public class Dog : AnimalBase
    {
    }
}

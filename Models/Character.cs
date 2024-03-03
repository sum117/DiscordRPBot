public class Character
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required int Level { get; set; }

    public required ulong OwnerId { get; set; }

}

public class AnimalBlock : Block
{
    //override TakeDamage
    //Inventory add animal block
    public AnimalType animalType;

    public override void TakeDamage(int damage)
    {
        AnimalBar.Instance.AddToAnimalList(animalType);
        base.TakeDamage(damage);
    }
}

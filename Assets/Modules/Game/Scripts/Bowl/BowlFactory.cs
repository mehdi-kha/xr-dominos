using Zenject;

class BowlFactory : PlaceholderFactory<BowlController>
{
    public BowlController Create(IDesk desk)
    {
        var bowlController = base.Create();
        bowlController.CorrespondingDesk = desk;
        return bowlController;
    }
}

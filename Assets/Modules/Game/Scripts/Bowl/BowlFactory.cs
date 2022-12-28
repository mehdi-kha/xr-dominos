using Zenject;

class BowlFactory : PlaceholderFactory<BowlController>
{
    public BowlController Create(IDesk desk)
    {
        var bowlController = base.Create();
        bowlController.CorrespondingDesk = desk;
        desk.Bowl = bowlController;
        return bowlController;
    }
}

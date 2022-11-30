using Zenject;

class DominosFactory : PlaceholderFactory<DominosController>
{
    public DominosController Create(IDesk desk)
    {
        var bowlController = base.Create();
        bowlController.CorrespondingDesk = desk;
        return bowlController;
    }
}

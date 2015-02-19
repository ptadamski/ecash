namespace Bank.Interface
{
    public interface IFactory<In, Out>
    {
        Out Construct(In aInput);
        void Destruct(Out aItem);
    }
}

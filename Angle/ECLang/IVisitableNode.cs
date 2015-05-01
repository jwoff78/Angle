namespace ECLang
{
    public interface IVisitableNode
    {
        void Accept(IVisitor v);
    }
}
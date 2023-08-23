namespace APIs.Contract
{
    public interface IMapper<TModel, TViewModel>
    {
        TViewModel Map(TModel model);
        TModel Map(TViewModel viewModel);
    }
}

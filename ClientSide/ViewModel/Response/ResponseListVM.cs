namespace ClientSide.ViewModel.Response
{
    public class ResponseListVM<Entity>
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public List<Entity>? Data { get; set; }
    }
}

namespace ClientSide.ViewModel.Response
{
    public class ResponseViewModel<Entity>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public Entity Data { get; set; }
    }
}

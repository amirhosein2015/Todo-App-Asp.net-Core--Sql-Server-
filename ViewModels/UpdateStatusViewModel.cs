namespace TodoApp.ViewModels
{

    //بصورت پابلیک تعریف شده تا در سرتاسر پروژه در دسترس باشه//
    public class UpdateStatusViewModel
    {
        public int Id { get; set; }
      //نام فیلدی که باید تغییر کنه//
        public string Field { get; set; }
        public bool Value { get; set; }
    }

}
